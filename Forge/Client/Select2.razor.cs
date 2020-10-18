using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NEvaldas.Blazor.Select2.Models;

namespace Forge.Client
{
    public class Select2Base<TItem> : ComponentBase, IDisposable
    {
        private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
        private readonly JsonSerializerOptions _jsonSerializerOptions =
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        [Inject] private IJSRuntime JSRuntime { get; set; }
        private DotNetObjectReference<Select2Base<TItem>> _elementRef;
        private bool _previousParsingAttemptFailed;
        private ValidationMessageStore _parsingValidationMessages;
        private Type _nullableUnderlyingType;

        [CascadingParameter] public EditContext CascadingEditContext { get; set; }

        [Parameter] public EditContext EditContext { get; set; }

        [Parameter] public string Id { get; set; }

        [Parameter] public bool IsDisabled { get; set; }

        [Parameter] public Func<TItem, bool> IsOptionDisabled { get; set; } = item => false;

        [Parameter] public List<TItem> Data { get; set; }

        [Parameter] public Func<Select2QueryData, Task<List<TItem>>> GetPagedData { get; set; }

        [Parameter] public Func<TItem, string> OptionTemplate { get; set; }

        [Parameter] public Func<TItem, string> TextExpression { get; set; } = item => item.ToString();

        [Parameter] public string Placeholder { get; set; } = "Select value";

        [Parameter] public string Theme { get; set; } = "bootstrap4";

        [Parameter] public string Class { get; set; } = "";

        [Parameter] public bool AllowClear { get; set; }
        /// <summary>
        /// Gets or sets an expression that identifies the bound id.
        /// </summary>
        [Parameter] public Expression<Func<TItem, string>> IdEpxression { get; set; }
        /// <summary>
        /// Gets or sets an expression that identifies the bound value.
        /// </summary>
        [Parameter] public Expression<Func<List<TItem>>> ValueExpression { get; set; }

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public List<TItem> Value { get; set; }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter] public EventCallback<List<TItem>> ValueChanged { get; set; }

        public void Refresh()
        {
            StateHasChanged();
        }

        /// <summary>
        /// Constructs an instance of <see cref="Select2Base{TItem}"/>.
        /// </summary>
        protected Select2Base()
        {
            _validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
        }

        protected Dictionary<string, TItem> InternallyMappedData { get; set; } = new Dictionary<string, TItem>();

        protected string FieldClass => GivenEditContext?.FieldCssClass(FieldIdentifier) ?? string.Empty;

        protected EditContext GivenEditContext { get; set; }

        /// <summary>
        /// Gets the <see cref="FieldIdentifier"/> for the bound value.
        /// </summary>
        protected FieldIdentifier FieldIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the current value of the input.
        /// </summary>
        protected List<TItem> CurrentValue
        {
            get => Value;
            set
            {
                _ = SelectItems(value);

                var hasChanged = false;

                if (Value != null)
                {
                    var firstNotSecond = value.Except(Value).ToList();
                    var secondNotFirst = Value.Except(value).ToList();
                    hasChanged = (firstNotSecond.Any() && secondNotFirst.Any()) || value.Count != Value.Count;
                    Console.WriteLine($"CurrentValue -> set -> hasChanged([{String.Join(',', value)}], [{String.Join(',', Value)}]) = {hasChanged}");
                }
                else
                {
                    hasChanged = Value != value;
                }
                if (!hasChanged) return;

                Value = value;
                Console.WriteLine($"CurrentValue -> set -> Value = {JsonSerializer.Serialize(value)}");
                _ = ValueChanged.InvokeAsync(value);
                GivenEditContext?.NotifyFieldChanged(FieldIdentifier);
            }
        }

        protected bool TryParseValuesFromString(string[] values, out List<TItem> result)
        {
            result = new List<TItem>();
            bool overallSuccess = true;

            if (values.Length == 0)
                return AllowClear;

            foreach (var value in values)
            {
                if (!InternallyMappedData.ContainsKey(value))
                    return false;

                result.Add(InternallyMappedData[value]);
            }
            return overallSuccess;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _elementRef = DotNetObjectReference.Create(this);

            if (Value == null)
                throw new NullReferenceException("Value of Select2 should not be null at any time");
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            Console.WriteLine($"SetParametersAsync -> Called");
            Console.WriteLine($"SetParametersAsync -> Data: {JsonSerializer.Serialize(Data)}");

            parameters.SetParameterProperties(this);

            FieldIdentifier = FieldIdentifier.Create(ValueExpression);
            _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TItem));
            GivenEditContext = EditContext ?? CascadingEditContext;
            if (GivenEditContext != null)
                GivenEditContext.OnValidationStateChanged += _validationStateChangedHandler;

            GetPagedData ??= GetStaticData;

            CurrentValue = ValueExpression.Compile().Invoke();

            // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
            return base.SetParametersAsync(ParameterView.Empty);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                Console.WriteLine($"OnAfterRenderAsync -> First Render");
                var options = JsonSerializer.Serialize(new
                {
                    placeholder = Placeholder,
                    allowClear = AllowClear,
                    theme = Theme
                }, _jsonSerializerOptions);

                await JSRuntime.InvokeVoidAsync("select2Blazor.init",
                    Id, _elementRef, options, "select2Blazor_GetData");

                if (CurrentValue != null)
                    await SelectItems(CurrentValue);

                await JSRuntime.InvokeVoidAsync("select2Blazor.onChange",
                    Id, _elementRef, "select2Blazor_OnChange");
            }
        }

        private Task<List<TItem>> GetStaticData(Select2QueryData query)
        {
            if (query.Page != 1) 
                return Task.FromResult(default(List<TItem>));

            var data = Data;
            Console.WriteLine($"GetStaticData -> [{JsonSerializer.Serialize(Data)}]");
            var searchTerm = query.Term;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                data = data
                    .Where(x => TextExpression(x).Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return Task.FromResult(data);
        }

        private async Task SelectItems(List<TItem> items)
        {
            foreach (var item in items)
            {
                var mappedItem = MapToSelect2Item(item);
                Console.WriteLine($"SelectItems -> Selecting item: {item.ToString()}");
                InternallyMappedData[mappedItem.Id] = item;
                await JSRuntime.InvokeVoidAsync("select2Blazor.select", Id, mappedItem);
            }
        }

        internal Select2Item MapToSelect2Item(TItem item)
        {
            try
            {
                var id = IdEpxression == null ? GetId(item) : IdEpxression.Compile().Invoke(item);
                var text = TextExpression(item);
                var select2Item = new Select2Item(id, text, IsOptionDisabled(item));
                Console.WriteLine($"Mapping item - id: {id}, text: {text}");
                if (OptionTemplate != null)
                    select2Item.Html = OptionTemplate(item);
                if (Value.Count > 0)
                {
                    foreach (var selectedItem in Value)
                    {
                        var valueId = IdEpxression == null ? GetId(selectedItem) : IdEpxression.Compile().Invoke(selectedItem);
                        if (valueId == id)
                        {
                            select2Item.Selected = true;
                        }
                    }
                }
                return select2Item;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.ToString()}, {ex.StackTrace}");
                return null;
            }
        }

        [JSInvokable("select2Blazor_GetData")]
        public async Task<string> Select2_GetDataWrapper(JsonElement element)
        {
            Console.WriteLine("Select2_GetDataWrapper -> Called");

            var json = element.GetRawText();
            var queryParams = JsonSerializer.Deserialize<Select2QueryParams>(json, _jsonSerializerOptions);

            var data = await GetPagedData(queryParams.Data);

            if (!queryParams.Data.Type.Contains("append", StringComparison.OrdinalIgnoreCase))
                InternallyMappedData.Clear();

            var response = new Select2Response();
            if (data != null)
            {
                foreach (var item in data)
                {
                    var mappedItem = MapToSelect2Item(item);
                    Console.WriteLine($"Rendering item - id: {mappedItem.Id}, text: {mappedItem.Text}");
                    InternallyMappedData[mappedItem.Id] = item;
                    response.Results.Add(mappedItem);
                }
                response.Pagination.More = data.Count == queryParams.Data.Size;
            }

            var jsonResult = JsonSerializer.Serialize(response, _jsonSerializerOptions);
            Console.WriteLine($"Select2_GetDataWrapper -> Returning data: {jsonResult}");
            return jsonResult;
        }

        [JSInvokable("select2Blazor_OnChange")]
        public void Change(string[] value)
        {
            Console.WriteLine($"Select.Change -> Value: [{String.Join(',', value)}]");
            _parsingValidationMessages?.Clear();

            bool parsingFailed;

            if (value.Length == 0)
            {
                parsingFailed = false;
                CurrentValue = new List<TItem>();
            }
            else if (TryParseValuesFromString(value, out var parsedValue))
            {
                parsingFailed = false;
                CurrentValue = parsedValue;
            }
            else
            {
                parsingFailed = true;

                if (_parsingValidationMessages == null)
                {
                    _parsingValidationMessages = new ValidationMessageStore(GivenEditContext);
                }

                _parsingValidationMessages.Add(FieldIdentifier, "Given value was not found");

                // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                GivenEditContext?.NotifyFieldChanged(FieldIdentifier);
            }

            // We can skip the validation notification if we were previously valid and still are
            if (parsingFailed || _previousParsingAttemptFailed)
            {
                GivenEditContext?.NotifyValidationStateChanged();
                _previousParsingAttemptFailed = parsingFailed;
            }
        }

        private static string GetId(TItem item) => item.GetHashCode().ToString();

        protected virtual void Dispose(bool disposing)
        {
        }

        void IDisposable.Dispose()
        {
            if (GivenEditContext != null)
            {
                GivenEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
            }

            Dispose(disposing: true);
        }
    }
}

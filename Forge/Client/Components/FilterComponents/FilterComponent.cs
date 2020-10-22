using Forge.Shared.Filters;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Client.Components.FilterComponents
{
    public abstract class FilterComponent<TModel, TValue>: ComponentBase where TModel: BaseFilter
    {
        [Parameter]
        public TModel Model { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public Action<TModel, TValue> ModelUpdate { get; set; }

        [Parameter]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [CascadingParameter]
        public CollapsableFilter<TModel> CollapsableFilter { get; set; }
    }
}

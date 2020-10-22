using Forge.Client.Components;
using Forge.Client.Models;
using Forge.Shared.Filters;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Client.Services
{
    public class DatatableService
    {
        private readonly IJSRuntime _jsRuntime;

        public DatatableService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async ValueTask CreateTable<TModel, TFilter>(string table, DotNetObjectReference<DataTable<TModel, TFilter>> elementRef, string getData, List<DataTableColumnModel> columns, string detailPageUrl = null, bool editButton = false) where TFilter: BaseFilter
        {
            await _jsRuntime.InvokeVoidAsync("datatableBlazor.createTable", table, elementRef, getData, JsonConvert.SerializeObject(columns), detailPageUrl, editButton);
        }

        public async ValueTask RemoveTable(string table)
        {
            await _jsRuntime.InvokeVoidAsync("datatableBlazor.removeTable", table);
        }

        public async ValueTask ReloadTable(string table)
        {
            await _jsRuntime.InvokeVoidAsync("datatableBlazor.reloadTable", table);
        }
    }
}

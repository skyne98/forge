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
    public class UppyService
    {
        private readonly IJSRuntime _jsRuntime;

        public UppyService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async ValueTask Create(DotNetObjectReference<FileSelector> elementRef, string target, bool inline, int width, int height, string fileAdded, string fileRemoved)
        {
            await _jsRuntime.InvokeVoidAsync("uppyBlazor.create", elementRef, target, inline, width, height, fileAdded, fileRemoved);
        }

        public async ValueTask Remove(string target)
        {
            await _jsRuntime.InvokeVoidAsync("uppyBlazor.remove", target);
        }

        public async ValueTask<bool> IsModalOpen(string target)
        {
            return await _jsRuntime.InvokeAsync<bool>("uppyBlazor.isModalOpen", target);
        }

        public async ValueTask OpenModal(string target)
        {
            await _jsRuntime.InvokeAsync<bool>("uppyBlazor.open", target);
        }

        public async ValueTask CloseModal(string target)
        {
            await _jsRuntime.InvokeAsync<bool>("uppyBlazor.close", target);
        }
    }
}

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
    public class SimpleMDEService
    {
        private readonly IJSRuntime _jsRuntime;

        public SimpleMDEService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async ValueTask Create(DotNetObjectReference<MarkdownEditor> elementRef, string target, bool readOnly, string renderMarkdown, string onChange)
        {
            await _jsRuntime.InvokeVoidAsync("simplemdeBlazor.create", elementRef, target, readOnly, renderMarkdown, onChange);
        }

        public async ValueTask<string> GetValue(string target)
        {
            return await _jsRuntime.InvokeAsync<string>("simplemdeBlazor.value", target);
        }

        public async ValueTask SetValue(string target, string value)
        {
            await _jsRuntime.InvokeVoidAsync("simplemdeBlazor.value", target, value);
        }

        public async ValueTask RemoveAsync(string target, bool removeFromDOM = true)
        {
            _jsRuntime.InvokeVoidAsync("simplemdeBlazor.remove", target, removeFromDOM);
        }

        public void Remove(string target, bool removeFromDOM = true)
        {
            ((IJSInProcessRuntime)_jsRuntime).InvokeVoid("simplemdeBlazor.remove", target, removeFromDOM);
        }
    }
}

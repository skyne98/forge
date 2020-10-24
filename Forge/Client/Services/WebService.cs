using Blazored.Toast.Services;
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
    public class WebService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IToastService _toastService;
        private readonly DotNetObjectReference<WebService> _thisRef;

        public WebService(IJSRuntime jsRuntime, IToastService toastService)
        {
            _jsRuntime = jsRuntime;
            _toastService = toastService;

            _thisRef = DotNetObjectReference.Create(this);
        }

        public async ValueTask CopyTextToClipboard(string text)
        {
            await _jsRuntime.InvokeVoidAsync("webBlazor.copyTextToClipboard", text, _thisRef, "web_CopyToClipboardCallback", "web_CopyToClipboardCallbackError");
        }

        [JSInvokable("web_CopyToClipboardCallback")]
        public void Web_CopyToClipboardCallback()
        {
            _toastService.ShowSuccess("Copied to clipboard");
        }

        [JSInvokable("web_CopyToClipboardCallbackError")]
        public void Web_CopyToClipboardCallbackError(string error)
        {
            _toastService.ShowSuccess($"Error: {error}");
        }
    }
}

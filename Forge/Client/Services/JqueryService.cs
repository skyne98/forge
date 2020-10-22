using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forge.Client.Services
{
    public class JqueryService
    {
        private readonly IJSRuntime _jsRuntime;

        public JqueryService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async ValueTask Hide(string selector, int duration)
        {
            await _jsRuntime.InvokeVoidAsync("jqueryBlazor.hide", selector, duration);
        }

        public async ValueTask Show(string selector, int duration)
        {
            await _jsRuntime.InvokeVoidAsync("jqueryBlazor.show", selector, duration);
        }

        public async ValueTask Toggle(string selector, int duration)
        {
            await _jsRuntime.InvokeVoidAsync("jqueryBlazor.toggle", selector, duration);
        }
    }
}

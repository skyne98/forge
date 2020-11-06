using Blazored.Toast.Services;
using Forge.Client.Components;
using Forge.Client.Models;
using Forge.Shared.Filters;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Forge.Client.Services
{
    public class PixiService
    {
        private readonly IJSInProcessRuntime _jsRuntime;

        public PixiService(IJSRuntime jsRuntime)
        {
            _jsRuntime = (IJSInProcessRuntime)jsRuntime;
        }

        public async ValueTask Create(string target, DotNetObjectReference<GraphicsCanvas> callerRef, string onUpdate)
        {
            await _jsRuntime.InvokeVoidAsync("pixiBlazor.create", callerRef, target, onUpdate);
        }

        public async ValueTask Remove(string target)
        {
            await _jsRuntime.InvokeVoidAsync("pixiBlazor.remove", target);
        }

        public async ValueTask Start(string target)
        {
            await _jsRuntime.InvokeVoidAsync("pixiBlazor.start", target);
        }

        public async ValueTask Stop(string target)
        {
            await _jsRuntime.InvokeVoidAsync("pixiBlazor.stop", target);
        }

        public List<double> GetScreenSize(string target)
        {
            var resultJson = _jsRuntime.Invoke<string>("pixiBlazor.screenGetWidth", target);
            return JsonSerializer.Deserialize<List<double>>(resultJson);
        }

        public int Sprite(string target, string uri)
        {
            return _jsRuntime.Invoke<int>("pixiBlazor.sprite", target, uri);
        }

        public int Container(string target)
        {
            return _jsRuntime.Invoke<int>("pixiBlazor.container", target);
        }

        public void AddDisplayObjectToStage(string target, int id)
        {
            _jsRuntime.InvokeVoid("pixiBlazor.addDisplayObjectToStage", target, id);
        }

        public void RemoveDisplayObjectFromStage(string target, int id)
        {
            _jsRuntime.InvokeVoid("pixiBlazor.removeDisplayObjectFromStage", target, id);
        }

        public void AddDisplayObjectToContainer(string target, int id, int containerId)
        {
            _jsRuntime.InvokeVoid("pixiBlazor.addDisplayObjectToContainer", target, id, containerId);
        }

        public void RemoveDisplayObjectFromContainer(string target, int id, int containerId)
        {
            _jsRuntime.InvokeVoid("pixiBlazor.removeDisplayObjectFromContainer", target, id, containerId);
        }

        public List<double> ToLocal(string target, int id, double x, double y)
        {
            var resultJson = _jsRuntime.Invoke<string>("pixiBlazor.toLocal", target, id, x, y);
            return JsonSerializer.Deserialize<List<double>>(resultJson);
        }

        public List<double> ToGlobal(string target, int id, double x, double y)
        {
            var resultJson = _jsRuntime.Invoke<string>("pixiBlazor.toGlobal", target, id, x, y);
            return JsonSerializer.Deserialize<List<double>>(resultJson);
        }

        public T GetDisplayObjectMember<T>(string target, int id, List<string> memberPath)
        {
            return _jsRuntime.Invoke<T>("pixiBlazor.getDisplayObjectMember", target, id, memberPath);
        }

        public void SetDisplayObjectMember<T>(string target, int id, List<string> memberPath, T value)
        {
            _jsRuntime.InvokeVoid("pixiBlazor.setDisplayObjectMember", target, id, memberPath, value);
        }

        public T GetContainerMember<T>(string target, int id, List<string> memberPath)
        {
            return _jsRuntime.Invoke<T>("pixiBlazor.getContainerMember", target, id, memberPath);
        }

        public void SetContainerMember<T>(string target, int id, List<string> memberPath, T value)
        {
            _jsRuntime.InvokeVoid("pixiBlazor.setContainerMember", target, id, memberPath, value);
        }

        public T GetSpriteMember<T>(string target, int id, List<string> memberPath)
        {
            return _jsRuntime.Invoke<T>("pixiBlazor.getSpriteMember", target, id, memberPath);
        }

        public void SetSpriteMember<T>(string target, int id, List<string> memberPath, T value)
        {
            _jsRuntime.InvokeVoid("pixiBlazor.setSpriteMember", target, id, memberPath, value);
        }
    }
}

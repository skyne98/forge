using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.Toast;
using Forge.Client.Services;

namespace Forge.Client
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddTransient<JqueryService>();
            builder.Services.AddTransient<DatatableService>();
            builder.Services.AddTransient<UppyService>();
            builder.Services.AddTransient<SimpleMDEService>();
            builder.Services.AddTransient<PixiService>();
            builder.Services.AddTransient<WebService>();
            builder.Services.AddBlazoredToast();

            await builder.Build().RunAsync();
        }
    }
}

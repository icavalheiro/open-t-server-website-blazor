using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace AspNetCore.SSG
{
    public static class Generator
    {
        public static async Task Generate(IHostBuilder builder, string distPath, int port)
        {
            Console.WriteLine("Server Side Generation starting...");

            builder.ConfigureWebHost(builder =>
            {
                builder.UseUrls($"http://0.0.0.0:{port}");

            });

            var host = builder.Build();
            var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(async () =>
            {
                Console.WriteLine("Deleting dist folder...");
                if (Directory.Exists(distPath))
                {
                    Directory.Delete(distPath, true);
                }

                /*
                src="$"
                src='$'
                href='$'
                href="$"
                url($)

                tudo que n tiver *.* tem que ser posto em _pasta_/index.html

                */

                var endpointDataSources = host.Services.GetRequiredService<IEnumerable<EndpointDataSource>>();
                var endpoints = endpointDataSources
                    .SelectMany(es => es.Endpoints)
                    .OfType<RouteEndpoint>();
                var output = endpoints.Select(
                    e =>
                    {
                        var controller = e.Metadata
                            .OfType<ControllerActionDescriptor>()
                            .FirstOrDefault();
                        var action = controller != null
                            ? $"{controller.ControllerName}.{controller.ActionName}"
                            : null;
                        var controllerMethod = controller != null
                            ? $"{controller.ControllerTypeInfo.FullName}:{controller.MethodInfo.Name}"
                            : null;
                        return $"/{e.RoutePattern.RawText.TrimStart('/')}";
                    }
                );

                Console.WriteLine("output:");
                foreach (var o in output)
                    Console.WriteLine(o);

                // Console.WriteLine($"Starting, saving files to {distPath}");
                // var crawler = new Crawler($"http://localhost:{port}", distPath);
                // await crawler.Run();

                // // var process = Process.Start("wget", $"-m http://localhost:{port} -P {distPath} -nv -E -nH -k");

                // // await process.WaitForExitAsync();

                Console.WriteLine("Generation completed, stoppping server...");
                await host.StopAsync();
            });

            Console.WriteLine($"Starting server for generation at port {port}");
            await host.RunAsync();
        }
    }
}

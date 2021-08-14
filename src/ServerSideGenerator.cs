using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using System.Net;


namespace TibiaWebsite
{

    public static class ServerSideGenerator
    {
        public static IEnumerable<string> GetMvcAndRazorRoutes(IActionDescriptorCollectionProvider actionDescriptorsCollections)
        {
            var routes = actionDescriptorsCollections.ActionDescriptors.Items;
            var paths = new List<string>();
            foreach (ActionDescriptor actionDescriptor in routes)
            {
                string path = "";
                // Path and Invocation of Razor Pages
                if (actionDescriptor is PageActionDescriptor)
                {
                    var e = actionDescriptor as PageActionDescriptor;
                    path = e.ViewEnginePath;
                }

                // Path of Route Attribute
                if (actionDescriptor.AttributeRouteInfo != null)
                {
                    var e = actionDescriptor;
                    path = $"/{e.AttributeRouteInfo.Template}";
                }

                // Path and Invocation of Controller/Action
                if (actionDescriptor is ControllerActionDescriptor)
                {
                    var e = actionDescriptor as ControllerActionDescriptor;
                    if (path == "")
                    {
                        path = $"/{e.ControllerName}/{e.ActionName}";
                    }
                }

                path = path.ToLower();

                if (path.EndsWith("/index"))
                {
                    path = path.Replace("/index", "");
                }

                paths.Add(path);
            }

            return paths;
        }

        public static IEnumerable<string> GetBlazorRoutes()
        {
            var paths = new List<string>();

            var allComponents = Assembly
                .GetExecutingAssembly()
                .ExportedTypes
                .Where(t => t.IsSubclassOf(typeof(ComponentBase)));

            var routableComponents = allComponents
                .Where(c => c
                            .GetCustomAttributes(inherit: true)
                            .OfType<RouteAttribute>()
                            .Count() > 0);

            foreach (var component in routableComponents)
            {
                var routeAttribute = component.GetCustomAttributes(inherit: true).OfType<RouteAttribute>().FirstOrDefault();
                if (routeAttribute is not null)
                {
                    var path = routeAttribute.Template;
                    paths.Add(path.ToLower());
                }
            }

            return paths;
        }

        public static IEnumerable<string> GetAllRoutes(IServiceProvider services, string[] extraRoutes)
        {
            var paths = new List<string>();
            var actionDescriptorsCollections = services.GetRequiredService<IActionDescriptorCollectionProvider>();
            paths.AddRange(GetMvcAndRazorRoutes(actionDescriptorsCollections));
            paths.AddRange(GetBlazorRoutes());

            //adding 404 page (because ofc it's not in the list lol)
            paths.Add("/404");

            //remove all dynamic routes, we won't process them
            var dynamicRoutes = paths.Where(x => x.Contains("{")).ToArray();
            foreach (var dynamic in dynamicRoutes)
            {
                paths.Remove(dynamic);
            }

            paths.AddRange(extraRoutes);

            paths = paths.Distinct().ToList();

            return paths;
        }

        public static void CopyStaticToDist(string distPath)
        {
            var files = Directory.GetFiles("./wwwroot", "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var source = file;
                var dest = Path.Combine(distPath, file.Replace("/wwwroot", ""));
                var destFolder = Path.GetDirectoryName(dest);
                Directory.CreateDirectory(destFolder);
                File.Copy(file, dest);
            }
        }


        public static async Task Generate(IHostBuilder builder, string distPath, int port, string[] extraRoutes)
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

                Console.WriteLine("Copying static files");
                CopyStaticToDist(distPath);

                Console.WriteLine("Generating .html files");

                // #####
                var distFolder = Path.GetFullPath(distPath);
                var routes = GetAllRoutes(host.Services, extraRoutes);
                using (var client = new WebClient())
                {
                    foreach (var route in routes)
                    {
                        var uri = new Uri($"http://localhost:{port}" + route);
                        var html = client.DownloadString(uri);
                        var destRoute = distFolder + route;
                        Directory.CreateDirectory(destRoute);
                        var dest = (uri.IsFile) ? destRoute : Path.Combine(destRoute, "index.html");
                        html = html.Replace(@"<script src=""_framework/blazor.server.js""></script>", "");
                        await File.WriteAllTextAsync(dest, html);
                    }
                }

                // ####

                Console.WriteLine("Generation completed, stoppping server...");
                await host.StopAsync();
            });

            Console.WriteLine($"Starting server for generation at port {port}");
            await host.RunAsync();
        }

        public static void RunOrGenerate(this IHostBuilder builder, string[] args, string[] extraRoutes = null, string distPath = "./dist", int port = 8744)
        {
            Task.WaitAll(Generate(builder, distPath, port, extraRoutes));
            // if (args.Length > 0 && args[0] == "generate")
            // {
            // }
            // else
            // {
            //     builder.Build().Run();
            // }
        }
    }
}

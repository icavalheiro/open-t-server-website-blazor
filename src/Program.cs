using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.IO;


namespace TibiaWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            if (args.Length > 0 && args[0] == "generate")
            {
                Task.WaitAll(GenerateStatic(hostBuilder));
            }
            else
                hostBuilder.Build().Run();
        }

        private static async Task GenerateStatic(IHostBuilder hostBuilder, string distPath = "./dist", int port = 8744)
        {
            Console.WriteLine("Server Side Generation with \"wget\". Please make sure \"wget\" is installed and in path");
            hostBuilder.ConfigureWebHost(builder =>
            {
                builder.UseUrls($"http://0.0.0.0:{port}");

            });

            var host = hostBuilder.Build();
            var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(async () =>
            {
                Console.WriteLine("Deleting dist folder...");
                if (Directory.Exists(distPath))
                {
                    Directory.Delete(distPath, true);
                }

                Console.WriteLine($"Starting wget, saving files to {distPath}");
                var process = Process.Start("wget", $"-m http://localhost:{port} -P {distPath} -nv -E -nH");

                await process.WaitForExitAsync();

                Console.WriteLine("Generation completed, stoppping server...");
                await host.StopAsync();
            });

            Console.WriteLine($"Starting server for generation at port {port}");
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

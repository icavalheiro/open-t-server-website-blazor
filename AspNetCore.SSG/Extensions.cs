using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace AspNetCore.SSG
{
    public static class Extensions
    {
        public static void RunOrGenerate(this IHostBuilder builder, string[] args, string distPath = "./dist", int port = 8744)
        {
            if (args.Length > 0 && args[0] == "generate")
            {
                Task.WaitAll(Generator.Generate(builder, distPath, port));
            }
            else
            {
                builder.Build().Run();
            }
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TibiaWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);
            hostBuilder.RunOrGenerate(args, new string[] { "/news/123", "/news/tty", "/TibiaWebsite.styles.css" });
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

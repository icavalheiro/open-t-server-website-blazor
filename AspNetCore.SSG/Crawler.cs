using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace AspNetCore.SSG
{
    public class Crawler
    {
        private readonly string Url;
        private readonly string DistPath;
        public Crawler(string url, string dist)
        {
            Url = url;
            DistPath = dist;
        }

        public async Task Run()
        {

        }
    }
}

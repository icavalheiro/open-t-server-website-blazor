using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;

namespace AspNetCore.SSG
{
    public static class URLFinder
    {
        public static string[] Search(string html)
        {
            List<string> list = new List<string>();
            var lowerHtml = html.ToLower();
            var nextIndex = lowerHtml.IndexOf("href=");
            while (nextIndex >= 0)
            {
                var quoteChar = lowerHtml[nextIndex + 5];
                var end = lowerHtml.IndexOf(quoteChar, nextIndex + 6);
                var url = lowerHtml.Substring(nextIndex + 5, end);
                list.Add(url);
                nextIndex = lowerHtml.IndexOf("href=", end);
            }

            return list.ToArray();
        }
    }
}

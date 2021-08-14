using System;
using Xunit;
using System.IO;

namespace AspNetCore.SSG.Tests
{
    public class URLFinder
    {
        private readonly string href_html;

        public URLFinder()
        {
            href_html = File.ReadAllText("./href.html");
        }

        [Fact]
        public void SearchHref()
        {
            var exptected = new string[] {
                "http://test.com",
                "https://google.com/test/usual?param.gtes",
                "fake",
                "broken/",
                "/massive/oak",
                "finder.html",
                "~/dolls.apsx",
                "/server/test/gun.css"
            };
            var observed = AspNetCore.SSG.URLFinder.Search(href_html);

            Assert.Equal(exptected, observed);
        }
    }
}
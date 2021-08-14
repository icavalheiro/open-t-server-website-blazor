using System;
using Xunit;

namespace AspNetCore.SSG.Tests
{
    public class Extensions
    {
        [Fact]
        public void RunOrGenerateExists()
        {
            var expected = "RunOrGenerate";
            var observed = nameof(AspNetCore.SSG.Extensions.RunOrGenerate);

            Assert.Equal(expected, observed);
        }
    }
}
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Jellyfin.Plugin.OpenDouban.Tests
{
    public class OddbApiClientTest
    {
        private readonly OddbApiClient _oddbApiClient;

        public OddbApiClientTest(ITestOutputHelper output)
        {
            var serviceProvider = OddbServiceUtils.BuildServiceProvider<OddbApiClient>(output);
            _oddbApiClient = serviceProvider.GetService<OddbApiClient>();
        }

        [Fact]
        public void TestFullSearch() 
        {
            List<ApiSubject> list = _oddbApiClient.FullSearch("Harry Potter").Result;
            Assert.NotEmpty(list);
        }

        [Fact]
        public void TestPartialSearch() 
        {
            List<ApiSubject> list = _oddbApiClient.PartialSearch("Harry Potter").Result;
            Assert.NotEmpty(list);
        }

        [Fact]
        public void TestGetBySid() {
            ApiSubject result = _oddbApiClient.GetBySid("1295038").Result;
            Assert.Equal("1295038", result.Sid);
            Assert.NotEmpty(result.Celebrities);
        }

        [Fact]
        public void TestGetPhotoBySid()
        {
            List<ApiPhoto> list = _oddbApiClient.GetPhotoBySid("30435124").Result;
            Assert.NotEmpty(list);
        }
    }
}

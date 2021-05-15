using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Jellyfin.Plugin.OpenDouban.Service;

namespace Jellyfin.Plugin.OpenDouban.Tests
{
    public class ApiClientTest
    {
        private readonly ApiClient apiClient;

        public ApiClientTest(ITestOutputHelper output)
        {
            var serviceProvider = ServiceUtils.BuildServiceProvider<ApiClient>(output);
            this.apiClient = serviceProvider.GetService<ApiClient>();
        }

        [Fact]
        public void TestFullSearch() 
        {
            List<ApiSubject> list = apiClient.FullSearch("Harry Potter").Result;
            Assert.NotEmpty(list);
        }

        [Fact]
        public void TestPartialSearch() 
        {
            List<ApiSubject> list = apiClient.PartialSearch("Harry Potter").Result;
            Assert.NotEmpty(list);
        }

        [Fact]
        public void TestGetBySid() {
            ApiSubject result = apiClient.GetBySid("1295038").Result;
            Assert.Equal("1295038", result.Sid);
            Assert.NotEmpty(result.Celebrities);
        }
    }
}

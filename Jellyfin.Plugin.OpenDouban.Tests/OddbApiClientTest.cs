using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
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
        public async Task TestFullSearch()
        {
            List<ApiSubject> list = await _oddbApiClient.FullSearch("Harry Potter");
            list.ShouldNotBeNull();
        }

        [Fact]
        public async Task TestPartialSearch()
        {
            List<ApiSubject> list = await _oddbApiClient.PartialSearch("Harry Potter");
            list.ShouldNotBeNull();
        }

        [Fact]
        public async Task TestGetBySid()
        {
            ApiSubject result = await _oddbApiClient.GetBySid("1295038");
            result.Celebrities.ShouldNotBeNull();
            result.Sid.ShouldBe("1295038");
        }

        [Fact]
        public async Task TestGetPhotoBySid()
        {
            List<ApiPhoto> list = await _oddbApiClient.GetPhotoBySid("30435124");
            list.ShouldNotBeNull();
        }
    }
}

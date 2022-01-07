using System.Collections.Generic;
using System.Threading;
using MediaBrowser.Controller.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Jellyfin.Plugin.OpenDouban.Providers;
using System.Threading.Tasks;
using Shouldly;

namespace Jellyfin.Plugin.OpenDouban.Tests
{
    public class OddbSeasonProviderTest
    {
        private readonly OddbSeasonProvider _doubanProvider;
        public OddbSeasonProviderTest(ITestOutputHelper output)
        {
            var serviceProvider = OddbServiceUtils.BuildServiceProvider<OddbSeasonProvider>(output);
            _doubanProvider = serviceProvider.GetService<OddbSeasonProvider>();
            _doubanProvider.Pattern = OddbServiceUtils.Pattern;
        }

        [Fact]
        public async Task TestGetSeasonMetadata()
        {
            SeasonInfo seasonInfo = new()
            {
                Name = "老友记 第二季",
                SeriesProviderIds = new Dictionary<string, string> { { OddbPlugin.ProviderId, "1393859" } }
            };

            var metadataResult = await _doubanProvider.GetMetadata(seasonInfo, CancellationToken.None);
            metadataResult.HasMetadata.ShouldBeTrue();
        }
    }
}
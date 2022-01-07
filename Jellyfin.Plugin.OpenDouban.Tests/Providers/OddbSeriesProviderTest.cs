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
    public class OddbSeriesProviderTest
    {
        private readonly OddbSeriesProvider _doubanProvider;
        public OddbSeriesProviderTest(ITestOutputHelper output)
        {
            var serviceProvider = OddbServiceUtils.BuildServiceProvider<OddbSeriesProvider>(output);
            _doubanProvider = serviceProvider.GetService<OddbSeriesProvider>();
            _doubanProvider.Pattern = OddbServiceUtils.Pattern;
        }

        [Fact]
        public async Task TestSearchSeries()
        {
            SeriesInfo info = new()
            {
                Name = "老友记",
            };
            var result = await _doubanProvider.GetSearchResults(info, CancellationToken.None);
            result.ShouldNotBeEmpty();
        }
    }
}

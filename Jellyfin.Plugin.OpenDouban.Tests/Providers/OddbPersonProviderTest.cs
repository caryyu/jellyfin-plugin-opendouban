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
    public class OddbPersonProviderTest
    {
        private readonly OddbPersonProvider _provider;
        public OddbPersonProviderTest(ITestOutputHelper output)
        {
            var serviceProvider = OddbServiceUtils.BuildServiceProvider<OddbPersonProvider>(output);
            _provider = serviceProvider.GetService<OddbPersonProvider>();
        }

        [Fact]
        public async Task TestGetMetadata()
        {
            var info = new PersonLookupInfo
            {
                ProviderIds = new Dictionary<string, string> { { OddbPlugin.ProviderId, "1032025" } }
            };

            var meta = await _provider.GetMetadata(info, CancellationToken.None);
            meta.HasMetadata.ShouldBeTrue();
            meta.Item.Name.ShouldBe("肖恩·比格斯代夫");
        }
    }
}

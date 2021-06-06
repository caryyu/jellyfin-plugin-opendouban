using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Jellyfin.Plugin.OpenDouban.Tests
{
    public class PersonProviderTest
    {
        private readonly PersonProvider _provider;
        public PersonProviderTest(ITestOutputHelper output)
        {
            var serviceProvider = ServiceUtils.BuildServiceProvider<PersonProvider>(output);
            _provider = serviceProvider.GetService<PersonProvider>();
        }

        [Fact]
        public void TestGetMetadata()
        {
            PersonLookupInfo info = new PersonLookupInfo
            {
                ProviderIds = new Dictionary<string, string>{{OpenDoubanPlugin.ProviderID, "1032025"}}
            };

            var meta = _provider.GetMetadata(info, CancellationToken.None).Result;
            Assert.True(meta.HasMetadata);
            Assert.Equal("肖恩·比格斯代夫", meta.Item.Name);
        }
    }
}

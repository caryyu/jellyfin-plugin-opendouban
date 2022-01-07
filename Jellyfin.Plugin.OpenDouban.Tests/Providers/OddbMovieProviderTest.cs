using System;
using System.Linq;
using System.Threading;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using System.Text.RegularExpressions;
using Jellyfin.Plugin.OpenDouban.Configuration;
using Jellyfin.Plugin.OpenDouban.Providers;
using System.Threading.Tasks;
using Shouldly;

namespace Jellyfin.Plugin.OpenDouban.Tests
{
    public class OddbMovieProviderTest
    {
        private readonly OddbMovieProvider _provider;
        public OddbMovieProviderTest(ITestOutputHelper output)
        {
            var serviceProvider = OddbServiceUtils.BuildServiceProvider<OddbMovieProvider>(output);
            _provider = serviceProvider.GetService<OddbMovieProvider>();
            _provider.Pattern = OddbServiceUtils.Pattern;
        }

        [Fact]
        public async Task TestGetSearchResults()
        {
            // Test 1: search metadata.
            MovieInfo info = new ()
            {
                Name = "蝙蝠侠.黑暗骑士",
            };

            var result = await _provider.GetSearchResults(info, CancellationToken.None);
            result.ShouldNotBeNull();
            result.Count().ShouldBeGreaterThan(1);
            string doubanId = result.FirstOrDefault()?.GetProviderId(OddbPlugin.ProviderId);
            int? year = result.FirstOrDefault()?.ProductionYear;
            doubanId.ShouldBe("1851857");
            year.ShouldBe(2008);

            // Test 2: Already has provider Id.
            info.SetProviderId(OddbPlugin.ProviderId, "1851857");
            result = await _provider.GetSearchResults(info, CancellationToken.None);
            result.Count().ShouldBe(1);
            doubanId = result.FirstOrDefault()?.GetProviderId(OddbPlugin.ProviderId);
            year = result.FirstOrDefault()?.ProductionYear;
            doubanId.ShouldBe("1851857");
            year.ShouldBe(2008);
        }

        [Fact]
        public async Task TestGetMetadata()
        {
            // Test 1: Normal case.
            var info = new MovieInfo()
            {
                Name = "源代码"
            };
            var meta = await _provider.GetMetadata(info, CancellationToken.None);
            meta.HasMetadata.ShouldBeTrue();
            meta.Item.Name.ShouldBe("源代码");
            meta.Item.GetProviderId(OddbPlugin.ProviderId).ShouldBe("3075287");
            meta.Item.PremiereDate.ShouldBe(DateTime.Parse("2011-08-30"));

            // Test 2: Already has provider Id.
            info = new MovieInfo()
            {
                Name = "Source Code"
            };
            info.SetProviderId(OddbPlugin.ProviderId, "1851857");
            meta = await _provider.GetMetadata(info, CancellationToken.None);
            meta.HasMetadata.ShouldBeTrue();
            meta.Item.Name.ShouldBe("蝙蝠侠：黑暗骑士");
        }

        [Fact]
        public void TestNameRegexFiltering()
        {
            PluginConfiguration cfg = new();

            string[] names = {
              "Loki.S01E03.HDR.2160p.WEB.H265-EXPLOIT.mkv",
              "Minuscule.The.Valley.Of.The.Lost.Ants.2013.1080p.BluRay.H264.AAC-RARBG.mp4",
              "Harry.Potter.and.the.Goblet.of.Fire.2005.1080p.BrRip.x264.YIFY.mp4",
              "Harry.Potter.and.the.Sorcerers.Stone.2001.1080p.BrRip.x264.YIFY ( FIRST TRY).mp4",
              "Harry.Potter.and.the.Sorcerer's.Stone.Extended.Cut.2001.720p.BrRip.x264.AAC.5.1.{MrMoviesFX}.【ThumperDC】.mkv",
              "Ice Age Dawn of the Dinosaurs.2009.720p.BrRip.x264.YIFY.mp4",
              "Titanic.1997.HDTV.1080p.x264.YIFY.mp4",
              "The.Lion.King.2019.1080p.BluRay.x264-[YTS.LT].mp4",
              "The.Croods.A.New.Age.2020.720p.WEBRip.800MB.x264-GalaxyRG.mkv"
            };

            // Loki
            // Minuscule The Valley Of The Lost Ants 2013
            // Harry Potter and the Goblet of Fire 2005
            // Harry Potter and the Sorcerers Stone 2001
            // Harry Potter and the Sorcerer's Stone Extended Cut 2001
            // Ice Age Dawn of the Dinosaurs 2009
            // Titanic 1997
            // The Lion King 2019
            // The Croods A New Age 2020
            Regex.Replace(names[0], cfg.Pattern, " ").Trim().ShouldBe("Loki");
            Regex.Replace(names[7], cfg.Pattern, " ").Trim().ShouldBe("The Lion King 2019");
            Regex.Replace(names[8], cfg.Pattern, " ").Trim().ShouldBe("The Croods A New Age 2020");
        }
    }
}

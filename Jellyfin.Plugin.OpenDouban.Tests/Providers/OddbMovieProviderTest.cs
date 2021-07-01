// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using MediaBrowser.Controller.Providers;
// using MediaBrowser.Model.Entities;
// using MediaBrowser.Model.Providers;
// using Microsoft.Extensions.DependencyInjection;
// using Xunit;
// using Xunit.Abstractions;
// using System.Text.RegularExpressions;
// using Jellyfin.Plugin.OpenDouban.Configuration;

// namespace Jellyfin.Plugin.OpenDouban.Tests
// {
//     public class MovieProviderTest
//     {
//         private readonly MovieProvider _provider;
//         public MovieProviderTest(ITestOutputHelper output)
//         {
//             var serviceProvider = ServiceUtils.BuildServiceProvider<MovieProvider>(output);
//             _provider = serviceProvider.GetService<MovieProvider>();
//         }

//         [Fact]
//         public void TestGetSearchResults()
//         {
//             // Test 1: search metadata.
//             MovieInfo info = new MovieInfo()
//             {
//                 Name = "蝙蝠侠.黑暗骑士",
//             };

//             var result = _provider.GetSearchResults(info, CancellationToken.None).Result;
//             Assert.NotEmpty(result);
//             Assert.True(result.Count() > 1);
//             string doubanId = result.FirstOrDefault()?.GetProviderId(OpenDoubanPlugin.ProviderID);
//             int? year = result.FirstOrDefault()?.ProductionYear;
//             Assert.Equal("1851857", doubanId);
//             Assert.Equal(2008, year);

//             // Test 2: Already has provider Id.
//             info.SetProviderId(OpenDoubanPlugin.ProviderID, "1851857");
//             result = _provider.GetSearchResults(info, CancellationToken.None).Result;
//             Assert.True(result.Count() == 1);
//             doubanId = result.FirstOrDefault()?.GetProviderId(OpenDoubanPlugin.ProviderID);
//             year = result.FirstOrDefault()?.ProductionYear;
//             Assert.Equal("1851857", doubanId);
//             Assert.Equal(2008, year);
//         }

//         [Fact]
//         public void TestGetMetadata()
//         {
//             // Test 1: Normal case.
//             MovieInfo info = new MovieInfo()
//             {
//                 Name = "源代码"
//             };
//             var meta = _provider.GetMetadata(info, CancellationToken.None).Result;
//             Assert.True(meta.HasMetadata);
//             Assert.Equal("源代码", meta.Item.Name);
//             Assert.Equal("3075287", meta.Item.GetProviderId(OpenDoubanPlugin.ProviderID));
//             // Assert.Equal(DateTime.Parse("2011-08-30"), meta.Item.PremiereDate);

//             // Test 2: Already has provider Id.
//             info = new MovieInfo()
//             {
//                 Name = "Source Code"
//             };
//             info.SetProviderId(OpenDoubanPlugin.ProviderID, "1851857");
//             meta = _provider.GetMetadata(info, CancellationToken.None).Result;
//             Assert.True(meta.HasMetadata);
//             Assert.Equal("蝙蝠侠：黑暗骑士", meta.Item.Name);

//             // Test 2: Not movie type.
//             info = new MovieInfo()
//             {
//                 Name = "大秦帝国"
//             };
//             meta = _provider.GetMetadata(info, CancellationToken.None).Result;
//             Assert.False(meta.HasMetadata);
//         }

//         [Fact]
//         public void TestNameRegexFiltering() {
//             PluginConfiguration cfg = new PluginConfiguration();

//             string[] names = {
//               "Loki.S01E03.HDR.2160p.WEB.H265-EXPLOIT.mkv",
//               "Minuscule.The.Valley.Of.The.Lost.Ants.2013.1080p.BluRay.H264.AAC-RARBG.mp4",
//               "Harry.Potter.and.the.Goblet.of.Fire.2005.1080p.BrRip.x264.YIFY.mp4",
//               "Harry.Potter.and.the.Sorcerers.Stone.2001.1080p.BrRip.x264.YIFY ( FIRST TRY).mp4",
//               "Harry.Potter.and.the.Sorcerer's.Stone.Extended.Cut.2001.720p.BrRip.x264.AAC.5.1.{MrMoviesFX}.【ThumperDC】.mkv",
//               "Ice Age Dawn of the Dinosaurs.2009.720p.BrRip.x264.YIFY.mp4",
//               "Titanic.1997.HDTV.1080p.x264.YIFY.mp4",
//               "The.Lion.King.2019.1080p.BluRay.x264-[YTS.LT].mp4",
//               "The.Croods.A.New.Age.2020.720p.WEBRip.800MB.x264-GalaxyRG.mkv"
//             };

//             // Loki
//             // Minuscule The Valley Of The Lost Ants 2013
//             // Harry Potter and the Goblet of Fire 2005
//             // Harry Potter and the Sorcerers Stone 2001
//             // Harry Potter and the Sorcerer's Stone Extended Cut 2001
//             // Ice Age Dawn of the Dinosaurs 2009
//             // Titanic 1997
//             // The Lion King 2019
//             // The Croods A New Age 2020

//             Assert.Equal("Loki", Regex.Replace(names[0], cfg.Pattern, " ").Trim());
//             Assert.Equal("The Lion King 2019", Regex.Replace(names[7], cfg.Pattern, " ").Trim());
//             Assert.Equal("The Croods A New Age 2020", Regex.Replace(names[8], cfg.Pattern, " ").Trim());
//         }
//     }
// }

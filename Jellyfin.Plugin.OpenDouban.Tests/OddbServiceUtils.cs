using System;
using System.IO;
using System.Threading.Tasks;
using MediaBrowser.Model.Serialization;
using ServiceStack.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Jellyfin.Plugin.OpenDouban.Tests
{
    class OddbServiceUtils
    {
        public static ServiceProvider BuildServiceProvider<T>(ITestOutputHelper output) where T : class
        {
            var services = new ServiceCollection()
                .AddHttpClient()
                .AddLogging(builder => builder.AddXUnit(output).SetMinimumLevel(LogLevel.Debug))
                .AddSingleton<OddbApiClient>()
                .AddSingleton<T>();

            var serviceProvider = services.BuildServiceProvider();
            var oddbApiClient = serviceProvider.GetService<OddbApiClient>();
            oddbApiClient.ApiBaseUri = "http://localhost:8080";

            return serviceProvider;
        }

        public static string Pattern => @"(S\d{2}|E\d{2}|HDR|\d{3,4}p|WEBRip|WEB|YIFY|BrRip|BluRay|H265|H264|x264|AAC\.\d\.\d|AAC|HDTV|mkv|mp4)|(\[.*\])|(\-\w+|\{.*\}|【.*】|\(.*\)|\d+MB)|(\.|\-)";
    }
}

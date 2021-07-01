using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.OpenDouban.Providers
{
    /// <summary>
    /// OddbImageProvider.
    /// </summary>
    public class OddbImageProvider : IRemoteImageProvider
    {
        private ILogger<OddbImageProvider> _logger;
        private IHttpClientFactory _httpClientFactory;
        private OddbApiClient _oddbApiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="OddbImageProvider"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        /// <param name="logger">Instance of the <see cref="ILogger{OddbImageProvider}"/> interface.</param>
        /// <param name="oddbApiClient">Instance of <see cref="OddbApiClient"/>.</param>
        public OddbImageProvider(IHttpClientFactory httpClientFactory, ILogger<OddbImageProvider> logger, OddbApiClient oddbApiClient)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _oddbApiClient = oddbApiClient;
        }

        /// <inheritdoc />
        public string Name => OddbPlugin.ProviderName;

        /// <inheritdoc />
        public bool Supports(BaseItem item) => item is Movie || item is Series;

        /// <inheritdoc />
        public IEnumerable<ImageType> GetSupportedImages(BaseItem item) => new List<ImageType>
        {
            ImageType.Primary,
            ImageType.Backdrop
        };

        /// <inheritdoc />
        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[DOUBAN] GetImages for item: {item.Name}");

            if (!item.HasProviderId(OddbPlugin.ProviderId))
            {
                _logger.LogWarning($"[DOUBAN] Got images failed because the sid of \"{item.Name}\" is empty!");
                return new List<RemoteImageInfo>();
            }

            string sid = item.GetProviderId(OddbPlugin.ProviderId);
            var primary = await _oddbApiClient.GetBySid(sid);
            var dropback = await GetBackdrop(sid, cancellationToken);

            var res = new List<RemoteImageInfo> {
                new RemoteImageInfo 
                {
                    ProviderName = primary.Name,
                    Url = primary.Img,
                    Type = ImageType.Primary
                }
            };
            res.AddRange(dropback);
            return res;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[DOUBAN] GetImageResponse url: {0}", url);
            HttpResponseMessage response = await _httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }

        /// <summary>
        /// This logic should be covered by the backend
        /// </summary>
        public async Task<IEnumerable<RemoteImageInfo>> GetBackdrop(string sid, CancellationToken cancellationToken)
        {
            var url = $"https://movie.douban.com/subject/{sid}/photos?type=W&start=0&sortby=size&size=a&subtype=a";

            var response = await _httpClientFactory.CreateClient().GetAsync(url, cancellationToken);
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            string content = new StreamReader(stream).ReadToEnd();

            const String pattern = @"(?s)data-id=""(\d+)"".*?class=""prop"">\n\s*(\d+)x(\d+)";
            Match match = Regex.Match(content, pattern);

            var list = new List<RemoteImageInfo>();
            while (match.Success)
            {
                string data_id = match.Groups[1].Value;
                string width = match.Groups[2].Value;
                string height = match.Groups[3].Value;
                _logger.LogInformation("Find backdrop id {0}, size {1}x{2}", data_id, width, height);

                if (float.Parse(width) > float.Parse(height) * 1.3)
                {
                    // Just chose the Backdrop which width is larger than height
                    list.Add(new RemoteImageInfo
                    {
                        ProviderName = Name,
                        Url = string.Format("https://img9.doubanio.com/view/photo/l/public/p{0}.webp", data_id),
                        Type = ImageType.Backdrop,
                    });
                }

                match = match.NextMatch();
            }

            return list;
        }
    }
}
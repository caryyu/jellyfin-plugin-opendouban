using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.OpenDouban.Providers
{
    /// <summary>
    /// OddbPersonProvider.
    /// </summary>
    public class OddbPersonProvider : IRemoteMetadataProvider<Person, PersonLookupInfo>
    {
        private ILogger<OddbPersonProvider> _logger;
        private IHttpClientFactory _httpClientFactory;
        private OddbApiClient _oddbApiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="OddbPersonProvider"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        /// <param name="logger">Instance of the <see cref="ILogger{OddbPersonProvider}"/> interface.</param>
        /// <param name="oddbApiClient">Instance of <see cref="OddbApiClient"/>.</param>
        public OddbPersonProvider(IHttpClientFactory httpClientFactory, ILogger<OddbPersonProvider> logger, OddbApiClient oddbApiClient)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _oddbApiClient = oddbApiClient;
        }

        /// <inheritdoc />
        public string Name => OddbPlugin.ProviderName;

        /// <inheritdoc />
        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(PersonLookupInfo searchInfo, CancellationToken cancellationToken)
        {
            return await Task.FromResult<IEnumerable<RemoteSearchResult>>(new List<RemoteSearchResult>());
        }

        /// <inheritdoc />
        public async Task<MetadataResult<Person>> GetMetadata(PersonLookupInfo info, CancellationToken cancellationToken)
        {
            MetadataResult<Person> result = new MetadataResult<Person>();

            string cid = info.GetProviderId(OddbPlugin.ProviderId);
            _logger.LogInformation($"[Open DOUBAN] Person GetMetadata of [cid]: \"{cid}\"");
            ApiCelebrity c = await _oddbApiClient.GetCelebrityByCid(cid, cancellationToken);

            if (c != null)
            {
                Person p = new Person
                {
                    Name = c.Name,
                    HomePageUrl = c.Site,
                    Overview = c.Intro,
                    PremiereDate = DateTime.ParseExact(c.Birthdate, "yyyy年MM月dd日", System.Globalization.CultureInfo.CurrentCulture)
                };

                p.SetProviderId(OddbPlugin.ProviderId, c.Id);

                if (!string.IsNullOrWhiteSpace(c.Birthplace))
                {
                    p.ProductionLocations = new[] { c.Birthplace };
                }

                if (!string.IsNullOrEmpty(c.Imdb))
                {
                    p.SetProviderId(MetadataProvider.Imdb, c.Imdb);
                }

                result.HasMetadata = true;
                result.Item = p;
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Open DOUBAN] Person GetImageResponse url: {0}", url);
            HttpResponseMessage response = await _httpClientFactory.CreateClient().GetAsync(url, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
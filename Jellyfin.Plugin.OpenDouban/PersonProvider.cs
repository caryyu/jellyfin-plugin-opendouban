using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;
using Jellyfin.Plugin.OpenDouban.Service;

namespace Jellyfin.Plugin.OpenDouban
{
    public class PersonProvider : IRemoteMetadataProvider<Person, PersonLookupInfo>
    {
        public string Name => "Open Douban Person Provider";
        public int Order => 3;
        private ApiClient apiClient;
        private ILogger logger;
        private IHttpClientFactory httpClientFactory;

        public PersonProvider(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer, ILogger<MovieProvider> logger)
        {
            this.apiClient = new ApiClient(httpClientFactory, jsonSerializer);
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(PersonLookupInfo searchInfo, CancellationToken cancellationToken)
        {
            return new List<RemoteSearchResult>();
        }

        public async Task<MetadataResult<Person>> GetMetadata(PersonLookupInfo info, CancellationToken cancellationToken)
        {
            MetadataResult<Person> result = new MetadataResult<Person>();

            string cid = info.GetProviderId(OpenDoubanPlugin.ProviderID);
            logger.LogInformation($"[Open DOUBAN] Person GetMetadata of [cid]: \"{cid}\"");
            ApiCelebrity c = await apiClient.GetCelebrityByCid(cid);
            
            if(c != null) {
                Person p = new Person
                {
                    Name = c.Name,
                    HomePageUrl = c.Site,
                    Overview = c.Intro,
                    PremiereDate = DateTime.ParseExact(c.Birthdate, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture)
                };

                p.SetProviderId(OpenDoubanPlugin.ProviderID, c.Id);

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

        public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            logger.LogInformation("[Open DOUBAN] Person GetImageResponse url: {0}", url);
            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Douban
{
    public class MovieProvider : IHasOrder, IRemoteMetadataProvider<Movie, MovieInfo>
    {
        public string Name => "Douban Movie Provider";
        public int Order => 3;

        private ApiClient apiClient;
        private ILogger logger;
        private IHttpClientFactory httpClientFactory;

        public MovieProvider(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer, ILogger<MovieProvider> logger)
        {
            this.apiClient = new ApiClient(httpClientFactory, jsonSerializer);
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }
        
        public async Task<MetadataResult<Movie>> GetMetadata(MovieInfo info, CancellationToken cancellationToken)
        {
            ApiSubject subject = null;

            string sid = info.GetProviderId(BaseProvider.ProviderID);
            if(!string.IsNullOrEmpty(sid))
            {
                logger.LogInformation($"[DOUBAN] GetMetadata of [sid]: \"{sid}\"");
                subject = await apiClient.GetBySid(sid);
            }
            else if (!string.IsNullOrEmpty(info.Name))
            {
                List<ApiSubject> res = await apiClient.PartialSearch(info.Name);
                var has = res.Where<ApiSubject>(x => x.Name.Equals(info.Name) || x.OriginalName.Equals(info.Name));
                if (has.Any())
                {
                    subject = await apiClient.GetBySid(has.FirstOrDefault().Sid);
                }
            }

            var result = new MetadataResult<Movie>();
            if(subject == null) return result;

            var x = subject;
            // int year = 0; int.TryParse(x?.Year, out year);
            // float rating = 0; float.TryParse(x?.Rating, out rating);
            result.Item = new Movie
            {
                ProviderIds = new Dictionary<string, string> {{BaseProvider.ProviderID, x.Sid}},
                Name = x?.Name,
                OriginalTitle = x?.Subname,
                CommunityRating = x?.Rating,
                Overview = x?.Intro,
                ProductionYear = x?.Year,
                HomePageUrl = "https://www.douban.com",
                // ProductionLocations = [x?.Country],
                // PremiereDate = null,   
            };

            info.SetProviderId(BaseProvider.ProviderID, x.Sid);
            result.QueriedById = true;
            result.HasMetadata = true;
            return result;
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(MovieInfo info, CancellationToken cancellationToken)
        {
            List<ApiSubject> list = new List<ApiSubject>();

            string sid = info.GetProviderId(BaseProvider.ProviderID);
            if (!string.IsNullOrEmpty(sid))
            {
                logger.LogInformation($"[DOUBAN] GetSearchResults of [sid]: \"{sid}\"");
                ApiSubject res = await apiClient.GetBySid(sid);
                list.Add(res);
            }
            else if (!string.IsNullOrEmpty(info.Name))
            {
                logger.LogInformation($"[DOUBAN] GetSearchResults of [name]: \"{info.Name}\"");
                List<ApiSubject> res = await apiClient.PartialSearch(info.Name);
                list.AddRange(res);
            }

            if(!list.Any()) 
            {
                logger.LogInformation($"[DOUBAN] GetSearchResults Found Nothing...");
            }

            return list.Select(x =>
            {
                // int year = 0; int.TryParse(x?.Year, out year);
                return new RemoteSearchResult
                {
                    ProviderIds = new Dictionary<string, string>{{ BaseProvider.ProviderID, x.Sid }},
                    ImageUrl = x?.Img,
                    ProductionYear = x?.Year,
                    Name = x?.Name
                };
            });
        }

        public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            logger.LogInformation("[DOUBAN] GetImageResponse url: {0}", url);
            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}

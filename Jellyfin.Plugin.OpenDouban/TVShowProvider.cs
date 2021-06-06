using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;
using Jellyfin.Plugin.OpenDouban.Service;

namespace Jellyfin.Plugin.OpenDouban
{
    public class TVShowProvider : IHasOrder, IRemoteMetadataProvider<Series, SeriesInfo>, IRemoteMetadataProvider<Season, SeasonInfo>
    {
        public string Name => "Open Douban TV Provider";
        public int Order => 3;
        private ApiClient apiClient;
        private ILogger logger;
        private IHttpClientFactory httpClientFactory;

        public TVShowProvider(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer, ILogger<TVShowProvider> logger)
        {
            this.apiClient = new ApiClient(httpClientFactory, jsonSerializer);
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        #region series
        public async Task<MetadataResult<Series>> GetMetadata(SeriesInfo info, CancellationToken cancellationToken)
        {
            ApiSubject subject = null;

            string sid = info.GetProviderId(OpenDoubanPlugin.ProviderID);

            if (!string.IsNullOrEmpty(sid))
            {
                logger.LogInformation($"[Open DOUBAN] Series GetMetadata of [sid]: \"{sid}\"");
                subject = await apiClient.GetBySid(sid);
            }
            else if (!string.IsNullOrEmpty(info.Name))
            {
                List<ApiSubject> res = await apiClient.PartialSearch(info.Name);
                var has = res.Where<ApiSubject>(x => x.Name.Equals(info.Name) || x.OriginalName.Equals(info.Name));
                if (has.Any())
                {
                    sid = has.FirstOrDefault().Sid;
                    subject = await apiClient.GetBySid(sid);
                }
            }

            var result = new MetadataResult<Series>();
            if (subject == null) return result;

            var x = subject;

            result.Item = new Series
            {
                ProviderIds = new Dictionary<string, string> { { OpenDoubanPlugin.ProviderID, x.Sid } },
                Name = x?.Name,
                OriginalTitle = x?.Subname,
                CommunityRating = x?.Rating,
                Overview = x?.Intro,
                ProductionYear = x?.Year,
                HomePageUrl = "https://www.douban.com",
                // ProductionLocations = [x?.Country],
                // PremiereDate = null,   
            };

            info.SetProviderId(OpenDoubanPlugin.ProviderID, x.Sid);
            if(!string.IsNullOrEmpty(x.Imdb)) {
                info.SetProviderId(MetadataProvider.Imdb, x.Imdb);
            }
            
            result.QueriedById = true;
            result.HasMetadata = true;
            
            x.Celebrities = await apiClient.GetCelebritiesBySid(sid);

            x.Celebrities.ForEach(c => result.AddPerson(new MediaBrowser.Controller.Entities.PersonInfo
            {
                Name = c.Name,
                Type = c.Role.Equals("导演") ? PersonType.Director : c.Role.Equals("演员") ? PersonType.Actor : c.Role,
                Role = c.Role,
                ImageUrl = c.Img,
                ProviderIds = new Dictionary<string, string> { { OpenDoubanPlugin.ProviderID, c.Id } },
            }));

            return result;
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeriesInfo info, CancellationToken cancellationToken)
        {
            List<ApiSubject> list = new List<ApiSubject>();

            string sid = info.GetProviderId(OpenDoubanPlugin.ProviderID);
            if (!string.IsNullOrEmpty(sid))
            {
                logger.LogInformation($"[Open DOUBAN] Series GetSearchResults of [sid]: \"{sid}\"");
                ApiSubject res = await apiClient.GetBySid(sid);
                list.Add(res);
            }
            else if (!string.IsNullOrEmpty(info.Name))
            {
                logger.LogInformation($"[Open DOUBAN] Series GetSearchResults of [name]: \"{info.Name}\"");
                List<ApiSubject> res = await apiClient.PartialSearch(info.Name);
                list.AddRange(res);
            }

            if (!list.Any())
            {
                logger.LogInformation($"[Open DOUBAN] Series GetSearchResults Found Nothing...");
            }

            return list.Select(x =>
            {
                // int year = 0; int.TryParse(x?.Year, out year);
                return new RemoteSearchResult
                {
                    ProviderIds = new Dictionary<string, string> { { OpenDoubanPlugin.ProviderID, x.Sid } },
                    ImageUrl = x?.Img,
                    ProductionYear = x?.Year,
                    Name = x?.Name
                };
            });
        }
        #endregion series

        #region season
        public async Task<MetadataResult<Season>> GetMetadata(SeasonInfo info, CancellationToken cancellationToken)
        {
            ApiSubject subject = null;

            string sid = info.GetProviderId(OpenDoubanPlugin.ProviderID);
            if(string.IsNullOrEmpty(sid)) {
                sid =  info.SeriesProviderIds.Where(x => x.Key.Equals(OpenDoubanPlugin.ProviderID))?.FirstOrDefault().Value;
            }

            if (!string.IsNullOrEmpty(sid))
            {
                logger.LogInformation($"[Open DOUBAN] Season GetMetadata of [sid]: \"{sid}\"");
                subject = await apiClient.GetBySid(sid);
            }
            else if (!string.IsNullOrEmpty(info.Name))
            {
                List<ApiSubject> res = await apiClient.PartialSearch(info.Name);
                var has = res.Where<ApiSubject>(x => x.Name.Equals(info.Name) || x.OriginalName.Equals(info.Name));
                if (has.Any())
                {
                    sid = has.FirstOrDefault().Sid;
                    subject = await apiClient.GetBySid(sid);
                }
            }

            var result = new MetadataResult<Season>();
            if (subject == null) return result;

            var x = subject;

            result.Item = new Season
            {
                ProviderIds = new Dictionary<string, string> { { OpenDoubanPlugin.ProviderID, x.Sid } },
                Name = x?.Name,
                OriginalTitle = x?.Subname,
                CommunityRating = x?.Rating,
                Overview = x?.Intro,
                ProductionYear = x?.Year,
                HomePageUrl = "https://www.douban.com",
                // ProductionLocations = [x?.Country],
                // PremiereDate = null,   
            };

            info.SetProviderId(OpenDoubanPlugin.ProviderID, x.Sid);
            result.QueriedById = true;
            result.HasMetadata = true;

            if(x.Celebrities == null || !x.Celebrities.Any()) {
                // Load Persons & nice to have
                x.Celebrities = await apiClient.GetCelebritiesBySid(sid);
            } 
            x.Celebrities.ForEach(c => result.AddPerson(new MediaBrowser.Controller.Entities.PersonInfo
            {
                Name = c.Name,
                Type = c.Role.Equals("导演") ? PersonType.Director : c.Role.Equals("演员") ? PersonType.Actor : c.Role,
                Role = c.Role,
                ImageUrl = c.Img,
                ProviderIds = new Dictionary<string, string> { { OpenDoubanPlugin.ProviderID, c.Id } },
            }));

            return result;
        }

        public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SeasonInfo info, CancellationToken cancellationToken)
        {
            List<ApiSubject> list = new List<ApiSubject>();

            string sid = info.GetProviderId(OpenDoubanPlugin.ProviderID);
            if(string.IsNullOrEmpty(sid)) {
                sid =  info.SeriesProviderIds.Where(x => x.Key.Equals(OpenDoubanPlugin.ProviderID))?.FirstOrDefault().Value;
            }

            if (!string.IsNullOrEmpty(sid))
            {
                logger.LogInformation($"[Open DOUBAN] Season GetSearchResults of [sid]: \"{sid}\"");
                ApiSubject res = await apiClient.GetBySid(sid);
                list.Add(res);
            }
            else if (!string.IsNullOrEmpty(info.Name))
            {
                logger.LogInformation($"[Open DOUBAN] Season GetSearchResults of [name]: \"{info.Name}\"");
                List<ApiSubject> res = await apiClient.PartialSearch(info.Name);
                list.AddRange(res);
            }

            if (!list.Any())
            {
                logger.LogInformation($"[Open DOUBAN] Season GetSearchResults Found Nothing...");
            }

            return list.Select(x =>
            {
                // int year = 0; int.TryParse(x?.Year, out year);
                return new RemoteSearchResult
                {
                    ProviderIds = new Dictionary<string, string> { { OpenDoubanPlugin.ProviderID, x.Sid } },
                    ImageUrl = x?.Img,
                    ProductionYear = x?.Year,
                    Name = x?.Name
                };
            });
        }
        #endregion season
        public async Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            logger.LogInformation("[DOUBAN] GetImageResponse url: {0}", url);
            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}

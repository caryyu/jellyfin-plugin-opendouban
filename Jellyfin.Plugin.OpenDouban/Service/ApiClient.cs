using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.OpenDouban.Service
{
    public sealed class ApiClient
    {
        private string ApiBaseUri => OpenDoubanPlugin.Instance?.Configuration.ApiBaseUri == null ? "http://localhost:5000" : OpenDoubanPlugin.Instance?.Configuration.ApiBaseUri;

        private IHttpClientFactory httpClientFactory;
        private IJsonSerializer jsonSerializer;

        public ApiClient(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer)
        {
            this.httpClientFactory = httpClientFactory;
            this.jsonSerializer = jsonSerializer;
        }

        public async Task<List<ApiSubject>> FullSearch(string keyword)
        {
            string url = $"{ApiBaseUri}/fullsearch?q={keyword}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Stream content = await response.Content.ReadAsStreamAsync();
            List<ApiSubject> result = await jsonSerializer.DeserializeFromStreamAsync<List<ApiSubject>>(content);
            return result;
        }

        public async Task<List<ApiSubject>> PartialSearch(string keyword)
        {
            string url = $"{ApiBaseUri}/partialsearch?q={keyword}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Stream content = await response.Content.ReadAsStreamAsync();
            List<ApiSubject> result = await jsonSerializer.DeserializeFromStreamAsync<List<ApiSubject>>(content);
            return result;
        }

        public async Task<ApiSubject> GetBySid(string sid)
        {
            string url = $"{ApiBaseUri}/fetchbysid?sid={sid}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Stream content = await response.Content.ReadAsStreamAsync();
            ApiSubject result = await jsonSerializer.DeserializeFromStreamAsync<ApiSubject>(content);
            return result;
        }

        public async Task<List<ApiCelebrity>> GetCelebritiesBySid(string sid)
        {
            string url = $"{ApiBaseUri}/fetchcelebritiesbysid?sid={sid}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            if(!response.IsSuccessStatusCode) 
            {
                return new List<ApiCelebrity>();
            }
            Stream content = await response.Content.ReadAsStreamAsync();
            List<ApiCelebrity> result = await jsonSerializer.DeserializeFromStreamAsync<List<ApiCelebrity>>(content);
            return result;
        }
    }
}

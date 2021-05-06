using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Douban
{
    public sealed class ApiClient
    {
        private const string ApiBaseUri = "http://localhost:5000";
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
    }
}

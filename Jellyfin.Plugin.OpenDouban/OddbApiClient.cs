using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using MediaBrowser.Model.Serialization;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.OpenDouban
{
    /// <summary>
    /// OddbApiClient.
    /// </summary>
    public sealed class OddbApiClient
    {
        private IHttpClientFactory httpClientFactory;
        private IJsonSerializer jsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="OddbApiClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        /// <param name="jsonSerializer">Instance of the <see cref="IJsonSerializer"/> interface.</param>
        public OddbApiClient(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer)
        {
            this.httpClientFactory = httpClientFactory;
            this.jsonSerializer = jsonSerializer;
        }

        /// <summary>
        /// ApiBaseUri
        /// </summary>
        private string _apiBaseUri;
        public string ApiBaseUri 
        {
            get 
            {
                if(string.IsNullOrEmpty(_apiBaseUri)) 
                {
                    return OddbPlugin.Instance?.Configuration.ApiBaseUri;
                }
                return _apiBaseUri;
            }
            set { _apiBaseUri = value;}
        }

        public async Task<List<ApiSubject>> FullSearch(string keyword)
        {
            string url = $"{ApiBaseUri}/movies?q={keyword}&type=full";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Stream content = await response.Content.ReadAsStreamAsync();
            List<ApiSubject> result = await jsonSerializer.DeserializeFromStreamAsync<List<ApiSubject>>(content);
            return result;
        }

        public async Task<List<ApiSubject>> PartialSearch(string keyword)
        {
            string url = $"{ApiBaseUri}/movies?q={keyword}&type=partial";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Stream content = await response.Content.ReadAsStreamAsync();
            List<ApiSubject> result = await jsonSerializer.DeserializeFromStreamAsync<List<ApiSubject>>(content);
            return result;
        }

        public async Task<ApiSubject> GetBySid(string sid)
        {
            string url = $"{ApiBaseUri}/movies/{sid}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Stream content = await response.Content.ReadAsStreamAsync();
            ApiSubject result = await jsonSerializer.DeserializeFromStreamAsync<ApiSubject>(content);
            return result;
        }

        public async Task<List<ApiCelebrity>> GetCelebritiesBySid(string sid)
        {
            string url = $"{ApiBaseUri}/movies/{sid}/celebrities";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            if(!response.IsSuccessStatusCode) 
            {
                return new List<ApiCelebrity>();
            }
            Stream content = await response.Content.ReadAsStreamAsync();
            List<ApiCelebrity> result = await jsonSerializer.DeserializeFromStreamAsync<List<ApiCelebrity>>(content);
            return result;
        }

        public async Task<ApiCelebrity> GetCelebrityByCid(string cid)
        {
            string url = $"{ApiBaseUri}/celebrities/{cid}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            if(!response.IsSuccessStatusCode) 
            {
                return null;
            }
            Stream content = await response.Content.ReadAsStreamAsync();
            ApiCelebrity result = await jsonSerializer.DeserializeFromStreamAsync<ApiCelebrity>(content);
            return result;
        }

        public async Task<List<ApiPhoto>> GetPhotoBySid(string sid)
        {
            string url = $"{ApiBaseUri}/photo/{sid}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            if(!response.IsSuccessStatusCode) 
            {
                return null;
            }
            Stream content = await response.Content.ReadAsStreamAsync();
            List<ApiPhoto> result = await jsonSerializer.DeserializeFromStreamAsync<List<ApiPhoto>>(content);
            return result;
        }
    }

    public class ApiSubject
    {
        // "name": "哈利·波特与魔法石 Harry Potter and the Sorcerer's Stone",
        private string name;
        public string Name
        {
            get
            {
                return name.Split(" ", 2)[0].Trim();
            }
            set
            {
                name = value;
            }
        }
        public string FullName
        {
            get { return name; }
        }
        public string OriginalName
        {
            get
            {
                var names = name.Split(" ", 2);
                return names.Length > 1 ? names[1].Trim() : names[0].Trim();
            }
        }
        // "rating": "9.1",
        public float Rating { get; set; }
        // "img": "https://img9.doubanio.com/view/photo/s_ratio_poster/public/p2614949805.webp",
        public string Img { get; set; }
        // "sid": "1295038",
        public string Sid { get; set; }
        // "year": "2001",
        public int Year { get; set; }
        // "director": "克里斯·哥伦布",
        public string Director { get; set; }
        // "writer": "史蒂夫·克洛夫斯 / J·K·罗琳",
        public string Writer { get; set; }
        // "actor": "丹尼尔·雷德克里夫 / 艾玛·沃森 / 鲁伯特·格林特 / 艾伦·瑞克曼 / 玛吉·史密斯 / 更多...",
        public string Actor { get; set; }
        // "genre": "奇幻 / 冒险",
        public string Genre { get; set; }
        // "site": "www.harrypotter.co.uk",
        public string Site { get; set; }
        // "country": "美国 / 英国",
        public string Country { get; set; }
        // "language": "英语",
        public string Language { get; set; }
        // "screen": "2002-01-26(中国大陆) / 2020-08-14(中国大陆重映) / 2001-11-04(英国首映) / 2001-11-16(美国)",
        public string Screen { get; set; }
        // "duration": "152分钟 / 159分钟(加长版)",
        public string Duration { get; set; }
        // "subname": "哈利波特1：神秘的魔法石(港/台) / 哈1 / Harry Potter and the Philosopher's Stone",
        public string Subname { get; set; }
        // "imdb": "tt0241527"
        public string Imdb { get; set; }
        public string Intro { get; set; }

        public List<ApiCelebrity> Celebrities { get; set;}
    }

    public class ApiCelebrity
    {
        public string Id {get;set;}
        public string Name {get;set;}
        public string Img {get;set;}
        public string Role {get;set;}
        public string Intro {get;set;}
        public string Gender {get;set;}
        public string Constellation {get;set;}
        public string Birthdate {get;set;}
        public string Birthplace {get;set;}
        public string Nickname {get;set;}
        public string Imdb {get;set;}
        public string Site {get;set;}
    }

    public class ApiPhoto
    {
      public string Id {get;set;}
      public string Small {get;set;}
      public string Medium { get; set; }
      public string Large { get; set; }
      public string Size { get; set; }
      public int Width { get; set; }
      public int Height { get; set; }
    }
}

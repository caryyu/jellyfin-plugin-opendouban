using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Jellyfin.Plugin.OpenDouban
{
    /// <summary>
    /// OddbApiClient.
    /// </summary>
    public sealed class OddbApiClient
    {
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OddbApiClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        public OddbApiClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public string ApiBaseUri { get; set; } = OddbPlugin.Instance?.Configuration.ApiBaseUri;

        public async Task<List<ApiSubject>> FullSearch(string keyword, CancellationToken cancellationToken = default)
        {
            string url = $"{ApiBaseUri}/movies?q={keyword}&type=full";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ApiSubject>>(cancellationToken: cancellationToken);
        }

        public async Task<List<ApiSubject>> PartialSearch(string keyword, CancellationToken cancellationToken = default)
        {
            string url = $"{ApiBaseUri}/movies?q={keyword}&type=partial";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ApiSubject>>(cancellationToken: cancellationToken);
        }

        public async Task<ApiSubject> GetBySid(string sid, CancellationToken cancellationToken = default)
        {
            string url = $"{ApiBaseUri}/movies/{sid}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ApiSubject>(cancellationToken: cancellationToken);
        }

        public async Task<List<ApiCelebrity>> GetCelebritiesBySid(string sid, CancellationToken cancellationToken = default)
        {
            string url = $"{ApiBaseUri}/movies/{sid}/celebrities";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return new List<ApiCelebrity>();
            }
            return await response.Content.ReadFromJsonAsync<List<ApiCelebrity>>(cancellationToken: cancellationToken);
        }

        public async Task<ApiCelebrity> GetCelebrityByCid(string cid, CancellationToken cancellationToken = default)
        {
            string url = $"{ApiBaseUri}/celebrities/{cid}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<ApiCelebrity>(cancellationToken: cancellationToken);
        }

        public async Task<List<ApiPhoto>> GetPhotoBySid(string sid, CancellationToken cancellationToken = default)
        {
            string url = $"{ApiBaseUri}/photo/{sid}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadFromJsonAsync<List<ApiPhoto>>(cancellationToken: cancellationToken);
        }
    }

    public class ApiSubject
    {
        // "name": "哈利·波特与魔法石",
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        // "originalName": "Harry Potter and the Sorcerer's Stone",
        /// <summary>
        /// 原始名称
        /// </summary>
        public string OriginalName { get; set; }
        // "rating": "9.1",
        /// <summary>
        /// 评分
        /// </summary>
        [JsonConverter(typeof(StringFloatConverter))]
        public float Rating { get; set; }
        // "img": "https://img9.doubanio.com/view/photo/s_ratio_poster/public/p2614949805.webp",
        /// <summary>
        /// 封面
        /// </summary>
        public string Img { get; set; }
        // "sid": "1295038",
        /// <summary>
        /// 豆瓣id
        /// </summary>
        public string Sid { get; set; }
        // "year": "2001",
        /// <summary>
        /// 上映年份
        /// </summary>
        public int Year { get; set; }
        // "director": "克里斯·哥伦布",
        /// <summary>
        /// 导演
        /// </summary>
        public string Director { get; set; }
        // "writer": "史蒂夫·克洛夫斯 / J·K·罗琳",
        /// <summary>
        /// 编剧
        /// </summary>
        public string Writer { get; set; }
        // "actor": "丹尼尔·雷德克里夫 / 艾玛·沃森 / 鲁伯特·格林特 / 艾伦·瑞克曼 / 玛吉·史密斯 / 更多...",
        /// <summary>
        /// 主演
        /// </summary>
        public string Actor { get; set; }
        // "genre": "奇幻 / 冒险",
        /// <summary>
        /// 类型
        /// </summary>
        public string Genre { get; set; }
        // "site": "www.harrypotter.co.uk",
        /// <summary>
        /// 官方网站
        /// </summary>
        public string Site { get; set; }
        // "country": "美国 / 英国",
        /// <summary>
        /// 制片国家/地区
        /// </summary>
        public string Country { get; set; }
        // "language": "英语",
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        // "screen": "2002-01-26(中国大陆) / 2020-08-14(中国大陆重映) / 2001-11-04(英国首映) / 2001-11-16(美国)",
        /// <summary>
        /// 上映日期
        /// </summary>
        public string Screen { get; set; }
        public DateTime? ScreenTime
        {
            get
            {
                if(string.IsNullOrWhiteSpace(Screen)) return null;
                var items = Screen.Split("/");
                if (items.Length >= 0)
                {
                    var item = items[0].Split("(")[0];
                    DateTime result;
                    DateTime.TryParseExact(item, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out result);
                    return result;
                }
                return null;
            }
        }
        // "duration": "152分钟 / 159分钟(加长版)",
        /// <summary>
        /// 片长
        /// </summary>
        public string Duration { get; set; }
        // "subname": "哈利波特1：神秘的魔法石(港/台) / 哈1 / Harry Potter and the Philosopher's Stone",
        /// <summary>
        /// 又名
        /// </summary>
        public string Subname { get; set; }
        // "imdb": "tt0241527"
        /// <summary>
        /// imdb id
        /// </summary>
        public string Imdb { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 演职人员
        /// </summary>
        public List<ApiCelebrity> Celebrities { get; set; }
    }

    public class ApiCelebrity
    {
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 照片
        /// </summary>
        public string Img { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 星座
        /// </summary>
        public string Constellation { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthdate { get; set; }
        /// <summary>
        /// 出生地
        /// </summary>
        public string Birthplace { get; set; }
        /// <summary>
        /// 更多外文名
        /// </summary>
        public string Nickname { get; set; }
        /// <summary>
        /// 家庭成员
        /// </summary>
        public string Family { get; set; }
        /// <summary>
        /// imdb编号
        /// </summary>
        public string Imdb { get; set; }
        public string Site { get; set; }
    }

    public class ApiPhoto
    {
        public string Id { get; set; }
        /// <summary>
        /// 小图
        /// </summary>
        public string Small { get; set; }
        /// <summary>
        /// 中图
        /// </summary>
        public string Medium { get; set; }
        /// <summary>
        /// 大图
        /// </summary>
        public string Large { get; set; }
        public string Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}

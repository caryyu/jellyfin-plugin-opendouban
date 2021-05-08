using System.Collections.Generic;

namespace Jellyfin.Plugin.Douban
{
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
    }
}
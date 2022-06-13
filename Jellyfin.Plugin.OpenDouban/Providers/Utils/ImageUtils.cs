using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.OpenDouban.Providers.Utils
{
    class ImageUtils
    {
        private static Regex regImageDomain = new Regex(@"//img\d+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 替换图片域为没防盗链的img2
        /// </summary>
        public static string FixForbiddenImageDomain(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            return regImageDomain.Replace(url, "img2");
        }
    }
}

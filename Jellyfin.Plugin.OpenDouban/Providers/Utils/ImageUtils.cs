using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jellyfin.Plugin.OpenDouban.Providers.Utils
{
    class ImageUtils
    {
        public static string GetHighQualityImage(string url, string quality)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }

            switch(quality)
            {
                case "medium":
                    return url.Replace("s_ratio_poster", "m");
                case "large":
                    return url.Replace("s_ratio_poster", "l");
                default:
                    return url;
            }
            
        }
    }
}

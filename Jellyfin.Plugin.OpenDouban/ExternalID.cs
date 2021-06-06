using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.OpenDouban
{
    public class DoubanExternalId : IExternalId
    {
        public string ProviderName => "Open Douban";

        public string Key => OpenDoubanPlugin.ProviderID;

        public ExternalIdMediaType? Type => null;

        public string UrlFormatString => "https://movie.douban.com/subject/{0}/";

        public bool Supports(IHasProviderIds item)
        {
            return item is Movie || item is Series || item is Person;
        }
    }
}

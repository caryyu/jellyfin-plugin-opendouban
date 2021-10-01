
using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.OpenDouban.Providers.ExternalId
{
    /// <inheritdoc />
    public class OddbSeriesExternalId : IExternalId
    {
        /// <inheritdoc />
        public string ProviderName => OddbPlugin.ProviderName;

        /// <inheritdoc />
        public string Key => OddbPlugin.ProviderId;

        /// <inheritdoc />
        public ExternalIdMediaType? Type => ExternalIdMediaType.Series;

        /// <inheritdoc />
        public string UrlFormatString => "https://movie.douban.com/subject/{0}/";

        /// <inheritdoc />
        public bool Supports(IHasProviderIds item) => item is Series;
    }
}
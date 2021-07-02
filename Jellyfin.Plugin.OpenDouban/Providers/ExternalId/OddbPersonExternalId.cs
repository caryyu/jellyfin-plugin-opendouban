
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.OpenDouban.Providers.ExternalId
{
    /// <inheritdoc />
    public class OddbPersonExternalId : IExternalId
    {
        /// <inheritdoc />
        public string ProviderName => OddbPlugin.ProviderName;

        /// <inheritdoc />
        public string Key => OddbPlugin.ProviderId;

        /// <inheritdoc />
        public ExternalIdMediaType? Type => ExternalIdMediaType.Person;

        /// <inheritdoc />
        public string UrlFormatString => "https://movie.douban.com/celebrity/{0}/";

        /// <inheritdoc />
        public bool Supports(IHasProviderIds item) => item is Person;
    }
}
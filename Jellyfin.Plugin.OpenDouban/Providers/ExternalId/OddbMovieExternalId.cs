
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.OpenDouban.Providers.ExternalId
{
    /// <inheritdoc />
    public class OddbMovieExternalId : IExternalId
    {
        /// <inheritdoc />
        public string ProviderName => OddbPlugin.ProviderName;

        /// <inheritdoc />
        public string Key => OddbPlugin.ProviderId;

        /// <inheritdoc />
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;

        /// <inheritdoc />
        public string UrlFormatString => "https://movie.douban.com/subject/{0}/";

        /// <inheritdoc />
        public bool Supports(IHasProviderIds item) => item is Movie;
    }
}
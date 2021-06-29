using System;
using System.Collections.Generic;
using Jellyfin.Plugin.OpenDouban.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.OpenDouban
{
    /// <summary>
    /// Oddb Plugin.
    /// </summary>
    public class OddbPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        /// <summary>
        /// Gets the provider name.
        /// </summary>
        public const string ProviderName = "OpenDouban";
        
        /// <summary>
        /// Gets the provider id.
        /// </summary>
        public static string ProviderID = "OpenDoubanID";

        /// <summary>
        /// Initializes a new instance of the <see cref="OddbPlugin"/> class.
        /// </summary>
        /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
        /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
        public OpenDoubanPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) 
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        /// <summary>
        /// Gets current plugin instance.
        /// </summary>
        public static OpenDoubanPlugin Instance { get; private set; }

        /// <inheritdoc />
        public override string Name => "OpenDouban";
        
        /// <inheritdoc />
        public override Guid Id => Guid.Parse("7834517B-1A9A-4758-9DD3-73FE02C98AA3");

        /// <inheritdoc />
        public IEnumerable<PluginPageInfo> GetPages()
        {
            yield return new PluginPageInfo
            {
                Name = Name,
                EmbeddedResourcePath = $"{GetType().Namespace}.Configuration.config.html"
            };
        }
    }
}

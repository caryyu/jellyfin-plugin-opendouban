using System;
using System.Collections.Generic;
using Jellyfin.Plugin.OpenDouban.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.OpenDouban
{
    public class OpenDoubanPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public override string Name => "Open Douban";
        public override Guid Id => Guid.Parse("7834517B-1A9A-4758-9DD3-73FE02C98AA3");
        public static OpenDoubanPlugin Instance { get; private set; }

        public static string ProviderID = "OpenDoubanID";

        public OpenDoubanPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = this.Name,
                    EmbeddedResourcePath = string.Format("{0}.Configuration.configPage.html", GetType().Namespace)
                }
            };
        }
    }
}

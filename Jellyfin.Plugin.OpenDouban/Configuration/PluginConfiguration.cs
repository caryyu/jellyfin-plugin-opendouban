using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.OpenDouban.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        // public string ApiKey { get; set; }
        // public int MinRequestInternalMs { get; set; }
        public PluginConfiguration()
        {
            // MinRequestInternalMs = 2000;
            ApiBaseUri = "http://localhost:5000";
        }

        public string ApiBaseUri  { get; set; }
    }
}

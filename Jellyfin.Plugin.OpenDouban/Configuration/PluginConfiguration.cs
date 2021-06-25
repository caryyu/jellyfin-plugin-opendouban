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
            Pattern = @"(S\d{2}|E\d{2}|HDR|\d{3,4}p|WEBRip|WEB|YIFY|BrRip|BluRay|H265|H264|x264|AAC\.\d\.\d|AAC|HDTV|mkv|mp4)|(\[.*\])|(\-\w+|\{.*\}|【.*】|\(.*\)|\d+MB)|(\.|\-)";
        }

        public string ApiBaseUri  { get; set; }
        public string Pattern { get; set; }
    }
}

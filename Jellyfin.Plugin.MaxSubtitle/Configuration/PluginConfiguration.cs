using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.MaxSubtitle.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public PluginConfiguration()
        {
            ApiBaseUri = "http://localhost:3000";
        }

        public string ApiBaseUri { get; set; }
    }
}

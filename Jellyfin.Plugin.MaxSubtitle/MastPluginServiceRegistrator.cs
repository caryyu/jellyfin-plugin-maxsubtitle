using MediaBrowser.Common.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Jellyfin.Plugin.MaxSubtitle
{
    /// <summary>
    /// Register max subtitle services.
    /// </summary>
    public class MastPluginServiceRegistrator : IPluginServiceRegistrator
    {
        /// <inheritdoc />
        public void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<MastApiClient>();
        }
    }
}

using System;
using System.Collections.Generic;
using Jellyfin.Plugin.MaxSubtitle.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.MaxSubtitle
{
    /// <summary>
    /// Mast Plugin.
    /// </summary>
    public class MastPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        /// <summary>
        /// Gets the provider name.
        /// </summary>
        public const string ProviderName = "MaxSubtitle";

        /// <summary>
        /// Gets the provider id.
        /// </summary>
        public static string ProviderId = "MaxSubtitleID";

        /// <summary>
        /// Initializes a new instance of the <see cref="MastPlugin"/> class.
        /// </summary>
        /// <param name="applicationPaths">Instance of the <see cref="IApplicationPaths"/> interface.</param>
        /// <param name="xmlSerializer">Instance of the <see cref="IXmlSerializer"/> interface.</param>
        public MastPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        /// <summary>
        /// Gets current plugin instance.
        /// </summary>
        public static MastPlugin Instance { get; private set; }

        /// <inheritdoc />
        public override string Name => "MaxSubtitle";

        /// <inheritdoc />
        public override Guid Id => Guid.Parse("4EAEDA51-B4CF-45F8-99B3-EB2B1F6D3A53");

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

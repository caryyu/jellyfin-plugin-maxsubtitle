using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.MaxSubtitle
{
    /// <summary>
    /// MastSubtitleProvider.
    /// </summary>
    public class MastSubtitleProvider : ISubtitleProvider
    {
        private ILogger<MastSubtitleProvider> _logger;
        private IHttpClientFactory _httpClientFactory;
        private MastApiClient _mastApiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="OddbPersonProvider"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        /// <param name="logger">Instance of the <see cref="ILogger{MastSubtitleProvider}"/> interface.</param>
        /// <param name="mastApiClient">Instance of <see cref="MastApiClient"/>.</param>
        public MastSubtitleProvider(IHttpClientFactory httpClientFactory, ILogger<MastSubtitleProvider> logger, MastApiClient mastApiClient)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _mastApiClient = mastApiClient;
        }

        /// <inheritdoc />
        public string Name => MastPlugin.ProviderName;

        /// <inheritdoc />
        public IEnumerable<VideoContentType> SupportedMediaTypes => new[] { VideoContentType.Episode, VideoContentType.Movie };

        /// <inheritdoc />
        public async Task<IEnumerable<RemoteSubtitleInfo>> Search(SubtitleSearchRequest info, CancellationToken cancellationToken)
        {
            List<ApiSubtitle> list = new List<ApiSubtitle>();

            if (!string.IsNullOrEmpty(info.Name))
            {
                _logger.LogInformation($"[Max Subtitle] Search of [name]: \"{info.Name}\"");
                List<ApiSubtitle> res = await _mastApiClient.Search(info.Name, cancellationToken);
                list.AddRange(res);
            }

            if (!list.Any())
            {
                _logger.LogInformation($"[Max Subtitle] Search Found Nothing...");
            }

            return list.Select(x =>
            {
                // int year = 0; int.TryParse(x?.Year, out year);
                return new RemoteSubtitleInfo
                {
                    Author = "caryyu",
                    Comment = x?.Desc,
                    CommunityRating = 0,
                    DownloadCount = 0,
                    ProviderName = Name,
                    Format = x?.Format,
                    Id = x?.Id,
                    Name = x?.Name,
                    ThreeLetterISOLanguageName = "Chi",
                    IsHashMatch = false,
                };
            });
        }

        /// <inheritdoc />
        public async Task<SubtitleResponse> GetSubtitles(string id, CancellationToken cancellationToken)
        {
            var s = await _mastApiClient.Download(id, cancellationToken);

            return new SubtitleResponse
            {
                Format = s.Format,
                Language = "Chi",
                Stream = s.Content,
            };
        }
    }
}
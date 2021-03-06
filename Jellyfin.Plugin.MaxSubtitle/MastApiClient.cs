using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http.Json;

namespace Jellyfin.Plugin.MaxSubtitle
{
    /// <summary>
    /// MastApiClient.
    /// </summary>
    public sealed class MastApiClient
    {
        private IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MastApiClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        public MastApiClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// ApiBaseUri
        /// </summary>
        private string _apiBaseUri;
        public string ApiBaseUri
        {
            get
            {
                if (string.IsNullOrEmpty(_apiBaseUri))
                {
                    return MastPlugin.Instance?.Configuration.ApiBaseUri;
                }
                return _apiBaseUri;
            }
            set { _apiBaseUri = value; }
        }

        public async Task<List<ApiSubtitle>> Search(string keyword, CancellationToken cancellationToken = default)
        {
            string url = $"{ApiBaseUri}/subtitle/search/{keyword}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            List<ApiSubtitle> result = await response.Content.ReadFromJsonAsync<List<ApiSubtitle>>(cancellationToken: cancellationToken);
            return result;
        }

        public async Task<Subtitle> Download(string id, CancellationToken cancellationToken = default)
        {
            string url = $"{ApiBaseUri}/subtitle/{id}/download";
            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Stream content = await response.Content.ReadAsStreamAsync(cancellationToken);

            string filename = response.Content.Headers.ContentDisposition.FileName;
            string ext = Path.GetExtension(filename).Remove(0, 1);

            return new Subtitle
            {
                Format = ext,
                Content = content,
            };
        }
    }

    public class ApiSubtitle
    {
        public string Id { get; set; }
        public string OriginalId { get; set; }
        public string Desc { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Format { get; set; }
    }

    public class Subtitle
    {
        public string Format { get; set; }
        public Stream Content { get; set; }
    }
}

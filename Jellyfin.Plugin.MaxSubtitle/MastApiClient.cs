using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using MediaBrowser.Model.Serialization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;

namespace Jellyfin.Plugin.MaxSubtitle
{
    /// <summary>
    /// MastApiClient.
    /// </summary>
    public sealed class MastApiClient
    {
        private IHttpClientFactory httpClientFactory;
        private IJsonSerializer jsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MastApiClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Instance of the <see cref="IHttpClientFactory"/> interface.</param>
        /// <param name="jsonSerializer">Instance of the <see cref="IJsonSerializer"/> interface.</param>
        public MastApiClient(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer)
        {
            this.httpClientFactory = httpClientFactory;
            this.jsonSerializer = jsonSerializer;
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

        public async Task<List<ApiSubtitle>> Search(string keyword)
        {
            string url = $"{ApiBaseUri}/subtitle/search/{keyword}";

            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Stream content = await response.Content.ReadAsStreamAsync();
            List<ApiSubtitle> result = await jsonSerializer.DeserializeFromStreamAsync<List<ApiSubtitle>>(content);
            return result;
        }

        public async Task<Subtitle> Download(string id)
        {
            string url = $"{ApiBaseUri}/subtitle/{id}/download";
            HttpResponseMessage response = await httpClientFactory.CreateClient().GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            Stream content = await response.Content.ReadAsStreamAsync();

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

using System.Linq;
using System.Threading;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Subtitles;
using MediaBrowser.Model.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Jellyfin.Plugin.MaxSubtitle.Tests
{
    public class MastSubtitleProviderTest
    {
        private readonly MastSubtitleProvider _provider;
        public MastSubtitleProviderTest(ITestOutputHelper output)
        {
            var serviceProvider = MastServiceUtils.BuildServiceProvider<MastSubtitleProvider>(output);
            _provider = serviceProvider.GetService<MastSubtitleProvider>();
        }

        [Fact]
        public void TestSearch()
        {
            SubtitleSearchRequest info = new SubtitleSearchRequest()
            {
                Name = "木兰",
            };

            var result = _provider.Search(info, CancellationToken.None).Result;
            string id = result.FirstOrDefault()?.Id;

            Assert.NotEmpty(result);
            Assert.True(result.Count() > 1);
            Assert.Equal("19f425f3e0d7871f465ba81a499a527e23e406f1fc0fb178a20c64d0dfd0ab51", id);
        }

        [Fact]
        public void TestDownload()
        {
            string id = "19f425f3e0d7871f465ba81a499a527e23e406f1fc0fb178a20c64d0dfd0ab51";
            var result = _provider.GetSubtitles(id, CancellationToken.None).Result;
            Assert.Equal("ass", result.Format);
            Assert.True(result.Stream.CanRead);
        }
    }
}

using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Jellyfin.Plugin.MaxSubtitle.Tests
{
    public class MastApiClientTest
    {
        private readonly MastApiClient _mastApiClient;

        public MastApiClientTest(ITestOutputHelper output)
        {
            var serviceProvider = MastServiceUtils.BuildServiceProvider<MastApiClient>(output);
            _mastApiClient = serviceProvider.GetService<MastApiClient>();
        }

        [Fact]
        public void TestSearch()
        {
            List<ApiSubtitle> list = _mastApiClient.Search("木兰").Result;
            Assert.NotEmpty(list);
        }

        [Fact]
        public void TestDownload()
        {
            string id = "19f425f3e0d7871f465ba81a499a527e23e406f1fc0fb178a20c64d0dfd0ab51";
            Subtitle s = _mastApiClient.Download(id).Result;
            Assert.Equal("ass", s.Format);
            Assert.True(s.Content.CanRead);
        }
    }
}

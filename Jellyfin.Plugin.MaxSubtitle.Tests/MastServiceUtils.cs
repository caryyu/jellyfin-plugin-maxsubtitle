using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Jellyfin.Plugin.MaxSubtitle.Tests
{
    class MastServiceUtils
    {
        public static ServiceProvider BuildServiceProvider<T>(ITestOutputHelper output) where T : class
        {
            var services = new ServiceCollection()
                .AddHttpClient()
                .AddLogging(builder => builder.AddXUnit(output).SetMinimumLevel(LogLevel.Debug))
                .AddSingleton<MastApiClient>()
                .AddSingleton<T>();

            var serviceProvider = services.BuildServiceProvider();
            var oddbApiClient = serviceProvider.GetService<MastApiClient>();
            oddbApiClient.ApiBaseUri = "http://localhost:3000";

            return serviceProvider;
        }
    }
}

using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;

namespace LiveScore.Common.Services
{
    public static class HttpClientManager
    {
        private static readonly TimeSpan timeOutTimeSpan = new TimeSpan(0, 0, 10);

        public static void SetupHttpClient(
            string url,
            IContainerProvider container,
            IContainerRegistry containerRegistry)
        {
            var serviceCollection = new ServiceCollection();
            var loggingService = container.Resolve<ILoggingService>();
            var networkConnection = container.Resolve<INetworkConnection>();

            serviceCollection
                .AddHttpClient(
                    nameof(ApiService),
                    httpClient =>
                    {
                        httpClient.Timeout = timeOutTimeSpan;
                        httpClient.BaseAddress = new Uri(url);
                    })
                .AddHttpMessageHandler(() => new CustomMessageHandler(loggingService, networkConnection, container));

            var services = serviceCollection.BuildServiceProvider();

            var httpClientFactory = services.GetService<IHttpClientFactory>();

            containerRegistry.RegisterInstance(httpClientFactory);
        }


    }
}
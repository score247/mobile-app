using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;

namespace LiveScore.Configurations
{
    public static class HttpClientRegistration
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
                .AddHttpMessageHandler(() => new RetryHandler(loggingService, networkConnection));

            var services = serviceCollection.BuildServiceProvider();

            var httpClientFactory = services.GetService<IHttpClientFactory>();

            containerRegistry.RegisterInstance(httpClientFactory);
        }
    }

    public class RetryHandler : DelegatingHandler
    {
        private const byte retryCount = 5;
        private const int retryIntervalMiliseconds = 75;
        private readonly ILoggingService loggingService;
        private readonly INetworkConnection networkConnection;

        public RetryHandler(
            ILoggingService loggingService,
            INetworkConnection networkConnection)
        {
            this.loggingService = loggingService;
            this.networkConnection = networkConnection;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await SendRequestAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                await loggingService.LogExceptionAsync(ex);

                networkConnection.PublishConnectionTimeoutEvent();

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (networkConnection.IsSuccessfulConnection())
            {
                for (int retryTime = 0; retryTime < retryCount; retryTime++)
                {
                    try
                    {
                        return await base.SendAsync(request, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        await loggingService.TrackEventAsync($"Retry Request {retryTime} times", ex.ToString());

                        await Task.Delay(TimeSpan.FromMilliseconds(retryIntervalMiliseconds));
                    }
                }
            }

            networkConnection.PublishNetworkConnectionEvent();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Prism.Ioc;
using Refit;

namespace LiveScore.Common.Services
{
    public class CustomMessageHandler : DelegatingHandler
    {
        private const byte retryCount = 3;
        private const int retryIntervalMiliseconds = 75;
        private const string Scheme = "Bearer";
        private readonly ILoggingService loggingService;
        private readonly INetworkConnection networkConnection;
        private readonly IContainerProvider container;

        public CustomMessageHandler(
            ILoggingService loggingService,
            INetworkConnection networkConnection,
            IContainerProvider container)
        {
            this.loggingService = loggingService;
            this.networkConnection = networkConnection;
            this.container = container;
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
                        return await SubmitRequestAsync(request, cancellationToken).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await HandleRequestException(retryTime, ex).ConfigureAwait(false);
                    }
                }

                networkConnection.PublishConnectionTimeoutEvent();
            }
            else
            {
                networkConnection.PublishNetworkConnectionEvent();
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private async Task HandleRequestException(int retryTime, Exception ex)
        {
            await loggingService.TrackEventAsync($"Retry Request {retryTime} times", ex.ToString()).ConfigureAwait(false);

            if (ex is ApiException apiException 
                && apiException?.StatusCode == HttpStatusCode.Unauthorized)
            {
                await GetAuthenticationToken();
            }

            await Task.Delay(TimeSpan.FromMilliseconds(retryIntervalMiliseconds));
        }

        private async Task<HttpResponseMessage> SubmitRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await SetAuthenticationToken(request);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task SetAuthenticationToken(HttpRequestMessage request)
        {
            if (!request.RequestUri.PathAndQuery.Contains("generateToken"))
            {
                var token = await GetAuthenticationToken();
                request.Headers.Authorization = new AuthenticationHeaderValue(Scheme, token);
            }
        }

        private async Task<string> GetAuthenticationToken(bool forceRenew = false)
        {
            var apiService = container.Resolve<IApiService>();
            var token = await apiService.GetToken(forceRenew).ConfigureAwait(false);
            return token;
        }
    }
}
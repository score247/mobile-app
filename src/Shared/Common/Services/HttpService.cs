using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LiveScore.Common.Services
{
    public interface IHttpService
    {
        HttpClient HttpClient { get; }
    }

    public class CustomHttpHandler : DelegatingHandler
    {
        private readonly ILoggingService loggingService;
        private readonly INetworkConnection networkConnection;

        public CustomHttpHandler(
            HttpMessageHandler innerHandler,
            ILoggingService loggingService, 
            INetworkConnection networkConnection)
            : base(innerHandler)
        {
            this.loggingService = loggingService;
            this.networkConnection = networkConnection;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                if (networkConnection.IsSuccessfulConnection())
                {
                    return await base.SendAsync(request, cancellationToken);
                }

                networkConnection.PublishNetworkConnectionEvent();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                await loggingService.LogExceptionAsync(ex);

                networkConnection.PublishConnectionTimeoutEvent();

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }

    public class HttpService : IHttpService
    {
        private const int RequestTimeoutSeconds = 10;
        private readonly HttpClient httpClient;

        public HttpService(
            Uri baseUri, 
            ILoggingService loggingService, 
            INetworkConnection networkConnection)
        {
            httpClient = new HttpClient(new CustomHttpHandler(
                new HttpClientHandler(), loggingService, networkConnection))
            {
                BaseAddress = baseUri,
                Timeout = TimeSpan.FromSeconds(RequestTimeoutSeconds)
            };
        }

        public HttpClient HttpClient
        {
            get
            {
                Debug.WriteLine(httpClient.GetHashCode());
                return httpClient;
            }
        }
    }
}
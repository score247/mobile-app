namespace LiveScore.Common.Services
{
    using System;
    using System.Net.Http;

    public interface IHttpService
    {
        HttpClient HttpClient { get; }
    }

    public class HttpService : IHttpService
    {
        private const int RequestTimeoutSeconds = 10;

        public HttpService(Uri baseUri)
        {
            HttpClient = new HttpClient
            {
                BaseAddress = baseUri,
                Timeout = TimeSpan.FromSeconds(RequestTimeoutSeconds)
            };
        }

        public HttpClient HttpClient { get; }
    }
}
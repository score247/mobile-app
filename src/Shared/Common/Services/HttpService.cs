using System;
using System.Diagnostics;
using System.Net.Http;

namespace LiveScore.Common.Services
{
    public interface IHttpService
    {
        HttpClient HttpClient { get; }
    }

    public class HttpService : IHttpService
    {
        private const int RequestTimeoutSeconds = 10;
        private readonly HttpClient httpClient;

        public HttpService(Uri baseUri)
        {
            httpClient = new HttpClient
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
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
        public HttpService(Uri baseUri)
        {   
            HttpClient = new HttpClient
            {
                BaseAddress = baseUri
            };
        }

        public HttpClient HttpClient { get; }
    }
}
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using MethodTimer;
using Refit;

namespace LiveScore.Common.Services
{
    public interface IApiService
    {
        T GetApi<T>();

        Task<T> Execute<T>(Func<Task<T>> func);
    }

    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly RefitSettings refitSettings;

        public ApiService(
            IHttpClientFactory httpClientFactory,
            RefitSettings refitSettings)
        {
            this.httpClientFactory = httpClientFactory;
            this.refitSettings = refitSettings;
        }

        // TODO: Need to make it be singleton instance
        public T GetApi<T>()
        {
            var service = RestService.For<T>(httpClientFactory.CreateClient(nameof(ApiService)), refitSettings);

            Debug.WriteLine(nameof(service) + typeof(T) + "service hashcode:" + service.GetHashCode());

            return service;
        }

        [Time]
        public Task<T> Execute<T>(Func<Task<T>> func)
            => func.Invoke();
    }
}
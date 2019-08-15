namespace LiveScore.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Common.Configuration;
    using Refit;

    public interface IApiService
    {
        T GetApi<T>();

        Task<T> Execute<T>(Func<Task<T>> func);
    }

    public class ApiService : IApiService
    {
        private readonly IApiPolicy apiPolicy;

        public ApiService(IApiPolicy apiPolicy)
        {
            this.apiPolicy = apiPolicy;
        }

        public T GetApi<T>() => RestService.For<T>(Configuration.LocalEndPoint);

        public Task<T> Execute<T>(Func<Task<T>> func) => apiPolicy.RetryAndTimeout(func);
    }
}
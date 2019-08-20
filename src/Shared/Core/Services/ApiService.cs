namespace LiveScore.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using Refit;

    public interface IApiService
    {
        T GetApi<T>();

        Task<T> Execute<T>(Func<Task<T>> func);
    }

    public class ApiService : IApiService
    {
        private readonly IApiPolicy apiPolicy;
        private readonly ISettingsService settingsService;

        public ApiService(IApiPolicy apiPolicy, ISettingsService settingsService)
        {
            this.apiPolicy = apiPolicy;
            this.settingsService = settingsService;
        }

        public T GetApi<T>() => RestService.For<T>(settingsService.ApiEndpoint);

        public Task<T> Execute<T>(Func<Task<T>> func) => apiPolicy.RetryAndTimeout(func);
    }
}
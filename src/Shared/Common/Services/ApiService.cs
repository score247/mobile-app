namespace LiveScore.Common.Services
{
    using System;
    using System.Threading.Tasks;
    using JsonNet.ContractResolvers;
    using Newtonsoft.Json;
    using Refit;

    public interface IApiService
    {
        T GetApi<T>();

        Task<T> Execute<T>(Func<Task<T>> func);
    }

    public class ApiService : IApiService
    {
        private readonly IApiPolicy apiPolicy;
        private readonly IHttpService httpService;

        private static readonly RefitSettings RefitSettings = new RefitSettings
        {
            ContentSerializer = new JsonContentSerializer(new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new PrivateSetterContractResolver()
            }),
        };

        public ApiService(IApiPolicy apiPolicy, IHttpService httpService)
        {
            this.apiPolicy = apiPolicy;
            this.httpService = httpService;
        }

        // TODO: Need to make it be singleton instance
        public T GetApi<T>() => RestService.For<T>(httpService.HttpClient, RefitSettings);

        public Task<T> Execute<T>(Func<Task<T>> func) => apiPolicy.RetryAndTimeout(func);
    }
}
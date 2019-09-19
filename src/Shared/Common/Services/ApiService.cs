using MethodTimer;

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
        private readonly RefitSettings refitSettings;

        private static readonly RefitSettings RefitSettings = new RefitSettings
        {
            ContentSerializer = new JsonContentSerializer(new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new PrivateSetterContractResolver()
            }),
        };

        public ApiService(IApiPolicy apiPolicy, IHttpService httpService, RefitSettings refitSettings = null)
        {
            this.apiPolicy = apiPolicy;
            this.httpService = httpService;
            this.refitSettings = refitSettings ?? RefitSettings;
        }

        // TODO: Need to make it be singleton instance

        public T GetApi<T>() => RestService.For<T>(httpService.HttpClient, refitSettings);

        [Time]
        public Task<T> Execute<T>(Func<Task<T>> func) => apiPolicy.RetryAndTimeout(func);
    }
}
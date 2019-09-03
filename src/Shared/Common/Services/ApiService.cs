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

        public ApiService(IApiPolicy apiPolicy, IHttpService httpService, RefitSettings refitSettings)
        {
            this.apiPolicy = apiPolicy;
            this.httpService = httpService;
            this.refitSettings = refitSettings ?? new RefitSettings
            {
                ContentSerializer = new JsonContentSerializer(new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    ContractResolver = new PrivateSetterContractResolver()
                }),
            };
        }

        public T GetApi<T>() => RestService.For<T>(httpService.HttpClient, refitSettings);

        public Task<T> Execute<T>(Func<Task<T>> func) => apiPolicy.RetryAndTimeout(func);
    }
}
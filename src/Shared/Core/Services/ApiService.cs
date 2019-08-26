namespace LiveScore.Core.Services
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
        private readonly ISettingsService settingsService;
        private readonly RefitSettings refitSettings;

        public ApiService(IApiPolicy apiPolicy, ISettingsService settingsService, RefitSettings refitSettings)
        {
            this.apiPolicy = apiPolicy;
            this.settingsService = settingsService;
            this.refitSettings = refitSettings ?? new RefitSettings
            {
                ContentSerializer = new JsonContentSerializer(new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    ContractResolver = new PrivateSetterContractResolver()
                })
            };
        }

        public T GetApi<T>() => RestService.For<T>(settingsService.ApiEndpoint, refitSettings);

        public Task<T> Execute<T>(Func<Task<T>> func) => apiPolicy.RetryAndTimeout(func);
    }
}
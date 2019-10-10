using System;
using System.Threading.Tasks;
using JsonNet.ContractResolvers;
using MethodTimer;
using Newtonsoft.Json;
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
        private readonly IHttpService httpService;
        private readonly RefitSettings refitSettings;

        private readonly INetworkConnection networkConnectionManager;

        private static readonly RefitSettings RefitSettings = new RefitSettings
        {
            ContentSerializer = new JsonContentSerializer(new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new PrivateSetterContractResolver()
            }),
        };

        public ApiService(
            IHttpService httpService,
            INetworkConnection networkConnectionManager,
            RefitSettings refitSettings = null)
        {
            this.httpService = httpService;
            this.networkConnectionManager = networkConnectionManager;
            this.refitSettings = refitSettings ?? RefitSettings;
        }

        // TODO: Need to make it be singleton instance
        public T GetApi<T>() => RestService.For<T>(httpService.HttpClient, refitSettings);

        [Time]
        public Task<T> Execute<T>(Func<Task<T>> func)
        {
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                return func.Invoke();
            }

            return Task.FromResult(default(T));
        }
    }
}
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using LiveScore.Common.Helpers;
using MethodTimer;
using Refit;

namespace LiveScore.Common.Services
{
    public interface IApiService
    {
        T GetApi<T>();

        Task<T> Execute<T>(Func<Task<T>> func);

        Task Execute(Func<Task> func);

        Task<string> GetToken(bool forceRenew = false);
    }

    public class AuthenticateInfo
    {
        public AuthenticateInfo(string userId, string encryptedInfo)
        {
            UserId = userId;
            EncryptedInfo = encryptedInfo;
        }

        public string UserId { get; }

        public string EncryptedInfo { get; }
    }

    public interface AuthenticateApi
    {
        [Post("/authenticate/generateToken")]
        Task<string> Authenticate([Body]AuthenticateInfo authenticateInfo);
    }

    public class ApiService : IApiService
    {
        private const int CacheDuration = 420;
        private const string DefaultToken = "Default_Token";
        private readonly IHttpClientFactory httpClientFactory;
        private readonly RefitSettings refitSettings;
        private readonly ICacheManager cacheManager;
        private readonly ILoggingService loggingService;
        private readonly IDeviceInfo deviceInfo;
        private readonly ICryptographyHelper cryptographyHelper;
        private readonly IConfiguration configuration;

        public ApiService(
            IHttpClientFactory httpClientFactory,
            RefitSettings refitSettings,
            ICacheManager cacheManager,
            ILoggingService loggingService,
            IDeviceInfo deviceInfo,
            ICryptographyHelper cryptographyHelper,
            IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.refitSettings = refitSettings;
            this.cacheManager = cacheManager;
            this.loggingService = loggingService;
            this.deviceInfo = deviceInfo;
            this.cryptographyHelper = cryptographyHelper;
            this.configuration = configuration;
        }

        // TODO: Need to make it be singleton instance
        public T GetApi<T>()
        {
            var service = RestService.For<T>(httpClientFactory.CreateClient(nameof(ApiService)), refitSettings);

            Debug.WriteLine(nameof(service) + typeof(T) + "service hashcode:" + service.GetHashCode());

            return service;
        }

        public async Task<string> GetToken(bool forceRenew = false)
        {
            try
            {
                const string cacheKey = "ApiService.Authenticate.GetToken";

                return await cacheManager.GetOrSetAsync(
                     cacheKey,
                     ForceGetToken,
                     CacheDuration,
                     forceRenew).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await loggingService.LogExceptionAsync(ex);

                return string.Empty;
            }
        }

        private async Task<string> ForceGetToken()
        {
            try
            {
                var authenticateApi = GetApi<AuthenticateApi>();

                var authenticateInfo = new AuthenticateInfo(deviceInfo.Id, cryptographyHelper.Encrypt(deviceInfo.Id, configuration.EncryptKey));
                var token = await authenticateApi.Authenticate(authenticateInfo);

                return token;
            }
            catch (Exception ex)
            {
                await loggingService.TrackEventAsync(DefaultToken, ex.ToString());

                return DefaultToken;
            }
        }

        [Time]
        public Task<T> Execute<T>(Func<Task<T>> func)
            => func.Invoke();

        [Time]
        public Task Execute(Func<Task> func)
            => func.Invoke();
    }
}
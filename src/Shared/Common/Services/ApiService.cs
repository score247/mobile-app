﻿using System;
using System.Net.Http;
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
        private readonly ICacheManager cacheManager;
        private readonly IHttpService httpService;
        private readonly RefitSettings refitSettings;
        private readonly ILoggingService loggingService;
        private readonly INetworkConnectionManager networkConnectionManager;

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
            ILoggingService loggingService,
            INetworkConnectionManager networkConnectionManager,
            ICacheManager cacheManager,
            RefitSettings refitSettings = null)
        {
            this.cacheManager = cacheManager;
            this.httpService = httpService;
            this.loggingService = loggingService;
            this.networkConnectionManager = networkConnectionManager;
            this.refitSettings = refitSettings ?? RefitSettings;
        }

        // TODO: Need to make it be singleton instance
        public T GetApi<T>() => RestService.For<T>(httpService.HttpClient, refitSettings);

        [Time]
        public Task<T> Execute<T>(Func<Task<T>> func)
        {
            try
            {
                if (networkConnectionManager.IsConnectionOK())
                {
                    return func.Invoke();
                }
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException)
                {
                    // TODO : Fix later
                    cacheManager.InvalidateAll().GetAwaiter().GetResult();
                    networkConnectionManager.PublishNetworkConnectionEvent();
                }

                if (ex is TaskCanceledException)
                {
                    networkConnectionManager.PublishConnectionTimeoutEvent();
                }

                // TODO : Fix later
                loggingService.LogErrorAsync(ex).GetAwaiter().GetResult();
            }

            return Task.FromResult(default(T));
        }
    }
}
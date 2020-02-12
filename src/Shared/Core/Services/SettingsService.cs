using System;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using Refit;

namespace LiveScore.Core.Services
{
    public interface ISettingsApi
    {
        [Post("/soccer/{language}/settings/{userId}/notification")]
        Task<bool> UpdateNotificationStatus(string language, string userId, [Query]bool isEnable);
    }

    public interface ISettingsService
    {
        Task UpdateNotificationStatus(bool notificationStatus);

        bool GetNotificationStatus();

        Task SyncNotification();
    }

    public class SettingsService : BaseService, ISettingsService
    {
        private const string NotificationCacheKey = "Notification";
        protected readonly IUserSettingService userSettingService;
        protected readonly IUserService userService;
        protected readonly ISettingsApi settingsApi;
        protected readonly INetworkConnection networkConnection;

        protected SettingsService(
            IUserSettingService userSettingService,
            IUserService userService,
            IApiService apiService,
            ILoggingService loggingService,
            INetworkConnection networkConnection)
            : base(loggingService)
        {
            this.userSettingService = userSettingService;
            this.userService = userService;
            settingsApi = apiService.GetApi<ISettingsApi>();
            this.networkConnection = networkConnection;

            Task.Run(SyncNotification);
        }

        public bool GetNotificationStatus()
            => userSettingService.GetValueOrDefault(NotificationCacheKey, true);

        public async Task UpdateNotificationStatus(bool notificationStatus)
        {
            userSettingService.AddOrUpdateValue(NotificationCacheKey, notificationStatus);
            await SyncNotification();
        }

        public async Task SyncNotification()
        {
            var notificationStatus = userSettingService.GetValueOrDefault(NotificationCacheKey, true);

            if (networkConnection.IsSuccessfulConnection())
            {
                var userId = await userService.GetOrCreateUserIdAsync();

                await SendUpdateNotificationStatus(notificationStatus, userId);
            }

            userSettingService.AddOrUpdateValue(NotificationCacheKey, notificationStatus);
        }

        private async Task<bool> SendUpdateNotificationStatus(bool isEnable, Guid? userId)
        {
            try
            {
                return await settingsApi.UpdateNotificationStatus(Language.English.DisplayName, userId?.ToString(), isEnable);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return false;
            }
        }
    }
}
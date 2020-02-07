using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using Refit;

namespace LiveScore.Core.Services
{
    public interface ISettingApi
    {
        [Post("/soccer/{language}/settings/{userId}/notification")]
        Task<bool> UpdateNotificationStatus(string language, string userId, [Query]bool isEnable);
    }

    public interface IAccountSettingService
    {
        void UpdateNotificationStatus(bool isEnable);

        bool GetNotificationStatus();
    }

    public class AccountSettingsService : BaseService, IAccountSettingService
    {
        private const string Notification = "Notification";
        protected readonly IUserSettingService userSettingService;
        protected readonly IUserService userService;
        protected readonly ISettingApi settingApi;

        protected AccountSettingsService(
            IUserSettingService userSettingService,
            IUserService userService,
            IApiService apiService,
            ILoggingService loggingService)
            : base(loggingService)
        {
            this.userSettingService = userSettingService;
            this.userService = userService;
            settingApi = apiService.GetApi<ISettingApi>();
        }

        public bool GetNotificationStatus()
            => userSettingService.GetValueOrDefault(Notification, true);

        public async void UpdateNotificationStatus(bool isEnable)
        {
            userSettingService.AddOrUpdateValue(Notification, isEnable);
            var userId = await userService.GetOrCreateUserIdAsync();
            if (userId != null)
            {
                try
                {
                    await settingApi.UpdateNotificationStatus(Language.English.DisplayName, userId?.ToString(), isEnable);
                }
                catch (System.Exception ex)
                {
                    HandleException(ex);
                }
            }
        }
    }
}
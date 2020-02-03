using System;
using System.Threading.Tasks;
using Microsoft.AppCenter;

namespace LiveScore.Common.Services
{
    public interface IUserService
    {
        Task<Guid?> GetOrCreateUserIdAsync();

    }

    public class UserService: IUserService
    {
        private const string USER_ID_KEY = "UserId";

        private readonly ILoggingService loggingService;
        private readonly IUserSettingService userSettingService;

        public UserService(ILoggingService loggingService, IUserSettingService userSettingService)
        {
            this.loggingService = loggingService;
            this.userSettingService = userSettingService;
        }

        public async Task<Guid?> GetOrCreateUserIdAsync()
        {
            var userId = userSettingService.GetValueOrDefault(USER_ID_KEY, default(Guid));

            return userId == default
                ? (await AppCenter.GetInstallIdAsync())
                : userId;
        }
    }
}

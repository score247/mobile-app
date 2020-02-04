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
        private readonly IUserSettingService userSettingService;

        public UserService(IUserSettingService userSettingService)
        {
            this.userSettingService = userSettingService;
        }

        public async Task<Guid?> GetOrCreateUserIdAsync()
        {
            var userId = userSettingService.GetValueOrDefault(USER_ID_KEY, default(Guid));

            if (userId == default)
            {
                userId = await AppCenter.GetInstallIdAsync() ?? default;
            }
            
            userSettingService.AddOrUpdateValue(USER_ID_KEY, userId);            

            return userId;
        }
    }
}

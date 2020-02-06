namespace LiveScore.Core.Services
{
    public interface IAccountSettingService
    {
        bool UpdateNotificationStatus(bool isEnable);
    }
}
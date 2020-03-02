using System.Threading.Tasks;

namespace LiveScore.Core.Services
{
    public interface ISettingsService
    {
        Task UpdateNotificationStatus(bool notificationStatus);

        bool GetNotificationStatus();

        Task SyncNotification();
    }    
}
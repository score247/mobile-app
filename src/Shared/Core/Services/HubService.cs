using System.Threading.Tasks;

namespace LiveScore.Core.Services
{
    public interface IHubService
    {
        Task Start();

        Task ConnectWithRetry(byte retryTimes = 5);
    }
}
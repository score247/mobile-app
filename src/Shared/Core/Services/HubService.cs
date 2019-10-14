using System.Threading.Tasks;

namespace LiveScore.Core.Services
{
    public interface IHubService
    {
        Task Start();

        Task ReConnect(byte retryTimes = 5);
    }
}
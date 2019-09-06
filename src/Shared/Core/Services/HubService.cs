namespace LiveScore.Core.Services
{
    using System.Threading.Tasks;

    public interface IHubService
    {
        Task Start();

        Task Reconnect();
    }
}
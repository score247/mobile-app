namespace LiveScore.Core.Services
{
    using System.Threading.Tasks;

    public interface IHubService
    {
        byte SportId { get; }

        Task Start();

        Task Reconnect();
    }
}
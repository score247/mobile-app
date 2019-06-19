namespace LiveScore.Core.Services
{
    using LiveScore.Common.Configuration;
    using Microsoft.AspNetCore.SignalR.Client;

    public interface IHubService
    {
        HubConnection BuildMatchHubConnection();
    }

    public class HubService : IHubService
    {
        private readonly IHubConnectionBuilder hubConnectionBuilder;
        private HubConnection matchHubConnection;

        public HubService(IHubConnectionBuilder hubConnectionBuilder)
        {
            this.hubConnectionBuilder = hubConnectionBuilder;
        }

        public HubConnection BuildMatchHubConnection()
        {
            return matchHubConnection ??
                (matchHubConnection = hubConnectionBuilder.WithUrl($"{Configuration.LocalHubEndPoint}/MatchHub").Build());
        }
    }
}
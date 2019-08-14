namespace LiveScore.Core.Services
{
    using Microsoft.AspNetCore.SignalR.Client;

    public interface IHubService
    {
        HubConnection BuildMatchEventHubConnection();

        HubConnection BuildTeamStatisticHubConnection();

        HubConnection BuildOddsEventHubConnection();
    }
}
﻿// <auto-generated> Can not be tested
// </auto-generated>
namespace LiveScore.Soccer.Services
{
    using LiveScore.Common.Configuration;
    using LiveScore.Core.Services;
    using Microsoft.AspNetCore.SignalR.Client;

    public class HubService : IHubService
    {       
        private readonly IHubConnectionBuilder hubConnectionBuilder;
        private HubConnection matchHubConnection;
        private HubConnection teamHubConnection;
        private HubConnection oddsHubConnection;

        public HubService(IHubConnectionBuilder hubConnectionBuilder)
        {
            this.hubConnectionBuilder = hubConnectionBuilder;
        }

        public HubConnection BuildMatchEventHubConnection() 
            => matchHubConnection ??
                (matchHubConnection = new HubConnectionBuilder().WithUrl($"{Configuration.LocalHubEndPoint}/Soccer/MatchEventHub").Build());

        public HubConnection BuildTeamStatisticHubConnection()
            => teamHubConnection ??
                (teamHubConnection = new HubConnectionBuilder().WithUrl($"{Configuration.LocalHubEndPoint}/Soccer/TeamStatisticHub").Build());

        public HubConnection BuildOddsEventHubConnection() 
            => oddsHubConnection ??
                (oddsHubConnection = hubConnectionBuilder.WithUrl($"{Configuration.LocalHubEndPoint}/Soccer/OddsEventHub").Build());
    }
}
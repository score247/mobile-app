﻿// <auto-generated> Can not be tested
// </auto-generated>
namespace LiveScore.Soccer.Services
{
    using LiveScore.Common.Configuration;
    using LiveScore.Core.Services;
    using Microsoft.AspNetCore.SignalR.Client;

    public class HubService : IHubService
    {
        //TODO disconnect hubConnection after using?
        private readonly IHubConnectionBuilder hubConnectionBuilder;
        private HubConnection matchHubConnection;
        private HubConnection oddsHubConnection;

        public HubService(IHubConnectionBuilder hubConnectionBuilder)
        {
            this.hubConnectionBuilder = hubConnectionBuilder;
        }

        public HubConnection BuildMatchEventHubConnection()
        {
            return matchHubConnection ??
                (matchHubConnection = hubConnectionBuilder.WithUrl($"{Configuration.LocalHubEndPoint}/Soccer/MatchEventHub").Build());
        }

        public HubConnection BuildOddsEventHubConnection()
        {
            return oddsHubConnection ??
                (oddsHubConnection = hubConnectionBuilder.WithUrl($"{Configuration.LocalHubEndPoint}/Soccer/OddsEventHub").Build());
        }
    }
}
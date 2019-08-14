﻿// <auto-generated>
// </auto-generated>
namespace LiveScore.Basketball.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using Microsoft.AspNetCore.SignalR.Client;

    public class MatchService : IMatchService
    {
        public Task<IMatch> GetMatch(string matchId, string language, bool forceFetchNewData = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IMatch>> GetMatches(UserSettings settings, DateRange dateRange, bool forceFetchNewData = false)
        {
            return null;
        }

        public void SubscribeMatchEvent(HubConnection hubConnection, Action<byte, IMatchEvent> handler)
        {
            throw new NotImplementedException();
        }
    }
}
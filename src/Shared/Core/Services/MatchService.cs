namespace LiveScore.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Teams;
    using Microsoft.AspNetCore.SignalR.Client;

    public interface IMatchService
    {
        Task<IEnumerable<IMatch>> GetMatches(DateRange dateRange, Language language, bool forceFetchNewData = false);

        Task<IMatch> GetMatch(string matchId, Language language, bool forceFetchNewData = false);

        void SubscribeMatchEvent(HubConnection hubConnection, Action<byte, IMatchEvent> handler);

        void SubscribeTeamStatistic(HubConnection hubConnection, Action<byte, string, bool, ITeamStatistic> handler);
    }
}
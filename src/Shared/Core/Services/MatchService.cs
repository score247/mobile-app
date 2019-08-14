namespace LiveScore.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using Microsoft.AspNetCore.SignalR.Client;

    public interface IMatchService
    {
        Task<IEnumerable<IMatch>> GetMatches(UserSettings settings, DateRange dateRange, bool forceFetchNewData = false);

        Task<IMatch> GetMatch(string matchId, string language, bool forceFetchNewData = false);

        void SubscribeMatchEvent(HubConnection hubConnection, Action<byte, IMatchEvent> handler);
    }
}
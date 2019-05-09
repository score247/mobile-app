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

        void SubscribeMatch(HubConnection hubConnection, Action<string, IDictionary<string, MatchPayload>> handler);
    }
}
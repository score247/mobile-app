﻿namespace LiveScore.Core.Services
{
    using System.Threading.Tasks;
    using Models.Matches;

    public interface IOddsService
    {
        Task<IMatchOdds> GetOdds(string lang, string matchId, byte betTypeId, string formatType, bool forceFetchNew = false);

        Task<IMatchOddsMovement> GetOddsMovement(string lang, string matchId, byte betTypeId, string formatType, string bookmakerId, bool forceFetchNew = false);
    }
}
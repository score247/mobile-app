namespace LiveScore.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Models.Matches;

    public interface IMatchService
    {
        Task<IList<IMatch>> GetMatches(int sportId, string language, DateRange dateRange, bool forceFetchNewData = false);

        Task<IList<IMatch>> GetLiveMatches(int sportId, string language);

        Task<IList<IMatch>> GetMatchesByLeague(string leagueId, string group);
    }
}

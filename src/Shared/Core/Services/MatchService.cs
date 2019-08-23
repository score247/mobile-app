namespace LiveScore.Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;

    public interface IMatchService
    {
        Task<IEnumerable<IMatch>> GetMatches(DateRange dateRange, Language language, bool forceFetchNewData = false);

        Task<IMatchInfo> GetMatch(string matchId, Language language, bool forceFetchNewData = false);
    }
}
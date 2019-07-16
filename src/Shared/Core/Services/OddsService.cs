namespace LiveScore.Core.Services
{
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Matches;

    public interface IOddsService
    {
        Task<IMatchOdds> GetOdds(string lang, string matchId, int betTypeId, string formatType, bool forceFetchNewData = false);
    }
}

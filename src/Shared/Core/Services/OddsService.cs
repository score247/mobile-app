namespace LiveScore.Core.Services
{
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using System.Threading.Tasks;

    public interface IOddsService
    {
        Task<IMatchOdds> GetOdds(string matchId, int betTypeId, bool forceFetchNewData = false);
    }
}

namespace LiveScore.Core.Services
{
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Matches;

    public interface IOddsService
    {
        Task<IMatchOdds> GetOdds(string matchId, int betTypeId, bool forceFetchNewData = false);
    }
}

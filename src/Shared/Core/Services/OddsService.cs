namespace LiveScore.Core.Services
{
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Settings;
    using System.Threading.Tasks;

    public interface IOddsService
    {
        Task<IMatchOdds> GetOdds(UserSettings settings, string matchId, int betTypeId, bool forceFetchNewData = false);
    }
}

namespace LiveScore.Core.Services
{
    using System.Threading.Tasks;
    using LiveScore.Core.Models.Matches;

    public interface IOddsService
    {
        Task<IMatchOdds> GetOdds(string lang, string matchId, byte betTypeId, string formatType, bool forceFetchNewData = false);

        Task<IMatchOddsMovement> GetOddsMovement(string lang, string matchId, byte betTypeId, string formatType, string bookmakerId, bool forceFetchNewData = false);       
    }
}
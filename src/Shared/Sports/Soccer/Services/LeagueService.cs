namespace LiveScore.Soccer.Services
{    
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.Leagues;
    using LiveScore.Core.Services;
    using Refit;

    public interface ILeagueApi
    {
        [Get("/League/GetLeagues?sportId={sportId}&language={languageCode}")]
        Task<IEnumerable<ILeague>> GetLeagues(string sportId, string languageCode);
    }

    public class LeagueService : BaseService, ILeagueService
    {
        public LeagueService(
            ILeagueApi leagueApi,
            ILoggingService loggingService,
            IApiPolicy apiPolicy) : base(loggingService)
        {
            
        }

        public async Task<IEnumerable<ILeague>> GetLeagues() => Enumerable.Empty<ILeague>();
    }
}
namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Models.Settings;
    using LiveScore.Core.Services;
    using Refit;

    public interface ISoccerOddsApi
    {
        [Get("/Odds/Get?sportId={sportId}&matchId={matchId}&betTypeId={betTypeId}")]
        Task<MatchOdds> GetOdds(string sportId, string matchId, int betTypeId);
    }

    public class OddsService : BaseService, IOddsService
    {
        private readonly IApiService apiService;
        private readonly ILocalStorage cacheService;

        public OddsService(
            ILocalStorage cacheService,
            ILoggingService loggingService,
            IApiService apiService
            ) : base(loggingService)
        {
            this.cacheService = cacheService;
            this.apiService = apiService;
        }

        public async Task<IMatchOdds> GetOdds(UserSettings settings, string matchId, int betTypeId, bool forceFetchNewData = false)
        {
            try
            {
                var cacheExpiration = cacheService.CacheDuration(CacheDurationTerm.Long);
                var cacheKey = $"Odds-{matchId}";

                return await cacheService.GetAndFetchLatestValue(
                        cacheKey,
                        //() => GetOddsFromApi(settings.SportId, matchId, betTypeId),
                        () => GetDummyOdds(settings.SportId, matchId, betTypeId),
                        (offset) =>
                        {
                            if (forceFetchNewData)
                            {
                                return true;
                            }

                            var elapsed = DateTimeOffset.Now - offset;

                            return elapsed > cacheExpiration;
                        });
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new MatchOdds();
            }
        }

        private async Task<MatchOdds> GetOddsFromApi(string sportId, string matchId, int betTypeId)
           => await apiService.Execute
           (
               () => apiService.GetApi<ISoccerOddsApi>().GetOdds(sportId, matchId, betTypeId)
           );

        private async Task<MatchOdds> GetDummyOdds(string sportId, string matchId, int betTypeId)
           => await apiService.Execute
           (
               () => Task.FromResult(DummyData(matchId))
           );

        private MatchOdds DummyData(string matchId)
        => new MatchOdds
        {
            MatchId = matchId,
            BetTypeOddsList = new List<BetTypeOdds>
            {
                new BetTypeOdds { Id = 1, Name = "1x2", Bookmaker = new Bookmaker{ Id = "1", Name = "Bookmaker 01" }, BetOptions = DummyBetOptionOdds() },
                new BetTypeOdds { Id = 1, Name = "1x2", Bookmaker = new Bookmaker{ Id = "2", Name = "Bookmaker 02" }, BetOptions = DummyBetOptionOdds() },
                new BetTypeOdds { Id = 1, Name = "1x2", Bookmaker = new Bookmaker{ Id = "3", Name = "Bookmaker 03" }, BetOptions = DummyBetOptionOdds() }
            }
        };

        private List<BetOptionOdds> DummyBetOptionOdds()
         => new List<BetOptionOdds>
         {
             new BetOptionOdds{ Type = "home", OpeningOdds = 5.4m, LiveOdds = 5.4m, OddsTrend = OddsTrend.Neutral },
             new BetOptionOdds{ Type = "away", OpeningOdds = 5.3m, LiveOdds = 5.4m, OddsTrend = OddsTrend.Up },
             new BetOptionOdds{ Type = "draw", OpeningOdds = 5.0m, LiveOdds = 4.8m, OddsTrend = OddsTrend.Down }
         };
    }
}

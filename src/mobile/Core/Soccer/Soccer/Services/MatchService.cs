namespace Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.MatchInfo;
    using LiveScore.Core.Services;

    public class MatchService : BaseService, IMatchService
    {
        private const int CacheHours = 2;
        private const int CachedMonths = 1;
        private readonly IMatchApi soccerMatchApi;
        private readonly ISettingsService settingsService;
        private readonly ICacheService cacheService;

        public MatchService(
            IMatchApi soccerMatchApi,
            ISettingsService settingsService,
            ICacheService cacheService,
            ILoggingService loggingService) : base(loggingService)
        {
            this.settingsService = settingsService;
            this.cacheService = cacheService;
            this.soccerMatchApi = soccerMatchApi;
        }

        public async Task<IList<Match>> GetDailyMatches(DateTime date, bool forceFetchNewData = false)
        {
            var currentSportName = settingsService.CurrentSportName;
            var sportName = settingsService.SportNameMapper[currentSportName];
            var language = settingsService.LanguageMapper["en-US"];
            var eventDate = date.ToSportRadarFormat();
            var cacheExpiration = date < DateTime.Now ? DateTime.Now.AddMonths(CachedMonths) : DateTime.Now.AddHours(CacheHours);

            return await cacheService.GetAndFetchLatestValue(
                $"DailyMatches{eventDate}",
                () => GetMatchesByGroup(sportName, language, eventDate),
                forceFetchNewData,
                cacheExpiration);
        }

        public async Task<IList<Match>> GetMatchesByLeague(string leagueId, string group)
        {
            var sportType = settingsService.SportNameMapper[settingsService.CurrentSportName];
            var lang = settingsService.LanguageMapper[settingsService.CurrentLanguage];
            var matchEvents = await GetMatchEvents(group, sportType, lang, leagueId);
            var matches = matchEvents.Select(x => new Match { Event = x }).ToList();

            return matches;
        }

        private async Task<IList<Match>> GetMatchesByGroup(string sportName, string language, string eventDate)
        {
            var matches = new List<Match>();

            var tasks = settingsService.LeagueGroups.Select(async (group) =>
            {
                matches.AddRange(await GetMatches(group, sportName, language, eventDate).ConfigureAwait(false));
            });

            await Task.WhenAll(tasks);

            return matches;
        }

        private async Task<IEnumerable<Match>> GetMatches(string group, string sportName, string language, string eventDate)
        {
            var apiKeyByGroup = settingsService.ApiKeyMapper[group];

            try
            {
                var dailySchedule = await soccerMatchApi.GetDailyMatches(sportName, group, language, eventDate, apiKeyByGroup).ConfigureAwait(false);
                dailySchedule.Matches.ToList().ForEach(match => match.Event.ShortEventDate = match.Event.EventDate.ToShortDayMonth());

                return dailySchedule.Matches;
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return Enumerable.Empty<Match>();
            }
        }

        private async Task<IList<MatchEvent>> GetMatchEvents(string group, string sportName, string language, string leagueId)
        {
            var apiKeyByGroup = settingsService.ApiKeyMapper[group];
            var matches = new List<MatchEvent>();

            try
            {
                var result = await soccerMatchApi.GetMatchesByLeague(sportName, group, language, leagueId, apiKeyByGroup).ConfigureAwait(false);

                return result.SportEvents;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return matches;
        }
    }
}

namespace LiveScore.Soccer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Models.LeagueInfo;
    using LiveScore.Core.Services;
    using Refit;

    public class LeagueService : BaseService, ILeagueService
    {
        private const string UngroupedCategoryId = "sr:category:393";
        private readonly ILeagueApi leagueApi;
        private readonly ISettingsService settingsService;

        public LeagueService(
            ILeagueApi leagueApi,
            ISettingsService settingsService,
            ILoggingService loggingService) : base(loggingService)
        {
            this.settingsService = settingsService;
            this.leagueApi = leagueApi ?? RestService.For<ILeagueApi>(SettingsService.ApiEndPoint);
        }

        public async Task<IList<LeagueItem>> GetLeagues()
        {
            var leagueItems = new List<LeagueItem>();
            var leagueCategories = new List<LeagueItem>();
            var ungroupedLeagues = new List<LeagueItem>();

            var tasks = settingsService.LeagueGroups.Select(async (leagueGroup) =>
            {
                var leagues = await GetAllLeagues(leagueGroup);

                leagueCategories.AddRange(GroupLeagueByCategory(leagues, leagueGroup));
                ungroupedLeagues.AddRange(GetLeagueItems(leagues, leagueGroup));
            });

            await Task.WhenAll(tasks);

            leagueItems.AddRange(ungroupedLeagues);
            leagueItems.AddRange(leagueCategories);

            return leagueItems;
        }

        private static IList<LeagueItem> GetLeagueItems(IList<League> leagues, string group)
            => leagues
                .Where(x => x.Category.Id.Equals(UngroupedCategoryId, StringComparison.OrdinalIgnoreCase))
                .Select(x => new LeagueItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsGrouped = false,
                    GroupName = group
                })
                .ToList();

        private static IList<LeagueItem> GroupLeagueByCategory(IList<League> leagues, string group)
            => leagues
                .Where(x => !x.Category.Id.Equals(UngroupedCategoryId, StringComparison.OrdinalIgnoreCase))
                .GroupBy(x => x.Category.Id)
                .Select(g => new LeagueItem
                {
                    Id = g.FirstOrDefault()?.Category.Id,
                    Name = g.FirstOrDefault()?.Category.Name,
                    IsGrouped = true,
                    GroupName = group
                })
                .ToList();


        private async Task<IList<League>> GetAllLeagues(string group)
        {
            var leagues = new List<League>();
            var sportNameSetting = settingsService.SportNameMapper[settingsService.CurrentSportName];
            var languageSetting = settingsService.LanguageMapper[settingsService.CurrentLanguage];

            var groupLeagues = await GetLeaguesByGroup(group, sportNameSetting, languageSetting).ConfigureAwait(false);

            if (groupLeagues.Any())
            {
                leagues.AddRange(groupLeagues);
            }

            return leagues;
        }

        private async Task<IList<League>> GetLeaguesByGroup(string group, string sportName, string language)
        {
            var apiKeyByGroup = settingsService.ApiKeyMapper[group];
            var leagues = new List<League>();

            try
            {
                var leaguesResult = await leagueApi.GetLeaguesByGroup(sportName, group, language, apiKeyByGroup).ConfigureAwait(false);

                if (leaguesResult != null && leaguesResult.Leagues.Any())
                {
                    leagues = leaguesResult.Leagues.ToList();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return leagues;
        }
    }
}

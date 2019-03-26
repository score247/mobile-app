﻿namespace League.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Helpers.Logging.Logging;
    using Common.Models;
    using Common.Models.MatchInfo;
    using Common.Services;
    using League.Models;
    using Refit;

    public interface ILeagueApi
    {
        [Get("/{sport}-t3/{group}/{lang}/tournaments.json?api_key={key}")]
        Task<LeagueInfo> GetLeaguesByGroup(string sport, string group, string lang, string key);

        [Get("/{sport}-t3/{group}/{lang}/tournaments/{leagueId}/schedule.json?api_key={key}")]
        Task<LeagueSchedule> GetMatches(string sport, string group, string lang, string leagueId, string key);
    }

    public interface ILeagueService
    {
        Task<IList<LeagueItem>> GetLeaguesAsync();

        Task<IList<Match>> GetMatchesAsync(string leagueId, string group);
    }

    public class LeagueService : ILeagueService
    {
        private const string ungroupedCategoryId = "sr:category:393";
        private readonly ILeagueApi leagueApi;
        private readonly ISettingsService settingsService;

        public LeagueService(ILeagueApi leagueApi, ISettingsService settingsService)
        {
            this.settingsService = settingsService;
            this.leagueApi = leagueApi ?? RestService.For<ILeagueApi>(settingsService.ApiEndPoint);
        }

        public async Task<IList<Match>> GetMatchesAsync(string leagueId, string group)
        {
            var sportType = settingsService.SportNameMapper[settingsService.CurrentSportName];
            var lang = settingsService.LanguageMapper[settingsService.CurrentLanguage];

            var matchEvents = await GetMatchEvents(group, sportType, lang, leagueId);

            var matches = matchEvents.Select(x => new Match { Event = x }).ToList();

            return matches;
        }

        public async Task<IList<LeagueItem>> GetLeaguesAsync()
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
        {
            return leagues
                        .Where(x => x.Category.Id.Equals(ungroupedCategoryId, StringComparison.OrdinalIgnoreCase))
                        .Select(x => new LeagueItem
                        {
                            Id = x.Id,
                            Name = x.Name,
                            IsGrouped = false,
                            GroupName = group
                        })
                        .ToList();
        }

        private static IList<LeagueItem> GroupLeagueByCategory(IList<League> leagues, string group)
        {
            return leagues
                    .Where(x => !x.Category.Id.Equals(ungroupedCategoryId, StringComparison.OrdinalIgnoreCase))
                    .GroupBy(x => x.Category.Id)
                    .Select(g => new LeagueItem
                    {
                        Id = g.FirstOrDefault()?.Category.Id,
                        Name = g.FirstOrDefault()?.Category.Name,
                        IsGrouped = true,
                        GroupName = group
                    })
                    .ToList();
        }

        private async Task<IList<League>> GetAllLeagues(string group)
        {
            var leagues = new List<League>();
            var sportNameSetting = settingsService.SportNameMapper[settingsService.CurrentSportName];
            var languageSetting = settingsService.LanguageMapper[settingsService.CurrentLanguage];

            var tasks = settingsService.LeagueGroups.Select(async (leagueGroup) =>
            {
                leagues.AddRange(await GetLeaguesByGroup(leagueGroup, sportNameSetting, languageSetting).ConfigureAwait(false));
            });

            await Task.WhenAll(tasks);

            return leagues;
        }

        private async Task<IList<League>> GetLeaguesByGroup(string group, string sportName, string language)
        {
            var apiKeyByGroup = settingsService.ApiKeyMapper[group];
            var leagues = new List<League>();

            try
            {
                var leaguesResult = await leagueApi.GetLeaguesByGroup(sportName, group, language, apiKeyByGroup).ConfigureAwait(false);

                if (leaguesResult.Leagues.Any())
                {
                    leagues.AddRange(leaguesResult.Leagues);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return leagues;
        }

        private async Task<IList<MatchEvent>> GetMatchEvents(string group, string sportName, string language, string leagueId)
        {
            var apiKeyByGroup = settingsService.ApiKeyMapper[group];
            var matches = new List<MatchEvent>();

            try
            {
                var result = await leagueApi.GetMatches(sportName, group, language, leagueId, apiKeyByGroup).ConfigureAwait(false);

                return result.SportEvents;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return matches;
        }

        private static void HandleException(Exception ex)
        {
            LoggingService.LogError(ex);

            Debug.WriteLine(ex.Message);
        }
    }
}
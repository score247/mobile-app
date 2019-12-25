using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.NavigationParams;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Features.League.Views;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels.LeagueItemViewModels
{
    public class LeagueViewModel : ViewModelBase
    {
        private readonly ILeagueService leagueService;
        private readonly ILeague league;
#pragma warning disable S107 // Methods should not have too many parameters

        public LeagueViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            Func<string, string> buildFlagFunction,
            ILeague league,
            string countryCode,
            ILeagueService leagueService = null,
            bool isShowFlag = true)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(navigationService, dependencyResolver)
        {
            this.league = league;
            BuildFlagFunction = buildFlagFunction;
            LeagueId = league?.Id;
            LeagueSeasonId = league?.SeasonId;
            LeagueName = league?.Name.ToUpperInvariant();
            CountryName = league?.CountryName;
            CountryCode = countryCode;
            LeagueFlag = buildFlagFunction(countryCode);
            this.leagueService = leagueService;
            IsShowFlag = isShowFlag;
            LeagueTapped = new DelegateAsyncCommand(OnTapLeagueAsync);
        }

        public string LeagueId { get; protected set; }

        public string LeagueSeasonId { get; protected set; }

        public string CountryCode { get; protected set; }

        public string CountryName { get; }

        public string LeagueName { get; protected set; }

        public bool IsShowFlag { get; }

        public string LeagueFlag { get; protected set; }

        public DelegateAsyncCommand LeagueTapped { get; protected set; }

        protected Func<string, string> BuildFlagFunction { get; }

        private async Task OnTapLeagueAsync()
        {
            var leagueGroup = await leagueService?.GetLeagueGroupStages(LeagueId, LeagueSeasonId, CurrentLanguage);
            if (leagueGroup?.Any() == true)
            {
                await NavigateToLeagueGroupStages(leagueGroup);
            }
            else
            {
                await NavigateToLeagueDetail();
            }
        }

        private async Task NavigateToLeagueGroupStages(IEnumerable<ILeagueGroupStage> leagueGroupStages)
        {
            var parameters = new NavigationParameters
            {
                { "League", GetLeagueDetailNavigationParameter() },
                { "CountryFlag", LeagueFlag },
                { "LeagueFlag", LeagueFlag },
                { "LeagueGroupStages", leagueGroupStages }
            };

            var navigated = await NavigationService
                .NavigateAsync(nameof(LeagueGroupStagesView), parameters)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogExceptionAsync(navigated.Exception).ConfigureAwait(false);
            }
        }

        private async Task NavigateToLeagueDetail()
        {
            var parameters = new NavigationParameters
                {
                    { "League", GetLeagueDetailNavigationParameter() },
                    { "CountryFlag", LeagueFlag }
                };

            await NavigationService
                .NavigateAsync("LeagueDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);
        }

        private LeagueDetailNavigationParameter GetLeagueDetailNavigationParameter()
            => new LeagueDetailNavigationParameter(
                   league.Id,
                   league.Name,
                   league.Order,
                   league.CountryCode,
                   league.IsInternational,
                   league.RoundGroup,
                   league.SeasonId);
    }
}
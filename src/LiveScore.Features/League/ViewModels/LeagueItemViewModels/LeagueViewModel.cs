using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Extensions;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.NavigationParams;
using LiveScore.Core.ViewModels;
using LiveScore.Features.League.Views;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels.LeagueItemViewModels
{
    public class LeagueViewModel : ViewModelBase
    {
        private readonly ILeague league;

#pragma warning disable S107 // Methods should not have too many parameters

        public LeagueViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            Func<string, string> buildFlagFunction,
            ILeague league,
            string leagueName,
            string countryCode,
            bool isShowFlag = true)
            : base(navigationService, dependencyResolver)
        {
            this.league = league;
            BuildFlagFunction = buildFlagFunction;
            LeagueId = league?.Id;
            LeagueSeasonId = league?.SeasonId;
            LeagueName = leagueName?.ToUpperInvariant();
            CountryName = league?.CountryName;
            CountryCode = countryCode;
            CountryFlag = buildFlagFunction(countryCode);
            IsShowFlag = isShowFlag;
            LeagueTapped = new DelegateAsyncCommand(OnTapLeagueAsync);
        }

#pragma warning restore S107 // Methods should not have too many parameters

        public string LeagueId { get; protected set; }

        public string LeagueSeasonId { get; protected set; }

        public string CountryCode { get; protected set; }

        public string CountryName { get; }

        public string LeagueName { get; protected set; }

        public bool IsShowFlag { get; }

        public string CountryFlag { get; protected set; }

        public DelegateAsyncCommand LeagueTapped { get; protected set; }

        protected Func<string, string> BuildFlagFunction { get; }

        private async Task OnTapLeagueAsync()
        {
            if (league.HasGroups)
            {
                await NavigateToLeagueGroupStages();
            }
            else
            {
                await NavigateToLeagueDetail();
            }
        }

        private async Task NavigateToLeagueGroupStages()
        {
            var parameters = new NavigationParameters
            {
                { "League", GetLeagueDetailNavigationParameter() },
                { "CountryFlag", CountryFlag }
            };

            var navigated = await NavigationService
                .NavigateAsync(nameof(LeagueGroupStagesView), parameters);

            if (!navigated.Success)
            {
                await LoggingService.LogExceptionAsync(navigated.Exception);
            }
        }

        private async Task NavigateToLeagueDetail()
        {
            var parameters = new NavigationParameters
                {
                    { "League", GetLeagueDetailNavigationParameter() },
                    { "CountryFlag", CountryFlag }
                };

            var navigated = await NavigationService
                .NavigateAsync("LeagueDetailView" + CurrentSportId, parameters)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogExceptionAsync(navigated.Exception);
            }
        }

        private LeagueDetailNavigationParameter GetLeagueDetailNavigationParameter()
        {
            return new LeagueDetailNavigationParameter(
                   league.Id,
                   league.CombineCountryName(),
                   league.Order,
                   league.CountryCode,
                   league.IsInternational,
                   league.RoundGroup,
                   league.SeasonId,
                   true);
        }
    }
}
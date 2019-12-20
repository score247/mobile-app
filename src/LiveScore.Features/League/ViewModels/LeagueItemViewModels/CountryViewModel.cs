using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Features.League.Views;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels.LeagueItemViewModels
{
    public class CountryViewModel : LeagueViewModel
    {
        public CountryViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            LeagueCategory leagueCategory,
            IEnumerable<ILeague> leaguesOfCountry,
            Func<string, string> buildFlagFunction)
            : base(navigationService, dependencyResolver, buildFlagFunction, null, leagueCategory.CountryCode)
        {
            Country = leagueCategory;
            LeagueName = leagueCategory.Name.ToUpperInvariant();
            LeaguesOfCountry = leaguesOfCountry;
            LeagueTapped = new DelegateAsyncCommand(OnTapCountryAsync);
        }

        public IEnumerable<ILeague> LeaguesOfCountry { get; }

        public LeagueCategory Country { get; }

        private async Task OnTapCountryAsync()
        {
            var parameters = new NavigationParameters
            {
                { "Country", Country },
                { "LeaguesOfCountry", LeaguesOfCountry }
            };

            var navigated = await NavigationService
                .NavigateAsync(nameof(LeaguesOfCountryView), parameters)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogExceptionAsync(navigated.Exception).ConfigureAwait(false);
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core.Models.Leagues;
using Prism.Navigation;

namespace LiveScore.Core.ViewModels.Leagues
{
    public class LeagueItemViewModel : ViewModelBase
    {
        public LeagueItemViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            Func<string, string> buildFlagFunction,
            ILeague league,
            bool isShowFlag)
            : base(navigationService, dependencyResolver)
        {
            League = league;
            LeagueFlag = buildFlagFunction(league.CountryCode);
            IsShowFlag = isShowFlag;
            LeagueTapped = new DelegateAsyncCommand(OnTapLeagueAsync);
        }

        public ILeague League { get; }

        public bool IsShowFlag { get; }

        public string LeagueFlag { get; }
        public DelegateAsyncCommand LeagueTapped { get; protected set; }

        private async Task OnTapLeagueAsync()
        {
            var parameters = new NavigationParameters
            {
                { "League", League }
            };

            var navigated = await NavigationService
                .NavigateAsync("LeagueGroupView" + CurrentSportId, parameters)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogExceptionAsync(navigated.Exception).ConfigureAwait(false);
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.ViewModels;
using LiveScore.Features.League.Views;
using Prism.Navigation;

namespace LiveScore.Features.League.ViewModels.LeagueItemViewModels
{
    public class LeagueViewModel : ViewModelBase
    {
        public LeagueViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            Func<string, string> buildFlagFunction,
            ILeague league,
            string countryCode,
            bool isShowFlag = true)
            : base(navigationService, dependencyResolver)
        {
            BuildFlagFunction = buildFlagFunction;
            LeagueId = league?.Id;
            LeagueName = league?.Name.ToUpperInvariant();
            CountryCode = countryCode;
            LeagueFlag = buildFlagFunction(countryCode);
            IsShowFlag = isShowFlag;
            LeagueTapped = new DelegateAsyncCommand(OnTapLeagueAsync);
        }

        public string LeagueId { get; }

        public string CountryCode { get; protected set; }

        public string LeagueName { get; protected set; }

        public bool IsShowFlag { get; }

        public string LeagueFlag { get; }

        public DelegateAsyncCommand LeagueTapped { get; protected set; }

        protected Func<string, string> BuildFlagFunction { get; }

        private async Task OnTapLeagueAsync()
        {
            var parameters = new NavigationParameters
            {
                { "LeagueId", LeagueId }
            };

            var navigated = await NavigationService
                .NavigateAsync(nameof(LeagueGroupStagesView), parameters)
                .ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogExceptionAsync(navigated.Exception).ConfigureAwait(false);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Teams;
using LiveScore.Soccer.Services;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.LineUps
{
    internal class LineupsViewModel : TabItemViewModel
    {
        private readonly ISoccerMatchService soccerMatchService;
        private readonly IDeviceInfo deviceInfo;
        private readonly string matchId;
        private readonly Action<Action> beginInvokeOnMainThreadFunc;

        public LineupsViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, eventAggregator, AppResources.Lineups)
        {
            this.matchId = matchId;
            soccerMatchService = DependencyResolver.Resolve<ISoccerMatchService>();
            deviceInfo = DependencyResolver.Resolve<IDeviceInfo>();
            beginInvokeOnMainThreadFunc = DependencyResolver.Resolve<Action<Action>>(FuncNameConstants.BeginInvokeOnMainThreadFuncName);
            RefreshCommand = new DelegateAsyncCommand(
                async () => await LoadDataAsync(LoadLineUpsAsync, false));
        }

        public bool IsRefreshing { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public LineupsHeaderViewModel LineupsHeader { get; private set; }

        public List<LineupsGroupViewModel> SubstitutionAndCoachGroups { get; private set; }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadDataAsync(LoadLineUpsAsync);
        }

        public override async void OnResumeWhenNetworkOK()
        {
            await LoadDataAsync(LoadLineUpsAsync);
        }

        public override Task OnNetworkReconnectedAsync() => LoadDataAsync(LoadLineUpsAsync);

        private async Task LoadLineUpsAsync()
        {
            var matchLineups = await soccerMatchService
                .GetMatchLineups(matchId, Language.English)
                .ConfigureAwait(false);

            beginInvokeOnMainThreadFunc(() => { RenderMatchLineups(matchLineups); });
        }

        private void RenderMatchLineups(MatchLineups matchLineups)
        {
            if (string.IsNullOrWhiteSpace(matchLineups?.Id))
            {
                HasData = false;
            }
            else
            {
                HasData = true;
                LineupsHeader = new LineupsHeaderViewModel(
                                           matchLineups.PitchView,
                                           deviceInfo,
                                           matchLineups.Home?.Name,
                                           matchLineups.Home?.Formation,
                                           matchLineups.Away?.Name,
                                           matchLineups.Away?.Formation);
                SubstitutionAndCoachGroups = BuildSubstitutionAndCoachGroups(matchLineups);
            }

            IsRefreshing = false;
        }

        private static List<LineupsGroupViewModel> BuildSubstitutionAndCoachGroups(MatchLineups matchLineups)
        {
            var lineupsGroups = new List<LineupsGroupViewModel>();
            var homeTeam = matchLineups?.Home;
            var awayTeam = matchLineups?.Away;

            if (homeTeam != null && awayTeam != null)
            {
                lineupsGroups.Add(BuildSubstitutionGroup(homeTeam, awayTeam));
            }

            if (homeTeam?.Coach != null || awayTeam?.Coach != null)
            {
                lineupsGroups.Add(BuildCouchesGroup(homeTeam, awayTeam));
            }

            return lineupsGroups;
        }

        private static LineupsGroupViewModel BuildCouchesGroup(
            TeamLineups homeTeam,
            TeamLineups awayTeam)
            => new LineupsGroupViewModel(
                AppResources.Coaches,
                new List<LineupsItemViewModel>
                {
                    new LineupsItemViewModel(homeTeam?.Coach?.Name, awayTeam?.Coach?.Name)
                });

        private static LineupsGroupViewModel BuildSubstitutionGroup(
            TeamLineups homeTeam,
            TeamLineups awayTeam)
        {
            var homeSubstitutions = homeTeam.Substitutions.ToList();
            var awaySubstitutions = awayTeam.Substitutions.ToList();
            var totalHomeSubstitution = homeSubstitutions.Count;
            var totalAwaySubstitution = awaySubstitutions.Count;
            var totalSubstitution = Math.Max(totalHomeSubstitution, totalAwaySubstitution);
            var lineupsItems = new List<LineupsItemViewModel>();

            for (var index = 0; index < totalSubstitution; index++)
            {
                var homePlayer = index < totalHomeSubstitution ? homeSubstitutions.ElementAt(index) : default;
                var awayPlayer = index < totalAwaySubstitution ? awaySubstitutions.ElementAt(index) : default;

                lineupsItems.Add(new LineupsItemViewModel(
                    homePlayer?.Name,
                    awayPlayer?.Name,
                    homePlayer?.JerseyNumber,
                    awayPlayer?.JerseyNumber));
            }

            return new LineupsGroupViewModel(AppResources.SubstitutePlayers, lineupsItems);
        }
    }
}
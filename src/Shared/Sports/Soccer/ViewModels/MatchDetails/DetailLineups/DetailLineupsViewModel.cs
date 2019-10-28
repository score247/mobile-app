using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.LangResources;
using LiveScore.Core;
using LiveScore.Core.Controls.TabStrip;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Teams;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Teams;
using LiveScore.Soccer.Services;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailLineups
{
    internal class DetailLineupsViewModel : TabItemViewModel
    {
        private readonly ISoccerMatchService soccerMatchService;
        private readonly string matchId;

        public DetailLineupsViewModel(
            string matchId,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, eventAggregator, AppResources.Lineups)
        {
            this.matchId = matchId;
            soccerMatchService = DependencyResolver.Resolve<ISoccerMatchService>();
            RefreshCommand = new DelegateAsyncCommand(
                async () => await LoadDataAsync(() => LoadMatchLineupsDataAsync(true), false));
        }

        public bool IsRefreshing { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public MatchLineups MatchLineups { get; private set; }

        public List<LineupsGroupViewModel> SubstitutionAndCoachGroups { get; private set; }

        private async Task LoadMatchLineupsDataAsync(bool isRefresh = false)
        {
            MatchLineups = await soccerMatchService
                    .GetMatchLineups(matchId, Language.English, isRefresh)
                    .ConfigureAwait(false);

            SubstitutionAndCoachGroups = BuildSubstitutionAndCoachGroups();

            IsRefreshing = false;
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadMatchLineupsDataAsync();
        }

        private List<LineupsGroupViewModel> BuildSubstitutionAndCoachGroups()
        {
            var lineupsGroups = new List<LineupsGroupViewModel>();
            var homeTeam = MatchLineups?.Home;
            var awayTeam = MatchLineups?.Away;

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
        {
            var homeCoachName = string.IsNullOrWhiteSpace(homeTeam?.Coach?.Name) ? string.Empty : homeTeam.Coach.Name;
            var awayCoachName = string.IsNullOrWhiteSpace(awayTeam?.Coach?.Name) ? string.Empty : awayTeam.Coach.Name;

            return new LineupsGroupViewModel(
                AppResources.Coaches,
                new List<LineupsItemViewModel>
                {
                    new LineupsItemViewModel(homeCoachName, awayCoachName)
                });
        }

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

            for (int index = 0; index < totalSubstitution; index++)
            {
                var homePlayer = index < totalHomeSubstitution ? homeSubstitutions.ElementAt(index) : new Player();
                var awayPlayer = index < totalAwaySubstitution ? awaySubstitutions.ElementAt(index) : new Player();

                lineupsItems.Add(new LineupsItemViewModel(
                    homePlayer.Name,
                    awayPlayer.Name,
                    homePlayer.JerseyNumber,
                    awayPlayer.JerseyNumber));
            }

            return new LineupsGroupViewModel(AppResources.SubstitutePlayers, lineupsItems);
        }
    }
}
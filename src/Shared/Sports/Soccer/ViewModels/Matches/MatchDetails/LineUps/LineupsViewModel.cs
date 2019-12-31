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

namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.LineUps
{
    internal class LineupsViewModel : TabItemViewModel, IDisposable
    {
        private readonly string matchId;
        private readonly DateTimeOffset eventDate;
        private readonly ISoccerMatchService soccerMatchService;
        private readonly IDeviceInfo deviceInfo;
        private readonly Action<Action> beginInvokeOnMainThreadFunc;
        private bool disposed = false;

        public LineupsViewModel(
            string matchId,
            DateTimeOffset eventDate,
            INavigationService navigationService,
            IDependencyResolver serviceLocator,
            IEventAggregator eventAggregator,
            DataTemplate dataTemplate)
            : base(navigationService, serviceLocator, dataTemplate, eventAggregator, AppResources.Lineups)
        {
            this.matchId = matchId;
            this.eventDate = eventDate;
            soccerMatchService = DependencyResolver.Resolve<ISoccerMatchService>();
            deviceInfo = DependencyResolver.Resolve<IDeviceInfo>();
            beginInvokeOnMainThreadFunc = DependencyResolver.Resolve<Action<Action>>(FuncNameConstants.BeginInvokeOnMainThreadFuncName);
            RefreshCommand = new DelegateAsyncCommand(
                async () => await LoadDataAsync(LoadLineUpsAsync, false));
        }

        public bool IsRefreshing { get; set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public LineupsPicthViewModel LineupsPitch { get; private set; }

        public List<LineupsGroupViewModel> LineupsItemGroups { get; private set; }

        public bool HasFormation { get; protected set; }

        public bool NoFormation => !HasFormation;

        public override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadDataAsync(LoadLineUpsAsync);
        }

        public override async void OnResumeWhenNetworkOK()
        {
            await LoadDataAsync(LoadLineUpsAsync);
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        public override void Destroy()
        {
            base.Destroy();

            Dispose();
        }

        public override Task OnNetworkReconnectedAsync() => LoadDataAsync(LoadLineUpsAsync);

        public async Task LoadLineUpsAsync()
        {
            var matchLineups = await soccerMatchService
                .GetMatchLineupsAsync(matchId, Language.English, eventDate)
                .ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(matchLineups?.Id))
            {
                HasData = false;
            }
            else
            {
                HasData = true;
                RenderMatchLineups(matchLineups);
            }

            IsRefreshing = false;
        }

        private void RenderMatchLineups(MatchLineups matchLineups)
        {
            LineupsItemGroups = new List<LineupsGroupViewModel>();
            HasFormation = (matchLineups.Home?.Formation != null);

            if (HasFormation)
            {
                var otherLineupsInfo = BuildOtherLineupsInfo(matchLineups);
                beginInvokeOnMainThreadFunc(() =>
                {
                    LineupsItemGroups = otherLineupsInfo;
                });

                Task.Delay(500).ContinueWith(_ =>
                {
                    beginInvokeOnMainThreadFunc(() =>
                    {
                        LineupsPitch = new LineupsPicthViewModel(
                           matchLineups.PitchView,
                           deviceInfo,
                           matchLineups.Home?.Name,
                           matchLineups.Home?.Formation,
                           matchLineups.Away?.Name,
                           matchLineups.Away?.Formation);
                    });
                });
            }
            else
            {
                var allLineUpsPlayersGroup = new List<LineupsGroupViewModel>();
                var noFormationLineups = BuildNoFormationLineupsGroup(matchLineups);

                if (noFormationLineups != null)
                {
                    allLineUpsPlayersGroup.Add(noFormationLineups);
                }

                var otherLineupsInfo = BuildOtherLineupsInfo(matchLineups);
                allLineUpsPlayersGroup.AddRange(otherLineupsInfo);

                beginInvokeOnMainThreadFunc(() =>
                {
                    LineupsItemGroups = allLineUpsPlayersGroup;
                });
            }
        }

        private List<LineupsGroupViewModel> BuildOtherLineupsInfo(MatchLineups matchLineups)
        {
            var lineupsItemGroups = new List<LineupsGroupViewModel>();

            var substitutions = BuildSubstitutions(matchLineups);

            if (substitutions != null)
            {
                lineupsItemGroups.Add(substitutions);
            }

            var subtitutePlayers = BuildSubstitutePlayers(matchLineups);

            if (subtitutePlayers != null)
            {
                lineupsItemGroups.Add(subtitutePlayers);
            }

            var coaches = BuildCoaches(matchLineups.Home, matchLineups.Away);

            if (coaches != null)
            {
                lineupsItemGroups.Add(coaches);
            }

            return lineupsItemGroups;
        }

        private LineupsGroupViewModel BuildSubstitutions(MatchLineups matchLineups)
        {
            LineupsGroupViewModel lineupsGroups = null;
            var homeSubstitutionEvents = matchLineups?.Home?.SubstitutionEvents;
            var awaySubstitutionEvents = matchLineups?.Away?.SubstitutionEvents;

            if (homeSubstitutionEvents?.Any() == true || awaySubstitutionEvents?.Any() == true)
            {
                var totalHomeSubstitutions = homeSubstitutionEvents.Count();
                var totalAwaySubstitutions = awaySubstitutionEvents.Count();
                var totalSubstitutions = Math.Max(totalHomeSubstitutions, totalAwaySubstitutions);
                var substitutionsItems = BuildSubstitutionItems(
                        homeSubstitutionEvents,
                        awaySubstitutionEvents,
                        totalHomeSubstitutions,
                        totalAwaySubstitutions,
                        totalSubstitutions);

                lineupsGroups = new LineupsGroupViewModel(AppResources.Substitutions, substitutionsItems);
            }

            return lineupsGroups;
        }

        private List<SubstitutionViewModel> BuildSubstitutionItems(
            IEnumerable<TimelineEvent> homeSubstitutionEvents,
            IEnumerable<TimelineEvent> awaySubstitutionEvents,
            int totalHomeSubstitutions,
            int totalAwaySubstitutions,
            int totalSubstitutions)
        {
            var substitutionsItems = new List<SubstitutionViewModel>();

            for (var index = 0; index < totalSubstitutions; index++)
            {
                var homeSubstitution = index < totalHomeSubstitutions ? homeSubstitutionEvents.ElementAt(index) : default;
                var awaySubstitution = index < totalAwaySubstitutions ? awaySubstitutionEvents.ElementAt(index) : default;

                substitutionsItems.Add(new SubstitutionViewModel(
                    DependencyResolver,
                    homeSubstitution,
                    awaySubstitution));
            }

            return substitutionsItems;
        }

        private LineupsGroupViewModel BuildNoFormationLineupsGroup(MatchLineups matchLineups)
        {
            LineupsGroupViewModel lineupsGroups = null;
            var homeTeam = matchLineups?.Home;
            var awayTeam = matchLineups?.Away;

            if (homeTeam != null && awayTeam != null)
            {
                lineupsGroups = BuildLineupsPlayerGroup(homeTeam.Players, awayTeam.Players, AppResources.LineupsPlayers);
            }

            return lineupsGroups;
        }

        private LineupsGroupViewModel BuildSubstitutePlayers(MatchLineups matchLineups)
        {
            LineupsGroupViewModel lineupsGroups = null;
            var homeTeam = matchLineups?.Home;
            var awayTeam = matchLineups?.Away;

            if (homeTeam != null && awayTeam != null)
            {
                lineupsGroups = BuildLineupsPlayerGroup(homeTeam.Substitutions, awayTeam.Substitutions, AppResources.SubstitutePlayers);
            }

            return lineupsGroups;
        }

        private LineupsGroupViewModel BuildLineupsPlayerGroup(
              IEnumerable<Player> homePlayers,
              IEnumerable<Player> awayPlayers,
              string groupName)
        {
            var totalHomePlayers = homePlayers.Count();
            var totalAwayPlayers = awayPlayers.Count();
            var totalSubstitution = Math.Max(totalHomePlayers, totalAwayPlayers);
            var lineupsItems = new List<LineupsPlayerViewModel>();
            var isSubstitute = groupName == AppResources.SubstitutePlayers;

            for (var index = 0; index < totalSubstitution; index++)
            {
                var homePlayer = index < totalHomePlayers ? homePlayers.ElementAt(index) : default;
                var awayPlayer = index < totalAwayPlayers ? awayPlayers.ElementAt(index) : default;

                lineupsItems.Add(new LineupsPlayerViewModel(
                    DependencyResolver,
                    homePlayer?.Name,
                    awayPlayer?.Name,
                    homePlayer?.JerseyNumber,
                    awayPlayer?.JerseyNumber,
                    homePlayer?.EventStatistic,
                    awayPlayer?.EventStatistic,
                    isSubstitute));
            }

            return new LineupsGroupViewModel(groupName, lineupsItems);
        }

        private LineupsGroupViewModel BuildCoaches(TeamLineups homeTeam, TeamLineups awayTeam)
        {
            LineupsGroupViewModel lineupsGroups = null;

            if (homeTeam?.Coach != null || awayTeam?.Coach != null)
            {
                lineupsGroups = new LineupsGroupViewModel(
                    AppResources.Coaches,
                    new List<LineupsPlayerViewModel>
                    {
                        new LineupsPlayerViewModel(DependencyResolver, homeTeam?.Coach?.Name, awayTeam?.Coach?.Name)
                    });
            }

            return lineupsGroups;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                LineupsItemGroups = null;
                LineupsPitch = null;
            }

            disposed = true;
        }
    }
}
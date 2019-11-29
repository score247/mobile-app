using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Fixtures
{
    public class FixturesMatchesViewModel : MatchesViewModel
    {
        private const int DefaultLoadedMatchItemCount = 10;
        private readonly string currentLeagueId;
        private readonly string currentLeagueGroupName;
        private readonly ILeagueService leagueService;

        public FixturesMatchesViewModel(
            string leagueId,
            string leagueGroupName,
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {
            currentLeagueId = leagueId;
            currentLeagueGroupName = leagueGroupName;
            leagueService = DependencyResolver.Resolve<ILeagueService>(SportType.Soccer.Value.ToString());

            LoadResultMatchesCommand = new DelegateCommand(LoadResultMatches);
            LoadFixtureMatchesCommand = new DelegateCommand(LoadFixtureMatches);
        }

        public IEnumerable<IGrouping<MatchGroupViewModel, MatchViewModel>> ResultMatchItemSource { get; private set; }

        public IEnumerable<IGrouping<MatchGroupViewModel, MatchViewModel>> FixturesMatchItemSource { get; private set; }

        public DelegateCommand LoadResultMatchesCommand { get; }

        public DelegateCommand LoadFixtureMatchesCommand { get; }

        public bool ShowLoadResultsButton { get; private set; }

        public bool ShowLoadFixturesButton { get; private set; }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            SubscribeEvents();
            await Task.Run(() => LoadDataAsync(LoadMatchesAsync).ConfigureAwait(false));
        }

        protected override Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync()
            => leagueService.GetFixtures(currentLeagueId, currentLeagueGroupName, CurrentLanguage);

        protected override void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            (var resultMatches, var fixtureMatches) = GetResultsAndFixtures(matches);

            if (resultMatches?.Any() != true && fixtureMatches?.Any() != true)
            {
                HasData = false;
                return;
            }

            fixtureMatches = GenerateLoadedFixtures(fixtureMatches, ShowLoadFixturesButton || FirstLoad);

            var loadedItemCount = DefaultLoadedMatchItemCount - fixtureMatches.Count();
            resultMatches = GenerateLoadedResults(resultMatches, loadedItemCount < 0 ? 0 : loadedItemCount, ShowLoadResultsButton || FirstLoad);

            var loadedMatchItems = resultMatches
                    .Concat(fixtureMatches)
                    .Select(match => new MatchViewModel(match, matchStatusBuilder, matchMinuteBuilder, EventAggregator))
                    .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, NavigationService, CurrentSportId));

            Device.BeginInvokeOnMainThread(()
               => MatchItemsSource = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(loadedMatchItems));

            FirstLoad = false;
        }

        private void LoadResultMatches()
        {
            if (ResultMatchItemSource?.Any() == true)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentItem = MatchItemsSource.First();

                    foreach (var matchGroup in ResultMatchItemSource)
                    {
                        MatchItemsSource.Insert(0, matchGroup);
                    }

                    ScrollToCommand.Execute(currentItem);
                    ShowLoadResultsButton = false;
                    HeaderViewModel = null;
                    ResultMatchItemSource = null;
                });
            }
        }

        private void LoadFixtureMatches()
        {
            if (FixturesMatchItemSource?.Any() == true)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var matchGroup in FixturesMatchItemSource)
                    {
                        MatchItemsSource.Add(matchGroup);
                    }

                    ShowLoadFixturesButton = false;
                    FixturesMatchItemSource = null;
                });
            }
        }

        private static (IEnumerable<IMatch>, IEnumerable<IMatch>) GetResultsAndFixtures(IEnumerable<IMatch> matches)
        {
            var orderedMatches = matches.OrderBy(match => match.EventDate);
            var fixtures = orderedMatches.Where(match => match.EventStatus.IsNotStarted);
            var results = orderedMatches.Except(fixtures);

            return (results, fixtures);
        }

        private IEnumerable<IMatch> GenerateLoadedFixtures(IEnumerable<IMatch> matches, bool showLoadFixture = true)

        {
            if (showLoadFixture && matches.Count() > DefaultLoadedMatchItemCount)
            {
                var loadedFixtureMatches = matches.Take(DefaultLoadedMatchItemCount);
                FixturesMatchItemSource = matches.Skip(DefaultLoadedMatchItemCount)
                     .Select(match => new MatchViewModel(match, matchStatusBuilder, matchMinuteBuilder, EventAggregator))
                     .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, NavigationService, CurrentSportId));
                ShowLoadFixturesButton = true;

                return loadedFixtureMatches;
            }

            FixturesMatchItemSource = null;
            ShowLoadFixturesButton = false;
            return matches;
        }

        private IEnumerable<IMatch> GenerateLoadedResults(IEnumerable<IMatch> matches, int loadedItemCount, bool showLoadResult = true)
        {
            if (showLoadResult && matches.Count() > loadedItemCount)
            {
                var loadedResultMatches = matches.TakeLast(loadedItemCount);
                ResultMatchItemSource = matches.SkipLast(loadedItemCount)
                   .OrderByDescending(match => match.EventDate)
                   .Select(match => new MatchViewModel(match, matchStatusBuilder, matchMinuteBuilder, EventAggregator))
                   .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, NavigationService, CurrentSportId));
                ShowLoadResultsButton = true;
                HeaderViewModel = this;

                return loadedResultMatches;
            }

            ResultMatchItemSource = null;
            HeaderViewModel = null;
            ShowLoadResultsButton = false;
            return matches;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ImTools;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Extensions;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.NavigationParams;
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
        private readonly ILeagueService leagueService;
        private readonly LeagueDetailNavigationParameter currentLeague;
        private bool disposed;

        public FixturesMatchesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            LeagueDetailNavigationParameter league)
                : base(navigationService, dependencyResolver, eventAggregator)
        {
            currentLeague = league;
            leagueService = DependencyResolver.Resolve<ILeagueService>(SportType.Soccer.Value.ToString());

            LoadResultMatchesCommand = new DelegateCommand(LoadResultMatches);
            LoadFixtureMatchesCommand = new DelegateCommand(LoadFixtureMatches);
            EnableTapLeague = false;
        }

        public IEnumerable<IGrouping<MatchGroupViewModel, MatchViewModel>> ResultMatchItemSource { get; private set; }

        public ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>> FixturesMatchItemSource { get; private set; }

        public DelegateCommand LoadResultMatchesCommand { get; }

        public DelegateCommand LoadFixtureMatchesCommand { get; }

        public bool ShowLoadResultsButton { get; private set; }

        public bool ShowLoadFixturesButton { get; private set; }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            SubscribeEvents();

            if (FirstLoad)
            {
                await LoadDataAsync(LoadMatchesAsync);
            }
            else
            {
                await Task.Run(() => LoadDataAsync(UpdateMatchesAsync));
            }
        }

        public override async void OnResumeWhenNetworkOK()
        {
            base.OnResumeWhenNetworkOK();

            SubscribeEvents();
            await LoadDataAsync(LoadMatchesAsync);
        }

        protected override Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync()
            => leagueService.GetFixtures(currentLeague.Id, currentLeague.SeasonId, currentLeague.Name, CurrentLanguage);

        protected override void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            var (resultMatches, fixtureMatches) = GetResultsAndFixtures(matches);

            fixtureMatches = GenerateLoadedFixtures(fixtureMatches, ShowLoadFixturesButton || FirstLoad).ToList();

            var loadedItemCount = DefaultLoadedMatchItemCount - fixtureMatches.Count();
            resultMatches = GenerateLoadedResults(resultMatches, loadedItemCount < 0 ? 0 : loadedItemCount, ShowLoadResultsButton || FirstLoad);

            var loadedMatchItems = resultMatches
                    .Concat(fixtureMatches)
                    .Select(match => new MatchViewModel(match, matchStatusBuilder, matchMinuteBuilder, EventAggregator, favoriteService))
                    .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, NavigationService, CurrentSportId, EnableTapLeague));

            Device.BeginInvokeOnMainThread(()
               => MatchItemsSource = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(loadedMatchItems));

            FirstLoad = false;
        }

        protected override void UpdateMatchItems(IEnumerable<IMatch> matches)
        {
            var matchList = matches?.ToList();

            try
            {
                if ((MatchItemsSource == null || MatchItemsSource.Count == 0) && matchList?.Any() == true)
                {
                    InitializeMatchItems(matchList);
                    return;
                }

                var loadedYesterdayOrTodayMatches = matchList.Where(match => MatchItemsSource.Any(item =>
                    item.Any(matchItem
                        => matchItem?.Match != null && matchItem.Match.Id == match.Id &&
                           matchItem.Match.EventDate.IsFromYesterdayUntilNow())));

                Device.BeginInvokeOnMainThread(() =>
                    MatchItemsSource.UpdateMatchItems(
                        loadedYesterdayOrTodayMatches,
                        matchStatusBuilder,
                        matchMinuteBuilder,
                        EventAggregator,
                        buildFlagUrlFunc,
                        NavigationService,
                        CurrentSportId,
                        favoriteService));

                ReloadFixtures(matchList, loadedYesterdayOrTodayMatches);
            }
            catch (Exception ex)
            {
                LoggingService.LogException(ex);
            }
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

                    ScrollToCommand?.Execute(currentItem);
                    ClearResultButton();
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

                    ClearFixtureButton();
                });
            }
        }

        private static (IEnumerable<IMatch>, IEnumerable<IMatch>) GetResultsAndFixtures(IEnumerable<IMatch> matches)
        {
            var orderedMatches = matches.OrderBy(match => match.EventDate).ToList();
            var fixtures = orderedMatches.Where(IsFixture).ToList();
            var results = orderedMatches.Except(fixtures).ToList();

            return (results, fixtures);
        }

        private IEnumerable<IMatch> GenerateLoadedFixtures(IEnumerable<IMatch> matches, bool showLoadFixture = true)
        {
            var fixtures = matches.ToList();

            if (showLoadFixture && fixtures.Count > DefaultLoadedMatchItemCount)
            {
                var loadedFixtureMatches = fixtures.Take(DefaultLoadedMatchItemCount);
                FixturesMatchItemSource = new ObservableCollection<IGrouping<MatchGroupViewModel, MatchViewModel>>(
                    fixtures.Skip(DefaultLoadedMatchItemCount)
                     .Select(match => new MatchViewModel(match, matchStatusBuilder, matchMinuteBuilder, EventAggregator, favoriteService))
                     .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, NavigationService, CurrentSportId, EnableTapLeague)));
                ShowLoadFixturesButton = true;

                return loadedFixtureMatches;
            }

            ClearFixtureButton();
            return fixtures;
        }

        private IEnumerable<IMatch> GenerateLoadedResults(IEnumerable<IMatch> matches, int loadedItemCount, bool showLoadResult = true)
        {
            var results = matches.ToList();

            if (showLoadResult && results.Count > loadedItemCount)
            {
                var loadedResultMatches = results.TakeLast(loadedItemCount);
                ResultMatchItemSource = results.SkipLast(loadedItemCount)
                   .Select(match => new MatchViewModel(match, matchStatusBuilder, matchMinuteBuilder, EventAggregator, favoriteService))
                   .GroupBy(item => new MatchGroupViewModel(item.Match, buildFlagUrlFunc, NavigationService, CurrentSportId, EnableTapLeague))
                   .Reverse();
                ShowLoadResultsButton = true;
                HeaderViewModel = this;

                return loadedResultMatches;
            }

            ClearResultButton();
            return results;
        }

        private void ClearFixtureButton()
        {
            FixturesMatchItemSource = null;
            ShowLoadFixturesButton = false;
        }

        private void ClearResultButton()
        {
            ResultMatchItemSource = null;
            HeaderViewModel = null;
            ShowLoadResultsButton = false;
        }

        private void ReloadFixtures(IEnumerable<IMatch> matches, IEnumerable<IMatch> loadedYesterdayOrTodayMatches)
        {
            var fixtures = matches
                .Except(loadedYesterdayOrTodayMatches)
                .Where(IsFixture);

            FixturesMatchItemSource?.UpdateMatchItems(
                fixtures,
                matchStatusBuilder,
                matchMinuteBuilder,
                EventAggregator,
                buildFlagUrlFunc,
                NavigationService,
                CurrentSportId,
                favoriteService);
        }

        private static bool IsFixture(IMatch match)
            => match.EventStatus.IsLive || match.EventStatus.IsNotStarted || match.EventDate > DateTimeOffset.Now;

        protected override void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                ResultMatchItemSource = null;
                FixturesMatchItemSource = null;
            }

            disposed = true;

            base.Dispose(disposing);
        }
    }
}
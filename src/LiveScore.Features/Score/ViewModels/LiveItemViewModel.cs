using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ImTools;
using LiveScore.Core;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.ViewModels;
using LiveScore.Features.Score.Extensions;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.Features.Score.ViewModels
{
    public class LiveItemViewModel : ScoreItemViewModel
    {
        private readonly Action<int> changeLiveMatchCountAction;

        public LiveItemViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator,
            Action<int> changeLiveMatchCountAction)
            : base(DateTime.Today, navigationService, dependencyResolver, eventAggregator)
        {
            this.changeLiveMatchCountAction = changeLiveMatchCountAction;
            EventAggregator
                .GetEvent<LiveMatchPubSubEvent>()
                .Subscribe(OnReceivedLiveMatches, true);
        }

        public override void Destroy()
        {
            base.Destroy();

            EventAggregator
                .GetEvent<LiveMatchPubSubEvent>()
                .Unsubscribe(OnReceivedLiveMatches);
        }

        protected override void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            var matchItemViewModels = matches.Select(match =>
                new MatchViewModel(match, matchStatusConverter, matchMinuteConverter, EventAggregator));

            var matchItems = matchItemViewModels.GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc)).ToList();

            Device.BeginInvokeOnMainThread(()
                => MatchItemsSource = new ObservableCollection<IGrouping<GroupMatchViewModel, MatchViewModel>>(matchItems));
        }

        protected override void UpdateMatchItems(List<IMatch> matches)
        {
            changeLiveMatchCountAction.Invoke(matches.Count);

            var currentMatches = MatchItemsSource
                .SelectMany(g => g)
                .Select(vm => vm.Match)
                .ToList();
            var modifiedMatches = currentMatches.Intersect(matches).ToList();
            var removedMatchIds = currentMatches.Except(modifiedMatches).Select(m => m.Id);

            RemoveMatchesFromItemSource(removedMatchIds.ToArray());
            MatchItemsSource.UpdateMatchItems(matches, matchStatusConverter, matchMinuteConverter, EventAggregator, buildFlagUrlFunc);
        }

        protected override async Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync(bool getLatestData)
            => await MatchService
                .GetLiveMatches(CurrentLanguage, getLatestData)
                .ConfigureAwait(false);

        private void OnReceivedLiveMatches(ILiveMatchMessage message)
        {
            if (message?.SportId != CurrentSportId)
            {
                return;
            }

            RemoveMatchesFromItemSource(message.RemoveMatchIds);

            HasData = message.NewMatches.Any() || MatchItemsSource.Any();

            if (HasData)
            {
                MatchItemsSource.UpdateMatchItems(
                    message.NewMatches, matchStatusConverter, matchMinuteConverter, EventAggregator, buildFlagUrlFunc);
            }
        }

        private void RemoveMatchesFromItemSource(string[] removedMatchIds)
        {
            foreach (var removedMatchId in removedMatchIds)
            {
                var league = MatchItemsSource
                        .FirstOrDefault(l => l.Any(match => match.Match.Id == removedMatchId));

                if (league == null)
                {
                    continue;
                }

                var leagueMatches = league.ToList();
                leagueMatches.RemoveAll(m => m.Match.Id == removedMatchId);

                if (leagueMatches.Count == 0)
                {
                    MatchItemsSource.Remove(league);
                    continue;
                }

                var indexOfLeague = MatchItemsSource.IndexOf(league);
                MatchItemsSource[indexOfLeague] = leagueMatches
                    .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                    .FirstOrDefault();
            }
        }
    }
}
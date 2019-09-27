using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.ViewModels;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Score.ViewModels
{
    public class LiveItemViewModel : ScoreItemViewModel
    {
        public LiveItemViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(DateTime.Today, navigationService, dependencyResolver, eventAggregator)
        {
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

        protected override async Task<IEnumerable<IMatch>> LoadMatchesFromServiceAsync(DateTime date, bool getLatestData)
            => await MatchService
                .GetLiveMatches(CurrentLanguage, getLatestData)
                .ConfigureAwait(false);

        protected override void UpdateMatchItemSource(IEnumerable<IMatch> matches)
        {
            var currentMatches = MatchItemsSource
                .SelectMany(g => g)
                .Select(vm => vm.Match).ToList();
            var newMatches = matches.ToList();

            if (newMatches.Count < currentMatches.Count)
            {
                var removedMatchIds = currentMatches.Except(newMatches).Select(m => m.Id);

                RemoveMatchesFromItemSource(removedMatchIds.ToArray());
            }

            base.UpdateMatchItemSource(newMatches);
        }

        private void OnReceivedLiveMatches(ILiveMatchMessage message)
        {
            if (message?.SportId != CurrentSportId)
            {
                return;
            }

            RemoveMatchesFromItemSource(message.RemoveMatchIds);

            foreach (var newMatch in message.NewMatches)
            {
                AddNewMatchToItemSource(newMatch);
            }
        }

        private void RemoveMatchesFromItemSource(string[] removedMatchIds)
        {
            for (var index = 0; index < MatchItemsSource.Count; index++)
            {
                var league = MatchItemsSource[index];
                var leagueMatches = league.ToList();
                leagueMatches.RemoveAll(m => removedMatchIds.Contains(m.Match.Id));

                if (leagueMatches.Count == 0)
                {
                    MatchItemsSource.RemoveAt(index);
                    continue;
                }

                MatchItemsSource[index] = leagueMatches
                    .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                    .FirstOrDefault();
            }
        }
    }
}
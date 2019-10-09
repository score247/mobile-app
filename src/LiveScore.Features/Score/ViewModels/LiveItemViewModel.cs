using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.ViewModels;
using LiveScore.Features.Score.Extensions;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

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
            var currentMatches = MatchItemsSource
                .SelectMany(g => g)
                .Select(vm => vm.Match)
                .ToList();
            var modifiedMatches = currentMatches.Intersect(matches).ToList();
            var removedMatchIds = currentMatches.Except(modifiedMatches).Select(m => m.Id);

            Device.BeginInvokeOnMainThread(() =>
            {
                MatchItemsSource.RemoveMatches(removedMatchIds.ToArray(), buildFlagUrlFunc);

                MatchItemsSource.UpdateMatchItems(
                    matches, matchStatusConverter, matchMinuteConverter, EventAggregator, buildFlagUrlFunc);
            });
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

            MatchItemsSource.RemoveMatches(message.RemoveMatchIds, buildFlagUrlFunc);

            HasData = message.NewMatches.Any() || MatchItemsSource.Any();

            if (HasData)
            {
                Device.BeginInvokeOnMainThread(() => MatchItemsSource.UpdateMatchItems(
                        message.NewMatches, matchStatusConverter, matchMinuteConverter, EventAggregator, buildFlagUrlFunc));
            }
        }
    }
}
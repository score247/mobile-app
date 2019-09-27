using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Models.Matches;
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

                for (var index = 0; index < MatchItemsSource.Count; index++)
                {
                    var group = MatchItemsSource[index];
                    var groupMatches = group.ToList();
                    groupMatches.RemoveAll(m => removedMatchIds.Contains(m.Match.Id));

                    if (groupMatches.Count == 0)
                    {
                        MatchItemsSource.RemoveAt(index);
                        continue;
                    }

                    MatchItemsSource[index] = groupMatches
                        .GroupBy(item => new GroupMatchViewModel(item.Match, buildFlagUrlFunc))
                        .FirstOrDefault();
                }
            }

            base.UpdateMatchItemSource(newMatches);
        }
    }
}
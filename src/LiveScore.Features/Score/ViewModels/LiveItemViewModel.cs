using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Models.Matches;
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
    }
}
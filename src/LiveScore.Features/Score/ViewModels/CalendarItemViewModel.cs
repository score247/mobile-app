using System;
using LiveScore.Core;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Score.ViewModels
{
    public class CalendarItemViewModel : ScoreItemViewModel
    {
        public CalendarItemViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(DateTime.Today, navigationService, dependencyResolver, eventAggregator)
        {
            IsComingSoon = true;
            IsLoadingSkeleton = false;
        }
    }
}
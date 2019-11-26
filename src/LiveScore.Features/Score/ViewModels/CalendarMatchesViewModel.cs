using System;
using LiveScore.Core;
using Prism.Events;
using Prism.Navigation;

namespace LiveScore.Features.Score.ViewModels
{
    public class CalendarMatchesViewModel : ScoreMatchesViewModel
    {
        public CalendarMatchesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(DateTime.Today, navigationService, dependencyResolver, eventAggregator)
        {
            IsComingSoon = true;
        }
    }
}
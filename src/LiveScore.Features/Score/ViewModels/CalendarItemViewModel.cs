using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Core;
using LiveScore.Core.Models.Matches;
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
        }
    }
}
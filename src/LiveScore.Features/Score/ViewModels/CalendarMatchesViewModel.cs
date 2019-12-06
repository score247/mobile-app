using System;
using System.Collections.Generic;
using LiveScore.Core;
using LiveScore.Core.Models.Matches;
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
        }

        protected override void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            HasData = true;
            IsComingSoon = true;
        }
    }
}
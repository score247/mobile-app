using System;
using LiveScore.Core;
using Prism.Commands;
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
            TapCalendarCommand = new DelegateCommand(OnTapCalendar);
            IsDateNotSelected = true;
            VisibleCalendar = true;
        }

        public DelegateCommand TapCalendarCommand { get; protected set; }

        public bool VisibleCalendar { get; protected set; }

        public override void OnAppearing()
        {
            if (!FirstLoad)
            {
                base.OnAppearing();
            }
            else
            {
                IsBusy = false;
            }
        }

        private void OnTapCalendar()
        {
            if (IsActive)
            {
                VisibleCalendar = !VisibleCalendar;
            }
        }
    }
}
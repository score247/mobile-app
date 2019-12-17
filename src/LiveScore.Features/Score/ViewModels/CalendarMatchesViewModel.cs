using System;
using System.Threading.Tasks;
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
        }

        public DelegateCommand TapCalendarCommand { get; protected set; }

        public bool VisibleCalendar { get; protected set; }

        public override async void OnAppearing()
        {
            if (!FirstLoad)
            {
                base.OnAppearing();
            }
            else
            {
                IsBusy = false;
                VisibleCalendar = true;
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
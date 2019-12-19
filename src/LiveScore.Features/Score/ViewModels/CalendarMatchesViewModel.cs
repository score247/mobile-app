using System;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Controls.Calendar;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;

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
            IsBusy = false;
            TapCalendarCommand = new DelegateCommand(OnTapCalendar);
            CalendarDateSelectedCommand = new DelegateAsyncCommand<CalendarDate>(OnCalendarDateSelected);
            SwipedUpCommand = new DelegateCommand(OnSwipedUp);
        }

        public DelegateCommand TapCalendarCommand { get; protected set; }

        public DelegateAsyncCommand<CalendarDate> CalendarDateSelectedCommand { get; }

        public DelegateCommand SwipedUpCommand { get; }

        public bool VisibleCalendar { get; protected set; }

        public override async void OnAppearing()
        {
            VisibleCalendar = true;

            await Task.Delay(250).ContinueWith(_ => base.OnAppearing());
        }

        private void OnTapCalendar()
        {
            if (IsActive)
            {
                VisibleCalendar = !VisibleCalendar;
            }
        }

        private void OnSwipedUp()
        {
            VisibleCalendar = false;
        }

        private async Task OnCalendarDateSelected(CalendarDate calendarDate)
        {
            MatchItemsSource?.Clear();
            HasData = true;
            VisibleCalendar = false;
            ViewDate = calendarDate.Date;

            await Task.Delay(250).ContinueWith(async _ => await LoadDataAsync(LoadMatchesAsync));
        }
    }
}
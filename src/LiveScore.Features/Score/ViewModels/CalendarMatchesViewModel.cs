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
            : base(DateTime.MinValue, navigationService, dependencyResolver, eventAggregator)
        {
            IsBusy = false;
            TapCalendarCommand = new DelegateCommand(OnTapCalendar);
            CalendarDateSelectedCommand = new DelegateAsyncCommand<CalendarDate>(OnCalendarDateSelected);
            SwipedUpCommand = new DelegateCommand(OnSwipedUp);
        }

        private void OnSwipedUp()
        {
            VisibleCalendar = false;
        }

        public DelegateCommand TapCalendarCommand { get; protected set; }

        public DelegateAsyncCommand<CalendarDate> CalendarDateSelectedCommand { get; }

        public DelegateCommand SwipedUpCommand { get; }

        public bool VisibleCalendar { get; protected set; }

        public CalendarDate SelectedDate { get; private set; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            VisibleCalendar = true;
        }

        private void OnTapCalendar()
        {
            if (IsActive)
            {
                VisibleCalendar = !VisibleCalendar;
            }
        }

        private async Task OnCalendarDateSelected(CalendarDate calendarDate)
        {
            MatchItemsSource?.Clear();
            HasData = true;
            SelectedDate = calendarDate;
            VisibleCalendar = false;
            ViewDate = calendarDate.Date;

            await Task.Delay(250).ContinueWith(async _ => await LoadDataAsync(LoadMatchesAsync));
        }
    }
}
using System;
using System.Threading.Tasks;
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
        private bool firstLoad = true;

        public CalendarMatchesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(DateTime.Today, navigationService, dependencyResolver, eventAggregator)
        {
            IsBusy = false;
            TapCalendarCommand = new DelegateCommand(OnTapCalendar);
            CalendarDateSelectedCommand = new DelegateAsyncCommand<CalendarDate>(OnCalendarDateSelected);
            HeaderViewModel = this;
            IsHeaderVisible = true;
        }

        public DelegateCommand TapCalendarCommand { get; protected set; }

        public DelegateAsyncCommand<CalendarDate> CalendarDateSelectedCommand { get; }

        public override async void OnAppearing()
        {
            if (firstLoad)
            {
                FooterHeight = 1000;
                ScrollToHeaderCommand?.Execute();
            }

            await Task.Delay(250).ContinueWith(_ => base.OnAppearing());
        }

        private void OnTapCalendar()
        {
            if (IsActive)
            {
                if (!IsHeaderVisible)
                {
                    ScrollToHeaderCommand?.Execute();
                }
                else
                {
                    ScrollToFirstItemCommand?.Execute();
                }
            }
        }

        private async Task OnCalendarDateSelected(CalendarDate calendarDate)
        {
            Device.BeginInvokeOnMainThread(() => ScrollToFirstItemCommand?.Execute());

            HasData = true;
            IsBusy = true;
            ViewDate = calendarDate.Date;
            MatchItemsSource?.Clear();

            await Task.Delay(300)
                .ContinueWith(async _ => await LoadDataAsync(LoadMatchesAsync));
        }

        protected override async Task LoadMatchesAsync()
        {
            await base.LoadMatchesAsync();

            if (!firstLoad)
            {
                AdjustFooterHeightCommand?.Execute();
            }
            else
            {
                FooterHeight = 1;
            }

            firstLoad = false;
        }
    }
}
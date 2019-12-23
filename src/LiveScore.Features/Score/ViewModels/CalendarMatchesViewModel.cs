using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Controls.Calendar;
using LiveScore.Core.Models.Matches;
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
                    if (MatchItemsSource?.Any() == true)
                    {
                        ScrollToCommand?.Execute(MatchItemsSource.FirstOrDefault());
                    }
                }
            }
        }

        private async Task OnCalendarDateSelected(CalendarDate calendarDate)
        {
            HasData = true;
            ViewDate = calendarDate.Date;
            MatchItemsSource?.Clear();

            await LoadDataAsync(LoadMatchesAsync);
        }

        protected override void InitializeMatchItems(IEnumerable<IMatch> matches)
        {
            base.InitializeMatchItems(matches);

            if (!firstLoad)
            {
                Task.Delay(500).ContinueWith(_ =>
                   Device.BeginInvokeOnMainThread(() =>
                       ScrollToCommand?.Execute(MatchItemsSource.FirstOrDefault())
                   ));
            }

            firstLoad = false;
        }
    }
}
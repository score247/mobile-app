using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Core;
using LiveScore.Core.Controls.Calendar;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;

namespace LiveScore.Features.Score.ViewModels
{
    public class CalendarMatchesViewModel : ScoreMatchesViewModel
    {
        private readonly IPopupNavigation popupNavigation;
        private readonly PopupPage calendarView;

        public CalendarMatchesViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(DateTime.Today, navigationService, dependencyResolver, eventAggregator)
        {
            popupNavigation = DependencyResolver.Resolve<IPopupNavigation>();
            calendarView = new CalendarView(105);
            TapCalendarCommand = new DelegateAsyncCommand(OnTapCalendar);
            IsDateNotSelected = true;
        }

        public DelegateAsyncCommand TapCalendarCommand { get; protected set; }

        public override async void OnAppearing()
        {
            IsBusy = false;
            await popupNavigation.PushAsync(calendarView);
        }

        private async Task OnTapCalendar()
        {
            if (IsActive)
            {
                await popupNavigation.PushAsync(calendarView);
            }
        }
    }
}
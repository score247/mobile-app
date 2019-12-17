using System;
using System.Threading.Tasks;
using LiveScore.Common.Extensions;
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
            TapCalendarCommand = new DelegateAsyncCommand(OnTapCalendar);
            IsDateNotSelected = true;
        }

        public DelegateAsyncCommand TapCalendarCommand { get; protected set; }

        private async Task OnTapCalendar()
        {
        }
    }
}
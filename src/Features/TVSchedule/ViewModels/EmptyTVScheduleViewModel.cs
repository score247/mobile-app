namespace LiveScore.TVSchedule.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class EmptyTVScheduleViewModel : ViewModelBase
    {
        public EmptyTVScheduleViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = "TV";
        }
    }
}

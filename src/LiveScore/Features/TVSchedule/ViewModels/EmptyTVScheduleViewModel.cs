namespace LiveScore.Features.TVSchedule.ViewModels
{
    using Core;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class EmptyTVScheduleViewModel : ViewModelBase
    {
        public EmptyTVScheduleViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = "TV";
        }
    }
}
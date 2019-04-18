namespace LiveScore.TVSchedule.ViewModels
{
    using LiveScore.Core.Factories;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class EmptyTVScheduleViewModel : ViewModelBase
    {
        public EmptyTVScheduleViewModel(INavigationService navigationService, IServiceLocator serviceLocator) : base(navigationService, serviceLocator)
        {
            Title = "TV";
        }
    }
}

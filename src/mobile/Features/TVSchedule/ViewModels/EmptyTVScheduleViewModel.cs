namespace LiveScore.TVSchedule.ViewModels
{
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class EmptyTVScheduleViewModel : ViewModelBase
    {
        public EmptyTVScheduleViewModel(INavigationService navigationService, IGlobalFactoryProvider globalFactory, ISettingsService settingsService) 
            : base(navigationService, globalFactory, settingsService)
        {
            Title = "TV";
        }
    }
}

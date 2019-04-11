namespace LiveScore.News.ViewModels
{
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class EmptyNewsViewModel : ViewModelBase
    {
        public EmptyNewsViewModel(INavigationService navigationService, IGlobalFactoryProvider globalFactory, ISettingsService settingsService) 
            : base(navigationService, globalFactory, settingsService)
        {
            Title = "News";
        }
    }
}

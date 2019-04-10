namespace LiveScore.News.ViewModels
{
    using Core.Factories;
    using Core.Services;
    using Core.ViewModels;
    using Prism.Navigation;

    public class NewsViewModel : ViewModelBase
    {
        public NewsViewModel(
            INavigationService navigationService,
             IGlobalFactoryProvider globalFactory,
              ISettingsService settingsService)
                 : base(navigationService, globalFactory, settingsService)
        {
            Title = "News";
        }
    }
}
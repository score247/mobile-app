namespace LiveScore.Favorites.ViewModels
{
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;

    public class EmptyFavoriteViewModel : ViewModelBase
    {
        public EmptyFavoriteViewModel(INavigationService navigationService, IGlobalFactoryProvider globalFactory, ISettingsService settingsService) 
            : base(navigationService, globalFactory, settingsService)
        {
        }
    }
}

namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using Prism.AppModel;
    using Prism.Navigation;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class ViewModelBase : INavigationAware, IDestructible, IApplicationLifecycleAware, IPageLifecycleAware
    {
        public ViewModelBase(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService)
        {
            NavigationService = navigationService;
            GlobalFactoryProvider = globalFactory;
            SettingsService = settingsService;
        }

        public string Title { get; set; }

        public INavigationService NavigationService { get; }

        public IGlobalFactoryProvider GlobalFactoryProvider { get; }

        public ISettingsService SettingsService { get; }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

        public virtual void OnResume()
        {
        }

        public virtual void OnSleep()
        {
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }
    }
}
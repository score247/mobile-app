namespace LiveScore.Core.ViewModels
{
    using LiveScore.Core.Constants;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using Prism.AppModel;
    using Prism.Mvvm;
    using Prism.Navigation;

    public class ViewModelBase : BindableBase,
            INavigationAware, IDestructible, IApplicationLifecycleAware, IPageLifecycleAware
    {
        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ViewModelBase(
            INavigationService navigationService,
            IGlobalFactoryProvider globalFactory,
            ISettingsService settingsService)
        {
            NavigationService = navigationService;
            GlobalFactoryProvider = globalFactory;
            SettingsService = settingsService;
        }

        protected INavigationService NavigationService { get; }

        protected IGlobalFactoryProvider GlobalFactoryProvider { get; }

        protected ISettingsService SettingsService { get; }

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
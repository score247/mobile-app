namespace Core.ViewModels
{
    using Core.Contants;
    using Core.Factories;
    using Core.Services;
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
            IGlobalFactory globalFactory,
            ISettingsService settingsService)
        {
            NavigationService = navigationService;
            GlobalFactory = globalFactory;
            SettingsService = settingsService;
        }

        protected INavigationService NavigationService { get; }

        protected IGlobalFactory GlobalFactory { get; }

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
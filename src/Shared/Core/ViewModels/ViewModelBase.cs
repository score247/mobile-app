namespace LiveScore.Core.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using Prism.AppModel;
    using Prism.Events;
    using Prism.Navigation;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class ViewModelBase : INavigationAware, IDestructible, IApplicationLifecycleAware, IPageLifecycleAware
    {
        public ViewModelBase(
           INavigationService navigationService,
           IServiceLocator serviceLocator,
           IEventAggregator eventAggregator) : this(navigationService, serviceLocator)
        {
            EventAggregator = eventAggregator;
        }

        public ViewModelBase(
           INavigationService navigationService,
           IServiceLocator serviceLocator)
        {
            NavigationService = navigationService;
            ServiceLocator = serviceLocator;
            SettingsService = ServiceLocator.Create<ISettingsService>();
        }

        public string Title { get; protected set; }

        public IServiceLocator ServiceLocator { get; protected set; }

        public IEventAggregator EventAggregator { get; protected set; }

        public INavigationService NavigationService { get; protected set; }

        public ISettingsService SettingsService { get; protected set; }

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

        protected async Task NavigateToHome()
        {
            Destroy();
            await NavigationService.NavigateAsync("app:///MainView/MenuTabbedView");
        }
    }
}
namespace LiveScore.Core.ViewModels
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using LiveScore.Core.Services;
    using Prism.AppModel;
    using Prism.Events;
    using Prism.Navigation;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class ViewModelBase : INavigationAware, IDestructible, IApplicationLifecycleAware, IPageLifecycleAware
    {
        public ViewModelBase()
        {
        }

        public ViewModelBase(
           INavigationService navigationService,
           IDependencyResolver serviceLocator,
           IEventAggregator eventAggregator) : this(navigationService, serviceLocator)
        {
            EventAggregator = eventAggregator;
        }

        public ViewModelBase(
           INavigationService navigationService,
           IDependencyResolver depdendencyResolver)
        {
            NavigationService = navigationService;
            DependencyResolver = depdendencyResolver;

            SettingsService = DependencyResolver.Resolve<ISettingsService>();
            LoggingService = DependencyResolver.Resolve<ILoggingService>();

            CurrentSportName = SettingsService.CurrentSportType.DisplayName;
            CurrentSportId = SettingsService.CurrentSportType.Value;
        }

        public string CurrentSportName { get; }

        public byte CurrentSportId { get; }

        public string Title { get; set; }

        public IDependencyResolver DependencyResolver { get; protected set; }

        public IEventAggregator EventAggregator { get; protected set; }

        public INavigationService NavigationService { get; protected set; }

        public ISettingsService SettingsService { get; protected set; }

        public ILoggingService LoggingService { get; protected set; }

        public bool IsLoading { get; protected set; }

        public bool IsNotLoading => !IsLoading;

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
            Initialize();
        }

        public virtual void OnSleep()
        {
            Clean();
        }

        public virtual void OnAppearing()
        {
            Initialize();
        }

        public virtual void OnDisappearing()
        {
            Clean();
        }

        protected virtual async Task LoadData(Func<Task> loadDataFunc, bool showLoading = true)
        {
            IsLoading = showLoading;

            await loadDataFunc.Invoke();

            IsLoading = false;
        }

        protected async Task NavigateToHome()
            => await NavigationService.NavigateAsync("app:///MainView/MenuTabbedView");

        protected virtual void Initialize()
        {
        }

        protected virtual void Clean()
        {
        }
    }
}
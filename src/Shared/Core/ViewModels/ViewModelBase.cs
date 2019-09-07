namespace LiveScore.Core.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Common.Services;
    using Enumerations;
    using Services;
    using Prism.AppModel;
    using Prism.Events;
    using Prism.Navigation;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class ViewModelBase : IDestructible, IApplicationLifecycleAware, IPageLifecycleAware, IInitialize
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
           IDependencyResolver dependencyResolver)
        {
            NavigationService = navigationService;
            DependencyResolver = dependencyResolver;

            Settings = DependencyResolver.Resolve<ISettings>();
            LoggingService = DependencyResolver.Resolve<ILoggingService>();

            CurrentSportName = Settings.CurrentSportType.DisplayName;
            CurrentSportId = Settings.CurrentSportType.Value;
            CurrentLanguage = Settings.CurrentLanguage;
        }

        public Language CurrentLanguage { get; }

        public string CurrentSportName { get; }

        public byte CurrentSportId { get; }

        public string Title { get; set; }

        public IDependencyResolver DependencyResolver { get; protected set; }

        public IEventAggregator EventAggregator { get; protected set; }

        public INavigationService NavigationService { get; protected set; }

        public ISettings Settings { get; protected set; }

        public ILoggingService LoggingService { get; protected set; }

        public bool IsLoading { get; protected set; }

        public bool IsNotLoading => !IsLoading;

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

        public virtual void OnResume()
        {
            OnInitialized();
        }

        public virtual void OnSleep()
        {
            OnDisposed();
        }

        public virtual void OnAppearing()
        {
            OnInitialized();
        }

        public virtual void OnDisappearing()
        {
            OnDisposed();
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void OnDisposed()
        {
        }

        protected virtual async Task LoadData(Func<Task> loadDataFunc, bool showLoading = true)
        {
            IsLoading = showLoading;

            await loadDataFunc.Invoke().ConfigureAwait(false);

            IsLoading = false;
        }

        protected async Task NavigateToHome()
            => await NavigationService.NavigateAsync("app:///MainView/MenuTabbedView").ConfigureAwait(false);
    }
}
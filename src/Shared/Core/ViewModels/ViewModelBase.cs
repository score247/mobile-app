namespace LiveScore.Core.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Enumerations;
    using LiveScore.Common.Services;
    using Prism.AppModel;
    using Prism.Events;
    using Prism.Navigation;
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
    public class ViewModelBase : MvvmHelpers.BaseViewModel, IDestructible, IApplicationLifecycleAware, IPageLifecycleAware, IInitialize
    {
        public ViewModelBase()
        {
        }

        public ViewModelBase(
           INavigationService navigationService,
           IDependencyResolver dependencyResolver,
           IEventAggregator eventAggregator) : this(navigationService, dependencyResolver)
        {
            EventAggregator = eventAggregator;
        }

        public ViewModelBase(
           INavigationService navigationService,
           IDependencyResolver dependencyResolver)
        {
            NavigationService = navigationService;
            DependencyResolver = dependencyResolver;

            LoggingService = DependencyResolver.Resolve<ILoggingService>();

            var settings = AppSettings.Current;

            CurrentSportName = settings.CurrentSportType.DisplayName;
            CurrentSportId = settings.CurrentSportType.Value;
            CurrentLanguage = settings.CurrentLanguage;
        }

        public Language CurrentLanguage { get; }

        public string CurrentSportName { get; }

        public byte CurrentSportId { get; }

        public IDependencyResolver DependencyResolver { get; protected set; }

        public IEventAggregator EventAggregator { get; protected set; }

        public INavigationService NavigationService { get; protected set; }

        public ILoggingService LoggingService { get; protected set; }

        public virtual void Initialize(INavigationParameters parameters)
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

        protected virtual async Task LoadData(Func<Task> loadDataFunc, bool showBusy = true)
        {
            IsBusy = showBusy;

            await loadDataFunc();

            IsBusy = false;
        }

        protected async Task NavigateToHome()
            => await NavigationService.NavigateAsync("app:///MainView/MenuTabbedView").ConfigureAwait(false);
    }
}
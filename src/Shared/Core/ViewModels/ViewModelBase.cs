using System;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Events;
using Prism.AppModel;
using Prism.Events;
using Prism.Navigation;
using PropertyChanged;

namespace LiveScore.Core.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ViewModelBase : MvvmHelpers.BaseViewModel, IDestructible, IApplicationLifecycleAware, IPageLifecycleAware, IInitialize
    {
        protected readonly INetworkConnectionManager networkConnectionManager;

        public ViewModelBase()
        {
        }

        public ViewModelBase(
           INavigationService navigationService,
           IDependencyResolver dependencyResolver,
           IEventAggregator eventAggregator)
            : this(navigationService, dependencyResolver)
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
            networkConnectionManager = DependencyResolver.Resolve<INetworkConnectionManager>();

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

        /// <summary>
        /// Used for not implemented page
        /// </summary>
        public bool IsComingSoon { get; protected set; }

        public bool IsNotComingSoon => !IsComingSoon;

        public bool HasData { get; protected set; } = true;

        public bool NoData => !HasData;

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

        public virtual void OnResume()
        {
            if (networkConnectionManager.IsConnectionOK())
            {
                OnResumeWhenNetworkOK();
            }
        }

        public virtual void OnResumeWhenNetworkOK()
        { }

        public virtual void OnSleep()
        {
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

        protected virtual async Task LoadDataAsync(Func<Task> loadDataFunc, bool showBusy = true)
        {
            if (networkConnectionManager.IsConnectionOK())
            {
                EventAggregator.GetEvent<StartLoadDataEvent>().Publish();
                IsBusy = showBusy;

                await loadDataFunc().ConfigureAwait(false);

                EventAggregator.GetEvent<StopLoadDataEvent>().Publish();
                IsBusy = false;
            }
            else
            {
                networkConnectionManager.PublishNetworkConnectionEvent();
            }
        }

        protected async Task NavigateToHome()
        {
            var navigated = await NavigationService.NavigateAsync("app:///MainView/MenuTabbedView").ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogErrorAsync($"Cannot navigate to home. Exception {navigated.Exception.Message}", navigated.Exception).ConfigureAwait(false);
            }
        }
    }
}
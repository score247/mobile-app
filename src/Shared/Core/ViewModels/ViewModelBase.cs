﻿using System;
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
    public class ViewModelBase :
        MvvmHelpers.BaseViewModel, IDestructible, IApplicationLifecycleAware, IPageLifecycleAware, IInitialize
    {
        protected readonly INetworkConnection networkConnectionManager;
        private readonly bool isNetworkingViewModel = true;

        public ViewModelBase()
        {
        }

        public ViewModelBase(
           INavigationService navigationService,
           IDependencyResolver dependencyResolver,
           IEventAggregator eventAggregator,
           bool isNetworkingViewModel = true)
            : this(navigationService, dependencyResolver, isNetworkingViewModel)
        {
            EventAggregator = eventAggregator;
        }

        public ViewModelBase(
           INavigationService navigationService,
           IDependencyResolver dependencyResolver,
           bool isNetworkingViewModel = true)
        {
            NavigationService = navigationService;
            DependencyResolver = dependencyResolver;

            LoggingService = DependencyResolver.Resolve<ILoggingService>();
            networkConnectionManager = DependencyResolver.Resolve<INetworkConnection>();

            var settings = DependencyResolver.Resolve<ISettings>();

            CurrentSportName = settings.CurrentSportType.DisplayName;
            CurrentSportId = settings.CurrentSportType.Value;
            CurrentLanguage = settings.CurrentLanguage;
            this.isNetworkingViewModel = isNetworkingViewModel;
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

        public bool IsLoadingSkeleton { get; protected set; }

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

        public void OnResume()
        {
            if (networkConnectionManager.IsSuccessfulConnection())
            {
                OnResumeWhenNetworkOK();
            }
        }

        public virtual void OnResumeWhenNetworkOK()
        {
        }

        public virtual void OnSleep()
        {
        }

        public virtual void OnAppearing()
        {
            if (isNetworkingViewModel)
            {
                EventAggregator?
                    .GetEvent<ConnectionChangePubSubEvent>()
                    .Subscribe(OnConnectionChangedBase, true);
            }
        }

        public virtual void OnDisappearing()
        {
            if (isNetworkingViewModel)
            {
                EventAggregator?
                    .GetEvent<ConnectionChangePubSubEvent>()
                    .Unsubscribe(OnConnectionChangedBase);
            }
        }

        protected virtual async Task LoadDataAsync(Func<Task> loadDataFunc, bool showBusy = true)
        {
            await Task.Run(() =>
            {
                networkConnectionManager
                 .OnSuccessfulConnection(async () =>
                 {
                     EventAggregator?.GetEvent<StartLoadDataEvent>()?.Publish();
                     IsBusy = showBusy;
                     IsLoadingSkeleton = IsBusy;

                     await loadDataFunc().ConfigureAwait(false);

                     EventAggregator?.GetEvent<StopLoadDataEvent>()?.Publish();
                     IsBusy = false;
                     IsLoadingSkeleton = IsBusy;
                 })
                 .OnFailedConnection(() => networkConnectionManager.PublishNetworkConnectionEvent());
            }).ConfigureAwait(false);
        }

        protected async Task NavigateToHomeAsync()
        {
            var navigated = await NavigationService.NavigateAsync("app:///MainView/MenuTabbedView").ConfigureAwait(false);

            if (!navigated.Success)
            {
                await LoggingService.LogExceptionAsync(
                    navigated.Exception,
                    $"Cannot navigate to home. Exception {navigated.Exception.Message}"
                    ).ConfigureAwait(false);
            }
        }

        public virtual Task OnNetworkReconnectedAsync() => Task.CompletedTask;

        private void OnConnectionChangedBase(bool isConnected)
        {
            if (isConnected)
            {
                OnNetworkReconnectedAsync();
            }
        }
    }
}
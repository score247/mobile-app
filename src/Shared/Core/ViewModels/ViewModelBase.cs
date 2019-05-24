﻿namespace LiveScore.Core.ViewModels
{
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
            DepdendencyResolver = depdendencyResolver;
            SettingsService = DepdendencyResolver.Resolve<ISettingsService>();
            LoggingService = DepdendencyResolver.Resolve<ILoggingService>();
        }

        public string Title { get; protected set; }

        public IDependencyResolver DepdendencyResolver { get; protected set; }

        public IEventAggregator EventAggregator { get; protected set; }

        public INavigationService NavigationService { get; protected set; }

        public ISettingsService SettingsService { get; protected set; }

        public ILoggingService LoggingService { get; protected set; }

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
            Clean();
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
            Clean();
        }

        protected async Task NavigateToHome()
            => await NavigationService.NavigateAsync("app:///MainView/MenuTabbedView");

        protected virtual void Clean()
        {
        }
    }
}
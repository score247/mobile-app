﻿using System.Threading.Tasks;
using LiveScore.Common.Extensions;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.ViewModels;
using Prism.Navigation;
using Xamarin.Forms;

namespace LiveScore.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISettings settings;
        private readonly ICacheManager cacheManager;

        public MainViewModel(
            ISettings settings,
            ICacheManager cacheManager,
            INavigationService navigationService,
            IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            this.settings = settings;
            this.cacheManager = cacheManager;

            IsDemo = settings.IsDemo;
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);
        }

        public bool IsDemo { get; set; }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        //TODO remove this command when ready to release
        public DelegateAsyncCommand ToggledCommand => new DelegateAsyncCommand(Toggle);

        public DelegateAsyncCommand CleanCacheAndRefreshCommand => new DelegateAsyncCommand(CleanCacheAndRefresh);

        private Task Navigate(string page)
        {
            return NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
        }

        private Task Toggle()
        {
            settings.IsDemo = !settings.IsDemo;

            return CleanCacheAndRefreshCommand.ExecuteAsync();
        }

        private async Task CleanCacheAndRefresh()
        {
            await cacheManager.InvalidateAll();

            await NavigateToHome();
        }
    }
}
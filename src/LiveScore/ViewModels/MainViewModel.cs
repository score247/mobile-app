﻿namespace LiveScore.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using LiveScore.Core;
    using Prism.Commands;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationService navigationService, IDepdendencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            NavigateCommand = new DelegateAsyncCommand<string>(Navigate);
        }

        public DelegateAsyncCommand<string> NavigateCommand { get; set; }

        private async Task Navigate(string page)
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + page, useModalNavigation: true);
        }
    }
}
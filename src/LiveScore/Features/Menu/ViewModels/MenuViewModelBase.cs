﻿namespace LiveScore.Features.Menu.ViewModels
{
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core.ViewModels;
    using Core;
    using Prism.Navigation;

    public class MenuViewModelBase : ViewModelBase
    {
        public MenuViewModelBase(INavigationService navigationService, IDependencyResolver serviceLocator)
             : base(navigationService, serviceLocator)
        {
            DoneCommand = new DelegateAsyncCommand(OnDone);
        }

        public DelegateAsyncCommand DoneCommand { get; }

        protected virtual async Task OnDone()
        {
            await NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}
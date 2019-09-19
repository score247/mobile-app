using System;
using LiveScore.Core;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Views;
using Prism.Navigation;

namespace LiveScore.ViewModels
{
    public class SplashScreenViewModel : ViewModelBase
    {
        private readonly IMatchService matchService;

        public SplashScreenViewModel(INavigationService navigationService, IDependencyResolver dependencyResolver)
            : base(navigationService, dependencyResolver)
        {
            matchService = dependencyResolver.Resolve<IMatchService>();
        }

        public override async void Initialize(INavigationParameters parameters)
        {
            await matchService.GetMatchesByDate(
                  DateTime.Today,
                  CurrentLanguage,
                  true).ConfigureAwait(false);

            await NavigationService.NavigateAsync(nameof(MainView) + "/" + nameof(MenuTabbedView), animated: false).ConfigureAwait(true);
        }
    }
}
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]
namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using LiveScore.Core;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;
    internal class DetailOddsViewModel : ViewModelBase
    {
        private readonly IOddsService oddsService;
        private readonly string matchId;

        public DetailOddsViewModel(
            string matchId,
            INavigationService navigationService, 
            IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            this.matchId = matchId;

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value);
        }
    }
}
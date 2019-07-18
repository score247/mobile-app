namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System;
    using LiveScore.Core;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using Prism.Events;
    using Prism.Navigation;

    public class OddsMovementViewModel : ViewModelBase, IDisposable
    {
        private readonly IOddsService oddsService;

        public OddsMovementViewModel(
            INavigationService navigationService,
            IDependencyResolver dependencyResolver,
            IEventAggregator eventAggregator)
            : base(navigationService, dependencyResolver, eventAggregator)
        {

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value);
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters["MatchId"].ToString()))
            {

                Title = "Odds movement";              
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

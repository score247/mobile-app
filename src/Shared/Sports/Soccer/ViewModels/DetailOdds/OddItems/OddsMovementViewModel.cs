namespace LiveScore.Soccer.ViewModels.DetailOdds.OddItems
{
    using System;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Enumerations;
    using Prism.Events;
    using Prism.Navigation;

    public class OddsMovementViewModel : ViewModelBase, IDisposable
    {
        private readonly IOddsService oddsService;

        private string matchId;
        private Bookmaker bookmaker;
        private BetType betType;
        private string oddsFormat;

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
            try
            {
                matchId = parameters["MatchId"].ToString();
                bookmaker = parameters["Bookmaker"] as Bookmaker;
                betType = (BetType)parameters["BetType"];
                oddsFormat = parameters["Format"].ToString();

                Title = $"{bookmaker.Name} - {AppResources.ResourceManager.GetString(betType.ToString())} Odds";
            }
            catch (Exception ex)
            {
                LoggingService.LogError(ex);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

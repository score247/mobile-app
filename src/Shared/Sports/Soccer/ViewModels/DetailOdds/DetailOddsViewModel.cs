using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]
namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Odds;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;
    using PropertyChanged;

    public enum BetType 
    { 
        OneXTwo,
        AsianHDP,
        OverUnder
    }

    internal class DetailOddsViewModel : ViewModelBase
    {
        private readonly IOddsService oddsService;
        private readonly string matchId;

        public bool IsRefreshing { get; set; }

        public bool IsLoading { get; private set; }

        public bool IsNotLoading { get; private set; }

        public ObservableCollection<BetTypeOddItemViewModel> BetTypeOdds { get; private set; }

        public DetailOddsViewModel(
            string matchId,
            INavigationService navigationService, 
            IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            this.matchId = matchId;

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value);
        }

        protected override async void Initialize()
        {
            try
            {
                BetTypeOdds = new ObservableCollection<BetTypeOddItemViewModel>();
                await LoadOdds(1, true, true);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        private async Task LoadOdds(int bettypeId, bool showLoadingIndicator = true, bool isRefresh = false)
        {
            IsLoading = showLoadingIndicator;

            var odds = await oddsService.GetOdds(SettingsService.UserSettings, matchId, bettypeId, isRefresh);

            if(odds.BetTypeOddsList.Any())
            {
                foreach(var betType in odds.BetTypeOddsList)
                {
                    BetTypeOdds.Add(new BetTypeOddItemViewModel 
                    { 
                        Bookmaker = betType.Bookmaker.Name,
                        HomeLiveOdds = betType.BetOptions.First(x => x.Type == "home").LiveOdds,
                        HomeLiveTrend = betType.BetOptions.First(x => x.Type == "home").OddsTrend,
                        HomeOpeningOdds = betType.BetOptions.First(x => x.Type == "home").OpeningOdds,
                        DrawLiveOdds = betType.BetOptions.First(x => x.Type == "draw").LiveOdds,
                        DrawLiveTrend = betType.BetOptions.First(x => x.Type == "draw").OddsTrend,
                        DrawOpeningOdds = betType.BetOptions.First(x => x.Type == "draw").OpeningOdds,
                        AwayLiveOdds = betType.BetOptions.First(x => x.Type == "away").LiveOdds,
                        AwayLiveTrend = betType.BetOptions.First(x => x.Type == "away").OddsTrend,
                        AwayOpeningOdds = betType.BetOptions.First(x => x.Type == "away").OpeningOdds,

                    });
                }
            }

            IsLoading = false;
            IsNotLoading = true;
            IsRefreshing = false;
        }
    }

    [AddINotifyPropertyChangedInterface]
    public class BetTypeOddItemViewModel 
    {
        public string Bookmaker { get; set; }

        public decimal HomeLiveOdds { get; set; }

        public OddsTrend HomeLiveTrend { get; set; }

        public decimal DrawLiveOdds { get; set; }

        public OddsTrend DrawLiveTrend { get; set; }

        public decimal AwayLiveOdds { get; set; }

        public OddsTrend AwayLiveTrend { get; set; }

        public decimal HomeOpeningOdds { get; set; }

        public decimal DrawOpeningOdds { get; set; }

        public decimal AwayOpeningOdds { get; set; }
    }
}
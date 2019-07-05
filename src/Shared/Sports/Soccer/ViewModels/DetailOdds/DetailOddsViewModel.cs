using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Soccer.Tests")]
namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Enumerations;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;
    using Prism.Navigation;

    internal class DetailOddsViewModel : ViewModelBase, IDisposable
    {
        private readonly IOddsService oddsService;
        private readonly string matchId;

        public bool IsRefreshing { get; set; }

        public bool IsLoading { get; private set; }

        public bool IsNotLoading { get; private set; }

        public bool HasData { get; private set; }
        public bool NoData { get; private set; }

        private bool disposedValue;

        public ObservableCollection<BaseItemViewModel> BetTypeOdds { get; private set; }

        public DelegateAsyncCommand RefreshCommand { get; }

        public DetailOddsViewModel(
            string matchId,
            INavigationService navigationService, 
            IDependencyResolver serviceLocator)
            : base(navigationService, serviceLocator)
        {
            //this.matchId = matchId;
            this.matchId = "sr:match:17457493";

            oddsService = DependencyResolver.Resolve<IOddsService>(SettingsService.CurrentSportType.Value);

            RefreshCommand = new DelegateAsyncCommand(async () => await LoadOdds((int)BetTypeEnum.OneXTwo, false, true));
        }

        protected override async void Initialize()
        {
            try
            {               
                await LoadOdds((int) BetTypeEnum.OneXTwo);
            }
            catch (Exception ex)
            {
                await LoggingService.LogErrorAsync(ex);
            }
        }

        private async Task LoadOdds(int bettypeId, bool showLoadingIndicator = true, bool isRefresh = false)
        {
            IsLoading = showLoadingIndicator;

            var odds = await oddsService.GetOdds(matchId, bettypeId, isRefresh);

            if(odds.BetTypeOddsList != null && odds.BetTypeOddsList.Any())
            {
                BetTypeOdds = new ObservableCollection<BaseItemViewModel>(odds.BetTypeOddsList.Select(t =>
                   new BaseItemViewModel(BetTypeEnum.OneXTwo ,t, NavigationService, DependencyResolver)
                   .CreateInstance()));

                HasData = true;
                NoData = !HasData;
            }
            else 
            {
                HasData = false;
                NoData = !HasData;
            }

            IsLoading = false;
            IsNotLoading = true;
            IsRefreshing = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                // Not use dispose method because of keeping long using object, handling object is implemented in Clean()
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
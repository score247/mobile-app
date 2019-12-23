using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Common.LangResources;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Services;
using LiveScore.Core.Views;
using Rg.Plugins.Popup.Services;

namespace LiveScore.Soccer.Services
{
    public class FavoriteService : IFavoriteService
    {
        private const string MatchKey = "Matches";
        private const string LeagueKey = "Leagues";

        private const int LeagueLimitation = 30;
        private const int MatchLimitation = 99;

        private readonly string LeagueLimitationMessage = string.Format(AppResources.FavoriteLeagueLimitation, LeagueLimitation);
        private readonly string MatchLimitationMessage = string.Format(AppResources.FavoriteMatchLimitation, MatchLimitation);

        private readonly IUserSettingService userSettingService;

        private IList<IMatch> Matches;
        private IList<FavoriteLeague> Leagues;

        public FavoriteService(IUserSettingService userSettingService)
        {
            this.userSettingService = userSettingService;

            Init();
        }       

        public void Init()
        {
            Matches = LoadMatchesFromSetting();
            Leagues = LoadLeaguesFromSetting();
        }

        public void AddLeague(FavoriteLeague league)
        {
            if (Leagues.Count() >= LeagueLimitation)
            {
                OnReachedLimitation(LeagueLimitationMessage);
            }

            if (!Leagues.Any(m => m.Id == league.Id))
            {
                Leagues.Add(league);
            }
            
            Task.Run(() => userSettingService.AddOrUpdateValue(LeagueKey, Leagues)).ConfigureAwait(false);

            OnAddedFavorite();
        }

        public void RemoveLeague(FavoriteLeague league)
        {
            var favoriteLeague = Leagues.FirstOrDefault(l => l.Id == league.Id);
            if (favoriteLeague != null)
            {
                Leagues.Remove(favoriteLeague);
            }
            
            Task.Run(() => userSettingService.AddOrUpdateValue(LeagueKey, Leagues)).ConfigureAwait(false);

            userSettingService.AddOrUpdateValue(LeagueKey, Leagues);

            OnRemovedFavorite();
        }

        public IList<FavoriteLeague> GetLeagues() => Leagues;

        public bool IsFavoriteLeague(string leagueId) => Leagues.Any(league => league.Id == leagueId);

        public void AddMatch(IMatch match)
        {
            if (!Matches.Any(m => m.Id == match.Id))
            {
                Matches.Add(match);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(MatchKey, Matches)).ConfigureAwait(false);
        }

        public void RemoveMatch(IMatch match)
        {
            if (Matches.Count() >= MatchLimitation)
            {
                OnReachedLimitation(MatchLimitationMessage);
            }

            if (Matches.Any(m => m.Id == match.Id))
            {
                Matches.Remove(match);
            }

            Task.Run(() => userSettingService.AddOrUpdateValue(MatchKey, Matches)).ConfigureAwait(false);
        }

        private IList<FavoriteLeague> LoadLeaguesFromSetting()
            => userSettingService.GetValueOrDefault(LeagueKey, Enumerable.Empty<FavoriteLeague>()).ToList();

        private IList<IMatch> LoadMatchesFromSetting()
            => userSettingService.GetValueOrDefault(MatchKey, Enumerable.Empty<IMatch>()).ToList();

        private static void OnAddedFavorite()
            => PopupNavigation.Instance.PushAsync(new FavoritePopupView(AppResources.AddedFavorite));

        private static void OnRemovedFavorite()
            => PopupNavigation.Instance.PushAsync(new FavoritePopupView(AppResources.RemovedFavorite));

        private static void OnReachedLimitation(string message)
            => PopupNavigation.Instance.PushAsync(new FavoritePopupView(message));
    }
}

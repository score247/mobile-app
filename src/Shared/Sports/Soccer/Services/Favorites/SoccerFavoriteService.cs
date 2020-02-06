using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Models.Favorites;
using LiveScore.Core.Services;
using Prism.Events;

namespace LiveScore.Soccer.Services
{
    public abstract class SoccerFavoriteService<T> : FavoriteService<T>
    {
        private readonly IApiService apiService;
        private readonly SoccerApi.FavoriteApi favoriteApi;
        private readonly IUserService userService;
        private readonly ISettings settings;

        private string userId;

        protected SoccerFavoriteService(
            IUserSettingService userSettingService,
            IEventAggregator eventAggregator,
            string key,
            int limitation,
            ILoggingService loggingService,
            IApiService apiService,
            IUserService userService,
            ISettings settings,
            INetworkConnection networkConnection,
            SoccerApi.FavoriteApi favoriteApi = null) : base(userSettingService, eventAggregator, key, limitation, loggingService, networkConnection)
        {
            this.apiService = apiService;
            this.userService = userService;
            this.settings = settings;
            this.favoriteApi = favoriteApi ?? apiService.GetApi<SoccerApi.FavoriteApi>();
            SyncFunc = SyncToServer;
        }

        private async Task<bool> SyncToServer(IList<Favorite> addedFavorites, IList<Favorite> removedFavoriteds)
        {
            try
            {
                userId = await GetUserIdAsync();
                var syncUserFavorite = new SyncUserFavorite(new UserFavorite(userId, addedFavorites), new UserFavorite(userId, removedFavoriteds));

                return await apiService.Execute(()
                    => favoriteApi.Sync(syncUserFavorite, settings.CurrentLanguage.DisplayName));
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return false;
            }
        }

        private async Task<string> GetUserIdAsync()
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                var generatedUserId = (await userService.GetOrCreateUserIdAsync()).ToString();

                if (generatedUserId == null)
                {
                    throw new ArgumentNullException("UserId", "Cannot generate InstallId from AppCenter");
                }

                return generatedUserId;
            }

            return userId;
        }
    }
}
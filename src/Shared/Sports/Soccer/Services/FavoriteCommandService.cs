using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiveScore.Common.Services;
using LiveScore.Core.Models.Favorites;
using LiveScore.Core.Services;

namespace LiveScore.Soccer.Services
{
    public class FavoriteCommandService : BaseService, IFavoriteCommandService
    {
        private readonly IApiService apiService;
        private readonly SoccerApi.FavoriteCommandApi favoriteApi;
        private readonly IUserService userService;

        private string userId;

        public FavoriteCommandService(
            IApiService apiService,
            ILoggingService loggingService,
            IUserService userService,
            SoccerApi.FavoriteCommandApi favoriteApi = null) : base(loggingService)
        {
            this.apiService = apiService;
            this.userService = userService;
            this.favoriteApi = favoriteApi ?? apiService.GetApi<SoccerApi.FavoriteCommandApi>();
        }

        public Task AddFavorite(Favorite favorite)
        => AddFavorites(new List<Favorite> { favorite });

        public async Task AddFavorites(IList<Favorite> favorites)
        {
            try
            {
                userId = await GetUserIdAsync();

                await apiService.Execute(() => favoriteApi.AddFavorites(new UserFavorite(userId, favorites)));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public async Task RemoveFavorite(string favoriteId)
        {
            try
            {
                userId = await GetUserIdAsync();

                await apiService.Execute(() => favoriteApi.RemoveFavorite(userId, favoriteId));
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private async Task<string> GetUserIdAsync()
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                var generatedUserId =  (await userService.GetOrCreateUserIdAsync()).ToString();

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

using System.Collections.Generic;
using MessagePack;
using Xamarin.Forms;

namespace LiveScore.Core.Models.Favorites
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class UserFavorite
    {
        public UserFavorite(string userId, IList<Favorite> favorites, string platform = null)
        {
            UserId = userId;
            Favorites = favorites;
            Platform = platform ?? Device.RuntimePlatform;
        }

        public string UserId { get; }

        public IList<Favorite> Favorites { get; }

        public string Platform { get; }
    }
}
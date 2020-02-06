using MessagePack;

namespace LiveScore.Core.Models.Favorites
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class SyncUserFavorite
    {
        public SyncUserFavorite(UserFavorite addedUserFavorite, UserFavorite removedUserFavorite)
        {
            AddedUserFavorite = addedUserFavorite;
            RemovedUserFavorite = removedUserFavorite;
        }

        public UserFavorite AddedUserFavorite { get; }

        public UserFavorite RemovedUserFavorite { get; }
    }
}
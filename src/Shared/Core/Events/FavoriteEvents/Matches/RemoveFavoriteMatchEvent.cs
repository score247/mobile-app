using LiveScore.Core.Models.Matches;
using Prism.Events;

namespace LiveScore.Core.Events.FavoriteEvents.Matches
{
    public class RemoveFavoriteMatchEvent : PubSubEvent<IMatch>
    {
    }
}
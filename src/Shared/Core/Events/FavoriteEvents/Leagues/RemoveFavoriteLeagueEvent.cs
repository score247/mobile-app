using LiveScore.Core.Models.Leagues;
using Prism.Events;

namespace LiveScore.Core.Events.FavoriteEvents.Leagues
{
    public class RemoveFavoriteLeagueEvent : PubSubEvent<ILeague>
    {
    }
}
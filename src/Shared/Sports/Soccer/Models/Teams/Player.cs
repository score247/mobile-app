using System.Collections.Generic;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Teams;
using MessagePack;

namespace LiveScore.Soccer.Models.Teams
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class Player : IPlayer
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public PlayerType Type { get; set; }

        public int JerseyNumber { get; set; }

        public Position Position { get; set; }

        public int Order { get; set; }

        public IDictionary<EventType, int> EventStatistic { get; set; }
    }
}
using System.Collections.Generic;
using LiveScore.Core.Models.Teams;
using LiveScore.Soccer.Models.Matches;
using MessagePack;

namespace LiveScore.Soccer.Models.Teams
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class TeamLineups
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public TeamLineups(
            string id,
            string name,
            bool isHome,
            Coach coach,
            string formation,
            IEnumerable<Player> players,
            IEnumerable<Player> substitutions,
            IEnumerable<TimelineEvent> substitutionEvents)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            Id = id;
            Name = name;
            IsHome = isHome;
            Coach = coach;
            Formation = formation;
            Players = players;
            Substitutions = substitutions;
            SubstitutionEvents = substitutionEvents;
        }

        public string Id { get; }

        public string Name { get; }

        public bool IsHome { get; private set; }

        public Coach Coach { get; }

        public string Formation { get; }

        public IEnumerable<Player> Players { get; }

        public IEnumerable<Player> Substitutions { get; }

        public IEnumerable<TimelineEvent> SubstitutionEvents { get; }
    }
}
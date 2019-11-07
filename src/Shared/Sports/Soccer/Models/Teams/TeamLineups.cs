using System.Collections.Generic;
using LiveScore.Core.Models.Teams;
using LiveScore.Soccer.Models.Matches;
using MessagePack;

namespace LiveScore.Soccer.Models.Teams
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class TeamLineups
    {
        public TeamLineups(
            string id,
            string name,
            bool isHome,
            Coach coach,
            string formation,
            IEnumerable<Player> players,
            IEnumerable<Player> substitutions)
        {
            Id = id;
            Name = name;
            IsHome = isHome;
            Coach = coach;
            Formation = formation;
            Players = players;
            Substitutions = substitutions;
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
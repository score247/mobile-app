using System;
using LiveScore.Soccer.Models.Teams;
using MessagePack;

namespace LiveScore.Soccer.Models.Matches
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class MatchLineups
    {
        public MatchLineups(string id)
        {
            Id = id;
        }

        [SerializationConstructor]
        public MatchLineups(
            string id,
            DateTimeOffset eventDate,
            TeamLineups home,
            TeamLineups away,
            string pitchView)
        {
            Id = id;
            EventDate = eventDate;
            Home = home;
            Away = away;
            PitchView = pitchView;
        }

        public string Id { get; }

        public DateTimeOffset EventDate { get; }

        public TeamLineups Home { get; }

        public TeamLineups Away { get; }

        public string PitchView { get; }
    }
}
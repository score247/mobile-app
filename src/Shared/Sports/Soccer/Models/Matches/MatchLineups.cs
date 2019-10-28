﻿using System;
using LiveScore.Soccer.Models.Teams;
using MessagePack;

namespace LiveScore.Soccer.Models.Matches
{
    [MessagePackObject]
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

        [Key(0)]
        public string Id { get; }

        [Key(1)]
        public DateTimeOffset EventDate { get; }

        [Key(2)]
        public TeamLineups Home { get; }

        [Key(3)]
        public TeamLineups Away { get; }

        [Key(4)]
        public string PitchView { get; }

        [IgnoreMember]
        public string PitchViewHtml => $"<html><body>{PitchView}</body></html>";
    }
}
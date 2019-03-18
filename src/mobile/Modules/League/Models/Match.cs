using System;

namespace League.Models
{
    public class Match
    {
        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public DateTime EventDate { get; set; }

        public string GroupName { get; set; }

        public int HomeScore { get; set; }

        public int AwayScore { get; set; }
    }
}
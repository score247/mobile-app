namespace LiveScore.Soccer.DTOs.Matches
{
    public class MatchConditionDTO
    {
        public int Attendance { get; set; }

        public VenueDTO Venue { get; set; }

        public string Referee { get; set; }
    }
}
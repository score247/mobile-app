namespace LiveScore.Basketball.DTOs.Matches
{
    public class MatchConditionDto
    {
        public int Attendance { get; set; }

        public VenueDto Venue { get; set; }

        public string Referee { get; set; }
    }
}
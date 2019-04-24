namespace LiveScore.Core.Models.Matches
{
    using LiveScore.Common.Extensions;
    using Newtonsoft.Json;

    public interface IMatchCondition
    {
        int Attendance { get; }

        IVenue Venue { get; }

        string Referee { get; }
    }

    public class MatchCondition : IMatchCondition
    {
        public int Attendance { get; set; }

        [JsonConverter(typeof(JsonConcreteTypeConverter<Venue>))]
        public IVenue Venue { get; set; }

        public string Referee { get; set; }
    }
}
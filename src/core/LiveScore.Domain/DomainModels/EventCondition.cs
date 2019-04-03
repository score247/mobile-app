namespace LiveScore.DomainModels
{
    public interface IEventCondition
    {
        int Attendance { get; }

        IVenue Venue { get; }
    }

    public class EventCondition : IEventCondition
    {
        public int Attendance { get; set; }

        public IVenue Venue { get; set; }
    }

    public class Referee
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
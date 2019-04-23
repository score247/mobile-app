namespace LiveScore.Models
{
    using LiveScore.Core.Enumerations;

    public class SportItem
    {
        public SportTypes Type { get; set; }

        public int NumberOfEvent { get; set; }

        public bool IsVisible { get; set; }
    }
}
namespace LiveScore.Core.Models
{
    using Enumerations;

    public class SportItem
    {
        public SportType Type { get; set; }

        public int NumberOfEvent { get; set; }

        public bool IsVisible { get; set; }
    }
}
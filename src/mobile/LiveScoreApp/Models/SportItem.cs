namespace LiveScoreApp.Models
{
    public class SportItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NumberOfEvent { get; set; }

        public bool IsVisible { get; set; } = false;
    }
}
namespace LiveScore.Core.Models.Matches
{
    using MessagePack;

    [MessagePackObject(keyAsPropertyName: true)]
    public class Coverage
    {
        public bool Live { get; set; }

        public bool BasicScore { get; set; }

        public bool KeyEvents { get; set; }

        public bool DetailedEvents { get; set; }

        public bool Lineups { get; set; }

        public bool Commentary { get; set; }

        public string TrackerWidgetLink { get; set; }
    }
}

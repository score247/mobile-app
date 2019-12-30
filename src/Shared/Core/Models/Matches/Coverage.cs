using MessagePack;

namespace LiveScore.Core.Models.Matches
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class Coverage
    {
        public Coverage(
            bool live,
            bool basicScore,
            bool keyEvents,
            bool detailedEvents,
            bool lineups,
            bool commentary,
            string trackerWidgetLink)
        {
            Live = live;
            BasicScore = basicScore;
            KeyEvents = keyEvents;
            DetailedEvents = detailedEvents;
            Lineups = lineups;
            Commentary = commentary;
            TrackerWidgetLink = trackerWidgetLink;
        }

        public bool Live { get; private set; }

        public bool BasicScore { get; private set; }

        public bool KeyEvents { get; private set; }

        public bool DetailedEvents { get; private set; }

        public bool Lineups { get; private set; }

        public bool Commentary { get; private set; }

        public string TrackerWidgetLink { get; private set; }
    }
}
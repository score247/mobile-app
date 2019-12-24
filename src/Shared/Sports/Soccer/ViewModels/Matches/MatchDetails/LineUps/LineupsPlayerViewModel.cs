using System.Collections.Generic;
using System.Linq;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.TimelineImages;

namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.LineUps
{
    public class LineupsPlayerViewModel : LineupsItemViewModel
    {
        private readonly IEnumerable<EventType> LineupsEvents =
            new List<EventType> {
                EventType.ScoreChangeByOwnGoal,
                EventType.ScoreChange,
                EventType.Substitution};

#pragma warning disable S107 // Methods should not have too many parameters

        public LineupsPlayerViewModel(
            IDependencyResolver dependencyResolver,
            string homeName,
            string awayName,
            int? homeJerseyNumber = null,
            int? awayJerseyNumber = null,
            IDictionary<EventType, int> homeEventStatistics = null,
            IDictionary<EventType, int> awayEventStatistics = null,
            bool isSubstitute = false)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(dependencyResolver)
        {
            HomeName = homeName;
            AwayName = awayName;
            HomeJerseyNumber = homeJerseyNumber;
            AwayJerseyNumber = awayJerseyNumber;
            IsSubstitute = isSubstitute;

            HonePlayerStatistics = BuildPlayerStatistics(homeEventStatistics);
            AwayPlayerStatistics = BuildPlayerStatistics(awayEventStatistics);
        }

        public List<KeyValuePair<string, int>> HonePlayerStatistics { get; private set; }
        public List<KeyValuePair<string, int>> AwayPlayerStatistics { get; private set; }

        public string HomeName { get; }

        public string AwayName { get; }

        public int? HomeJerseyNumber { get; }

        public int? AwayJerseyNumber { get; }

        protected ITimelineEventImageBuilder ImageConverter { get; set; }

        public bool IsSubstitute { get; private set; }

        private List<KeyValuePair<string, int>> BuildPlayerStatistics(IDictionary<EventType, int> playerStatistics)
        {
            var eventStatistics = InitEventStatistics();
            if (playerStatistics?.Any() == true)
            {
                var filteredEventStatistics = SumPlayerStatistics(eventStatistics, playerStatistics);

                return BuilStatisticImage(filteredEventStatistics);
            }

            return new List<KeyValuePair<string, int>>();
        }

        private List<KeyValuePair<string, int>> BuilStatisticImage(IDictionary<EventType, int> eventStatistics)
        {
            var allEventStatistics = eventStatistics.ToList();
            var statisticWithImage = new List<KeyValuePair<string, int>>();
            foreach (var statistic in allEventStatistics)
            {
                statisticWithImage.Add(new KeyValuePair<string, int>(BuildEventTimelineImage(statistic.Key), statistic.Value));
            }

            return statisticWithImage;
        }

        private static IDictionary<EventType, int> SumPlayerStatistics(IDictionary<EventType, int> eventStatistics, IDictionary<EventType, int> playerEventStatistics)
        {
            foreach (var eventStatistic in playerEventStatistics)
            {
                if (eventStatistics.ContainsKey(eventStatistic.Key))
                {
                    eventStatistics[eventStatistic.Key] += eventStatistic.Value;
                }

                if (eventStatistic.Key == EventType.ScoreChangeByPenalty)
                {
                    eventStatistics[EventType.ScoreChange] += eventStatistic.Value;
                }
            }

            return eventStatistics
                    .Where(eventStatistic => eventStatistic.Value > 0)
                    .ToDictionary(eventStatistic => eventStatistic.Key, eventStatistic => eventStatistic.Value);
        }

        private IDictionary<EventType, int> InitEventStatistics()
        {
            var eventStatistics = new Dictionary<EventType, int>();
            foreach (var eventType in LineupsEvents)
            {
                eventStatistics.Add(eventType, 0);
            }

            return eventStatistics;
        }

        private string BuildEventTimelineImage(EventType eventType)
        {
            var displayEventType = eventType;

            if (eventType == EventType.Substitution)
            {
                displayEventType = IsSubstitute ? EventType.SubstitutionIn : EventType.SubstitutionOut;
            }

            if (eventType == EventType.ScoreChange || eventType == EventType.PenaltyShootout)
            {
                ImageConverter = DependencyResolver.Resolve<ITimelineEventImageBuilder>(displayEventType.Value.ToString());
            }
            else
            {
                ImageConverter = new DefaultEventImageBuilder();
            }

            return ImageConverter.BuildImageSource(new TimelineEventImage(displayEventType));
        }
    }
}
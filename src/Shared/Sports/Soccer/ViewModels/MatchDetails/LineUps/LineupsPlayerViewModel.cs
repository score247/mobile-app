using System.Collections.Generic;
using System.Linq;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.TimelineImages;

namespace LiveScore.Soccer.ViewModels.MatchDetails.LineUps
{
    public class LineupsPlayerViewModel : LineupsItemViewModel
    {
        private readonly IEnumerable<EventType> LineupsEvents =
            new List<EventType> { EventType.Substitution,
                EventType.ScoreChange,
                EventType.ScoreChangeByOwnGoal};

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

            BuildHomeEventImages(homeEventStatistics);
            BuildAwayEventImages(awayEventStatistics);
        }

        public string HomeName { get; }

        public string AwayName { get; }

        public int? HomeJerseyNumber { get; }

        public int? AwayJerseyNumber { get; }

        public string HomeEventOneImageSource { get; private set; }

        public int HomeEventOneCount { get; private set; }

        public bool HomeEventOneVisible { get; private set; }

        public string HomeEventTwoImageSource { get; private set; }

        public int HomeEventTwoCount { get; private set; }

        public bool HomeEventTwoVisible { get; private set; }

        public string HomeEventThreeImageSource { get; private set; }

        public int HomeEventThreeCount { get; private set; }

        public bool HomeEventThreeVisible { get; private set; }

        public string AwayEventOneImageSource { get; private set; }

        public int AwayEventOneCount { get; private set; }

        public bool AwayEventOneVisible { get; private set; }

        public string AwayEventTwoImageSource { get; private set; }

        public int AwayEventTwoCount { get; private set; }

        public bool AwayEventTwoVisible { get; private set; }

        public string AwayEventThreeImageSource { get; private set; }

        public int AwayEventThreeCount { get; private set; }

        public bool AwayEventThreeVisible { get; private set; }

        protected ITimelineEventImageBuilder ImageConverter { get; set; }

        public bool IsSubstitute { get; private set; }

        private void BuildAwayEventImages(IDictionary<EventType, int> awayEventStatistics)
        {
            var eventStatistics = InitEventStatistics();
            if (awayEventStatistics?.Any() == true)
            {
                var filteredEventStatistics = UpdateEventStatistics(eventStatistics, awayEventStatistics);

                ApplyAwayEventImage(filteredEventStatistics);
            }
        }

        private void ApplyAwayEventImage(IDictionary<EventType, int> eventStatistics)
        {
            var allEventStatistics = eventStatistics.ToList();
            if (allEventStatistics.Count > 0)
            {
                AwayEventOneImageSource = BuildEventTimelineImage(allEventStatistics[0].Key);
                AwayEventOneCount = allEventStatistics[0].Value;
                AwayEventOneVisible = GetEventVisibility(allEventStatistics[0]);
            }
            if (allEventStatistics.Count > 1)
            {
                AwayEventTwoImageSource = BuildEventTimelineImage(allEventStatistics[1].Key);
                AwayEventTwoCount = allEventStatistics[1].Value;
                AwayEventTwoVisible = GetEventVisibility(allEventStatistics[1]);
            }
            if (allEventStatistics.Count > 2)
            {
                AwayEventThreeImageSource = BuildEventTimelineImage(allEventStatistics[2].Key);
                AwayEventThreeCount = allEventStatistics[2].Value;
                AwayEventThreeVisible = GetEventVisibility(allEventStatistics[2]);
            }
        }

        private void BuildHomeEventImages(IDictionary<EventType, int> homeEventStatistics)
        {
            var eventStatistics = InitEventStatistics();
            if (homeEventStatistics != null)
            {
                var filteredEventStatistics = UpdateEventStatistics(eventStatistics, homeEventStatistics);

                ApplyHomeEventImage(filteredEventStatistics);
            }
        }

        private void ApplyHomeEventImage(IDictionary<EventType, int> eventStatistics)
        {
            var allEventStatistics = eventStatistics.ToList();
            if (allEventStatistics.Count > 0)
            {
                HomeEventOneImageSource = BuildEventTimelineImage(allEventStatistics[0].Key);
                HomeEventOneCount = allEventStatistics[0].Value;
                HomeEventOneVisible = GetEventVisibility(allEventStatistics[0]);
            }
            if (allEventStatistics.Count > 1)
            {
                HomeEventTwoImageSource = BuildEventTimelineImage(allEventStatistics[1].Key);
                HomeEventTwoCount = allEventStatistics[1].Value;
                HomeEventTwoVisible = GetEventVisibility(allEventStatistics[1]);
            }
            if (allEventStatistics.Count > 2)
            {
                HomeEventThreeImageSource = BuildEventTimelineImage(allEventStatistics[2].Key);
                HomeEventThreeCount = allEventStatistics[2].Value;
                HomeEventThreeVisible = GetEventVisibility(allEventStatistics[2]);
            }
        }

        private static IDictionary<EventType, int> UpdateEventStatistics(IDictionary<EventType, int> eventStatistics, IDictionary<EventType, int> playerEventStatistics)
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

            try
            {
                ImageConverter = DependencyResolver.Resolve<ITimelineEventImageBuilder>(displayEventType.Value.ToString());
            }
            catch
            {
                ImageConverter = new DefaultEventImageBuilder();
            }

            return ImageConverter.BuildImageSource(new TimelineEventImage(displayEventType));
        }

        private static bool GetEventVisibility(KeyValuePair<EventType, int> eventTimline)
        {
            return eventTimline.Value > 1;
        }
    }
}
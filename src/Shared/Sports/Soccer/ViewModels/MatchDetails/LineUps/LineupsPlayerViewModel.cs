using System;
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

        public string HomeEventOneImageSource { get; private set; }

        public string HomeEventTwoImageSource { get; private set; }

        public string HomeEventThreeImageSource { get; private set; }

        public string AwayEventOneImageSource { get; private set; }

        public string AwayEventTwoImageSource { get; private set; }

        public string AwayEventThreeImageSource { get; private set; }

        public LineupsPlayerViewModel(
            IDependencyResolver dependencyResolver,
            string homeName,
            string awayName,
            int? homeJerseyNumber = null,
            int? awayJerseyNumber = null,
            IDictionary<EventType, int> homeEventStatistics = null,
            IDictionary<EventType, int> awayEventStatistics = null)
            : base(dependencyResolver, homeName, awayName, homeJerseyNumber, awayJerseyNumber)
        {
            BuildHomeEventImages(homeEventStatistics);
            BuildAwayEventImages(awayEventStatistics);
        }

        private void BuildAwayEventImages(IDictionary<EventType, int> awayEventStatistics)
        {
            var eventStatistics = InitEventStatistics();
            if (awayEventStatistics != null)
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
                AwayEventOneImageSource = BuildEventImage(allEventStatistics[0].Key);
            }
            if (allEventStatistics.Count > 1)
            {
                AwayEventTwoImageSource = BuildEventImage(allEventStatistics[1].Key);
            }
            if (allEventStatistics.Count > 2)
            {
                AwayEventThreeImageSource = BuildEventImage(allEventStatistics[2].Key);
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
                HomeEventOneImageSource = BuildEventImage(allEventStatistics[0].Key);
            }
            if (allEventStatistics.Count > 1)
            {
                HomeEventTwoImageSource = BuildEventImage(allEventStatistics[1].Key);
            }
            if (allEventStatistics.Count > 2)
            {
                HomeEventThreeImageSource = BuildEventImage(allEventStatistics[2].Key);
            }
        }

        private string BuildEventImage(EventType eventType)
        {
            var imageConverter = DependencyResolver.Resolve<ITimelineEventImageBuilder>(eventType.Value.ToString());
            return imageConverter.BuildImageSource(new TimelineEventImage(eventType));
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
    }
}
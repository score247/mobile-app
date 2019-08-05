﻿namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Extensions;
    using LiveScore.Soccer.Views.Templates.MatchDetailInfo;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class BaseItemViewModel : ViewModelBase
    {
        private static readonly IDictionary<EventTypes, Type> ViewModelMapper = new Dictionary<EventTypes, Type>
        {
            { EventTypes.YellowCard, typeof(DefaultItemViewModel) },
            { EventTypes.YellowRedCard, typeof(DefaultItemViewModel) },
            { EventTypes.RedCard, typeof(DefaultItemViewModel) },
            { EventTypes.PenaltyMissed, typeof(DefaultItemViewModel) },
            { EventTypes.ScoreChange, typeof(ScoreChangeItemViewModel) },
            { EventTypes.BreakStart, typeof(MainEventItemViewModel) },
            { EventTypes.PeriodStart, typeof(MainEventItemViewModel) },
            { EventTypes.MatchEnded, typeof(MainEventItemViewModel) },
            { EventTypes.PenaltyShootout, typeof(PenaltyShootOutViewModel) }
        };

        private static readonly IDictionary<EventTypes, DataTemplate> TemplateMapper = new Dictionary<EventTypes, DataTemplate>
        {
            { EventTypes.YellowCard, new DefaultItemTemplate() },
            { EventTypes.YellowRedCard, new DefaultItemTemplate() },
            { EventTypes.RedCard, new DefaultItemTemplate() },
            { EventTypes.PenaltyMissed, new DefaultItemTemplate() },
            { EventTypes.BreakStart, new MainEventItemTemplate() },
            { EventTypes.PeriodStart, new MainEventItemTemplate() },
            { EventTypes.MatchEnded, new MainEventItemTemplate() },
            { EventTypes.ScoreChange, new ScoreChangeItemTemplate() },
            { EventTypes.PenaltyShootout, new PenaltyShootOutTemplate() }
        };

        public BaseItemViewModel(
            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
            : base(navigationService, depdendencyResolver)
        {
            TimelineEvent = timelineEvent;
            Result = matchResult;

            BuildData();
            ItemAutomationId = $"{TimelineEvent.Id}-{TimelineEvent.Type}";
        }

        public string ItemAutomationId { get; }

        public ITimeline TimelineEvent { get; }

        public IMatchResult Result { get; }

        public Color RowColor { get; protected set; }

        public string MatchTime { get; protected set; }

        public string Score { get; protected set; }

        public bool VisibleScore { get; protected set; }

        public string HomePlayerName { get; protected set; }

        public string AwayPlayerName { get; protected set; }

        public string ImageSource { get; protected set; }

        public bool VisibleHomeImage { get; protected set; }

        public bool VisibleAwayImage { get; protected set; }

        public EventTypes TimelineEventType => Enumeration.FromDisplayName<EventTypes>(TimelineEvent.Type);

        public BaseItemViewModel CreateInstance()
        {
            if (ViewModelMapper.ContainsKey(TimelineEventType))
            {
                return Activator.CreateInstance(
                    ViewModelMapper[TimelineEventType],
                    TimelineEvent, Result, NavigationService, DependencyResolver) as BaseItemViewModel;
            }

            return new BaseItemViewModel(TimelineEvent, Result, NavigationService, DependencyResolver);
        }

        public DataTemplate CreateTemplate()
        {
            if (TemplateMapper.ContainsKey(TimelineEventType))
            {
                return TemplateMapper[TimelineEventType];
            }

            return new MainEventItemTemplate();
        }

        public static bool ValidateEvent(ITimeline timeline, IMatchResult matchResult)
            => timeline.IsDetailInfoEvent()
                || timeline.IsPenaltyShootOutStart()
                || timeline.IsNotExtraTimeHalfTimeBreak()
                || timeline.IsMatchEndNotAfterPenalty(matchResult);

        public static IEnumerable<ITimeline> FilterPenaltyEvents(IEnumerable<ITimeline> timelines, IMatchResult matchResult)
        {
            if (matchResult == null)
            {
                return timelines;
            }

            if (matchResult.EventStatus.IsClosed())
            {
                var timelineEvents = timelines.ToList();
                timelineEvents.RemoveAll(t => Enumeration.FromDisplayName<EventTypes>(t.Type) == EventTypes.PenaltyShootout && t.IsFirstShoot);

                return timelineEvents;
            }

            if (matchResult.EventStatus.IsLive() && matchResult.MatchStatus.IsInPenalties)
            {
                var lastEvent = timelines.LastOrDefault();
                var timelineEvents = timelines.ToList();
                timelineEvents.RemoveAll(t => t.IsFirstShoot);

                if (lastEvent?.IsFirstShoot == true)
                {
                    timelineEvents.Add(lastEvent);
                }

                return timelineEvents;
            }

            return timelines;
        }

        protected virtual void BuildInfo()
        {
            Score = $"{TimelineEvent.HomeScore} - {TimelineEvent.AwayScore}";

            if (Application.Current != null)
            {
                RowColor = (Color)Application.Current.Resources["PrimaryColor"];
            }

            MatchTime = string.IsNullOrEmpty(TimelineEvent.StoppageTime)
                ? TimelineEvent.MatchTime + "'"
                : TimelineEvent.MatchTime + "+" + TimelineEvent.StoppageTime + "'";
        }

        private void BuildData()
        {
            BuildInfo();
        }
    }
}
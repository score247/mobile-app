﻿namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System;
    using System.Collections.Generic;
    using LiveScore.Core;
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Models.Matches;
    using LiveScore.Core.ViewModels;
    using LiveScore.Soccer.Views.Templates.MatchDetailInfo;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class BaseItemViewModel : ViewModelBase
    {
        private static readonly IDictionary<EventType, Type> ViewModelMapper = new Dictionary<EventType, Type>
        {
            { EventType.YellowCard, typeof(DefaultItemViewModel) },
            { EventType.YellowRedCard, typeof(DefaultItemViewModel) },
            { EventType.RedCard, typeof(DefaultItemViewModel) },
            { EventType.PenaltyMissed, typeof(DefaultItemViewModel) },
            { EventType.ScoreChange, typeof(ScoreChangeItemViewModel) },
            { EventType.BreakStart, typeof(MainEventItemViewModel) },
            { EventType.PeriodStart, typeof(MainEventItemViewModel) },
            { EventType.MatchEnded, typeof(MainEventItemViewModel) },
            { EventType.PenaltyShootout, typeof(PenaltyShootOutViewModel) }
        };

        private static readonly IDictionary<EventType, DataTemplate> TemplateMapper = new Dictionary<EventType, DataTemplate>
        {
            { EventType.YellowCard, new DefaultItemTemplate() },
            { EventType.YellowRedCard, new DefaultItemTemplate() },
            { EventType.RedCard, new DefaultItemTemplate() },
            { EventType.PenaltyMissed, new DefaultItemTemplate() },
            { EventType.BreakStart, new MainEventItemTemplate() },
            { EventType.PeriodStart, new MainEventItemTemplate() },
            { EventType.MatchEnded, new MainEventItemTemplate() },
            { EventType.ScoreChange, new ScoreChangeItemTemplate() },
            { EventType.PenaltyShootout, new PenaltyShootOutTemplate() }
        };

        public BaseItemViewModel(
            ITimelineEvent timelineEvent,
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

        public ITimelineEvent TimelineEvent { get; }

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

        public BaseItemViewModel CreateInstance()
        {
            if (ViewModelMapper.ContainsKey(TimelineEvent.Type))
            {
                return Activator.CreateInstance(
                    ViewModelMapper[TimelineEvent.Type],
                    TimelineEvent, Result, NavigationService, DependencyResolver) as BaseItemViewModel;
            }

            return new BaseItemViewModel(TimelineEvent, Result, NavigationService, DependencyResolver);
        }

        public DataTemplate CreateTemplate()
        {
            if (TemplateMapper.ContainsKey(TimelineEvent.Type))
            {
                return TemplateMapper[TimelineEvent.Type];
            }

            return new MainEventItemTemplate();
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
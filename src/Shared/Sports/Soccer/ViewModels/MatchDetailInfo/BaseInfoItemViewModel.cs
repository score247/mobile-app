namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
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

    public class BaseInfoItemViewModel : ViewModelBase
    {
        public static readonly string[] InfoItemEventTypes = new[] {
            EventTypes.BreakStart,
            EventTypes.ScoreChange,
            EventTypes.PenaltyMissed,
            EventTypes.YellowCard,
            EventTypes.RedCard,
            EventTypes.YellowRedCard,
        };

        private static readonly IDictionary<string, Type> ViewModelMapper = new Dictionary<string, Type>
        {
            { EventTypes.ScoreChange, typeof(ScoreChangeItemViewModel) },
            { EventTypes.YellowCard, typeof(CardItemViewModel) },
            { EventTypes.YellowRedCard, typeof(CardItemViewModel) },
            { EventTypes.RedCard, typeof(CardItemViewModel) },
            { EventTypes.BreakStart, typeof(HalfTimeItemViewModel) },
            { EventTypes.PenaltyMissed, typeof(PenaltyMissedItemViewModel) }
        };

        private static readonly IDictionary<string, DataTemplate> TemplateMapper = new Dictionary<string, DataTemplate>
        {
            { EventTypes.ScoreChange, new ScoreChangeItemTemplate() },
            { EventTypes.YellowCard, new CardItemTemplate() },
            { EventTypes.YellowRedCard, new CardItemTemplate() },
            { EventTypes.RedCard, new CardItemTemplate() },
            { EventTypes.BreakStart, new EventStatusItemTemplate() },
            { EventTypes.PenaltyMissed, new PenaltyMissedItemTemplate() }
        };

        public BaseInfoItemViewModel(
            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
            : base(navigationService, depdendencyResolver)
        {
            TimelineEvent = timelineEvent;
            Result = matchResult;

            BuildData();
        }

        public ITimeline TimelineEvent { get; }

        public IMatchResult Result { get; }

        public Color RowColor { get; protected set; }

        public string MatchTime { get; protected set; }

        public string Score { get; protected set; }

        public string HomePlayerName { get; protected set; }

        public string AwayPlayerName { get; protected set; }

        public BaseInfoItemViewModel CreateInstance()
        {
            if (BaseInfoItemViewModel.ViewModelMapper.ContainsKey(TimelineEvent.Type))
            {
                return Activator.CreateInstance(
                    BaseInfoItemViewModel.ViewModelMapper[TimelineEvent.Type],
                    TimelineEvent, Result, NavigationService, DependencyResolver) as BaseInfoItemViewModel;
            }

            return new BaseInfoItemViewModel(TimelineEvent, Result, NavigationService, DependencyResolver);
        }

        public DataTemplate CreateTemplate()
        {
            if (TemplateMapper.ContainsKey(TimelineEvent.Type))
            {
                return TemplateMapper[TimelineEvent.Type];
            }

            return new EventStatusItemTemplate();
        }

        protected virtual void BuildInfo()
        {
            Score = $"{TimelineEvent.HomeScore} - {TimelineEvent.AwayScore}";

            if (Application.Current != null)
            {
                RowColor = (Color)Application.Current.Resources["SecondColor"];
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
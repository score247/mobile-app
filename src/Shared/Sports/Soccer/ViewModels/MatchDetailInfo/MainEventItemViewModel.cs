namespace LiveScore.Soccer.ViewModels.MatchDetailInfo
{
    using System.Linq;
    using LiveScore.Common.LangResources;
    using LiveScore.Core;
    using LiveScore.Core.Models.Matches;
    using Prism.Navigation;
    using Xamarin.Forms;

    public class MainEventItemViewModel : BaseItemViewModel
    {
        public MainEventItemViewModel(
            ITimeline timelineEvent,
            IMatchResult matchResult,
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
             : base(timelineEvent, matchResult, navigationService, depdendencyResolver)
        {
        }

        public string MainEventStatus { get; protected set; }

        protected override void BuildInfo()
        {
            base.BuildInfo();

            if (Application.Current != null)
            {
                RowColor = (Color)Application.Current.Resources["LineColor"];
            }

            MainEventStatus = AppResources.HalfTime;

            if (Result?.MatchPeriods != null)
            {
                var halfTimeResult = Result.MatchPeriods?.FirstOrDefault();
                Score = $"{halfTimeResult?.HomeScore} - {halfTimeResult?.AwayScore}";
            }
            else
            {
                Score = "-";
            }
        }
    }
}
using System.Linq;
using System.Runtime.CompilerServices;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Extensions;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.TimelineImages;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("Soccer.Tests")]
namespace LiveScore.Soccer.ViewModels.MatchDetails.TrackerCommentary
{
    public class CommentaryItemViewModel
    {
        private readonly IDependencyResolver dependencyResolver;

        public CommentaryItemViewModel(MatchCommentary commentary, IDependencyResolver dependencyResolver)
        {
            Commentary = commentary;
            this.dependencyResolver = dependencyResolver;

            BuildData();
        }

        public MatchCommentary Commentary { get; }

        public string ImageSource { get; private set; }

        public string MatchTime { get; private set; }

        public string CommentaryText { get; private set; }

        public Color CommentaryTextColor { get; private set; }

        public bool VisibleMatchTime { get; private set; }

        internal void BuildData()
        {
            if (Commentary == null)
            {
                return;
            }

            MatchTime = BuildMatchTime();
            ImageSource = BuildImageSource();
            CommentaryText = BuildCommentaryText();
            CommentaryTextColor = (Color)Application.Current.Resources[GetColorResourceName()];
            VisibleMatchTime = Commentary.IsVisibleMatchTimeEvent();
        }

        internal string BuildMatchTime()
        => string.IsNullOrEmpty(Commentary.StoppageTime)
                ? $"{Commentary.MatchTime}'"
                : $"{Commentary.MatchTime}+{Commentary.StoppageTime}'";

        internal string BuildImageSource()
        {
            ITimelineEventImageBuilder imageConverter;

            try
            {
                imageConverter =
                    dependencyResolver.Resolve<ITimelineEventImageBuilder>(Commentary.TimelineType.Value.ToString());
            }
            catch
            {
                imageConverter = new DefaultEventImageBuilder();
            }

            return imageConverter.BuildImageSource(
                    new TimelineEventImage(
                        Commentary.TimelineType,
                        Commentary.GoalScorer,
                        Commentary.IsPenaltyShootOutScored));
        }

        internal string BuildCommentaryText()
        => Commentary.Commentaries?.Any() != true
                ? EventType.EventTypeNames[Commentary.TimelineType]
                : string.Join("\r\n", Commentary.Commentaries.Select(c => c.Text));

        internal string GetColorResourceName()
        => Commentary.TimelineType.IsHighlightEvent()
                ? "HighlightCommentaryColor"
                : "CommentaryColor";
    }
}
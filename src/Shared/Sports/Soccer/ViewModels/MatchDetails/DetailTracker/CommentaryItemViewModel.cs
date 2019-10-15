using System.Linq;
using LiveScore.Core;
using LiveScore.Soccer.Converters.TimelineImages;
using LiveScore.Soccer.Extensions;
using LiveScore.Soccer.Models.Matches;
using Xamarin.Forms;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailTracker
{
    public class CommentaryItemViewModel
    {
        private readonly IDependencyResolver dependencyResolver;

        public CommentaryItemViewModel(MatchCommentary commentary, IDependencyResolver dependencyResolver)
        {
            Commentary = commentary;
            this.dependencyResolver = dependencyResolver;

            BuildMatchTime();
            BuildImageSource();
            BuildCommentaryText();
            BuildHighlightColor();
        }

        public MatchCommentary Commentary { get; }

        public string ImageSource { get; private set; }

        public string MatchTime { get; private set; }

        public string CommentaryText { get; private set; }

        public Color CommentaryTextColor { get; private set; }

        private void BuildMatchTime()
        {
            if (Commentary == null)
            {
                return;
            }

            MatchTime = string.IsNullOrEmpty(Commentary.StoppageTime)
                ? $"{Commentary.MatchTime}'"
                : $"{Commentary.MatchTime}+{Commentary.StoppageTime}'";
        }

        private void BuildImageSource()
        {
            ITimelineEventImageConverter imageConverter;

            try
            {
                imageConverter =
                    dependencyResolver.Resolve<ITimelineEventImageConverter>(Commentary.TimelineType.Value.ToString());
            }
            catch
            {
                imageConverter = new DefaultEventImageConverter();
            }

            ImageSource =
                imageConverter.BuildImageSource(new TimelineEventImage(Commentary.TimelineType, Commentary.GoalScorer));
        }

        private void BuildCommentaryText()
        {
            if (Commentary == null)
            {
                return;
            }

            CommentaryText = string.Join("\r\n", Commentary.Commentaries.Select(c => c.Text));
        }

        private void BuildHighlightColor()
        {
            CommentaryTextColor = Commentary.TimelineType.IsHighlightEvent()
                ? (Color)Application.Current.Resources["HighlightCommentaryColor"]
                : (Color)Application.Current.Resources["CommentaryColor"];
        }
    }
}
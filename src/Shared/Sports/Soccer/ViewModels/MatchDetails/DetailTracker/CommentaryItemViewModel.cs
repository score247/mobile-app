using System.Linq;
using LiveScore.Core;
using LiveScore.Soccer.Converters.TimelineImages;
using LiveScore.Soccer.Models.Matches;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailTracker
{
    public class CommentaryItemViewModel
    {
        private readonly IDependencyResolver dependencyResolver;

        public CommentaryItemViewModel(MatchCommentary commentary, IDependencyResolver dependencyResolver)
        {
            Commentary = commentary;
            this.dependencyResolver = dependencyResolver;

            BuildMatchTime(commentary);
            BuildImageSource();
            BuildCommentaryText(commentary);
        }

        public MatchCommentary Commentary { get; }

        public string ImageSource { get; private set; }

        public string MatchTime { get; private set; }

        public string CommentaryText { get; private set; }

        private void BuildMatchTime(MatchCommentary commentary)
        {
            MatchTime = string.IsNullOrEmpty(commentary.StoppageTime)
                ? $"{commentary.MatchTime}'"
                : $"{commentary.MatchTime}+{commentary.StoppageTime}";
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

        private void BuildCommentaryText(MatchCommentary commentary)
        {
            CommentaryText = string.Join("\r\n", commentary.Commentaries.Select(c => c.Text));
        }
    }
}
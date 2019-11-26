using System.Linq;
using AutoFixture;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.TimelineImages;
using LiveScore.Soccer.ViewModels.MatchDetails.TrackerCommentary;
using NSubstitute;
using Prism.Navigation;
using Xunit;

namespace Soccer.Tests.ViewModels.MatchDetails.TrackerCommenraty
{
    public class CommentaryItemViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly INavigationService navigationService;
        private readonly IDependencyResolver dependencyResolver;
        private readonly Fixture fixture;

        public CommentaryItemViewModelTests(ViewModelBaseFixture baseFixture)
        {
            navigationService = baseFixture.NavigationService;
            dependencyResolver = baseFixture.DependencyResolver;
            fixture = baseFixture.CommonFixture.Specimens;

            baseFixture.DependencyResolver.Resolve<ITimelineEventImageBuilder>(Arg.Is<string>(x => x != "NotFound"))
               .Returns(new DefaultEventImageBuilder());
        }

        [Fact]
        public void BuildCommentaryText_NotNull_CorrectFormat()
        {
            var matchCommentary = fixture.Create<MatchCommentary>();

            var viewModel = new CommentaryItemViewModel(matchCommentary, dependencyResolver);

            Assert.Equal(string.Join("\r\n", matchCommentary.Commentaries.Select(c => c.Text)), viewModel.CommentaryText);
        }

        [Theory]
        [InlineData("score_change")]
        [InlineData("yellow_card")]
        [InlineData("yellow_red_card")]
        [InlineData("red_card")]
        [InlineData("penalty_missed")]
        [InlineData("break_start")]
        [InlineData("match_ended")]
        [InlineData("free_kick")]
        [InlineData("goal_kick")]
        public void BuildCommentaryText_EmptyCommentary_ReturnEventName(string eventType)
        {
            var timelineEventType = Enumeration.FromDisplayName<EventType>(eventType);
            var matchCommentary = fixture.Create<MatchCommentary>()
                .With(commentary => commentary.TimelineType, timelineEventType)
                .With(commentary => commentary.Commentaries, Enumerable.Empty<Commentary>());

            var viewModel = new CommentaryItemViewModel(matchCommentary, dependencyResolver);

            Assert.Equal(EventType.EventTypeNames[timelineEventType], viewModel.CommentaryText);
        }

        [Fact]
        public void BuildCommentaryText_Null_ReturnEmpty()
        {
            var viewModel = new CommentaryItemViewModel(null, dependencyResolver);

            Assert.Null(viewModel.CommentaryText);
        }

        [Theory]
        [InlineData("score_change")]
        [InlineData("yellow_card")]
        [InlineData("yellow_red_card")]
        [InlineData("red_card")]
        public void BuildHighlightColor_Highlight_ReturnHighlightCommentaryColor(string eventType)
        {
            var timelineEventType = Enumeration.FromDisplayName<EventType>(eventType);
            var matchCommentary = fixture.Create<MatchCommentary>()
                .With(commentary => commentary.TimelineType, timelineEventType);

            var viewModel = new CommentaryItemViewModel(matchCommentary, dependencyResolver);

            var resource = viewModel.GetColorResourceName();

            Assert.Equal("HighlightCommentaryColor", resource);
        }

        [Theory]
        [InlineData("free_kick")]
        [InlineData("goal_kick")]
        public void BuildHighlightColor_NotHighlight_ReturnCommentaryColor(string eventType)
        {
            var timelineEventType = Enumeration.FromDisplayName<EventType>(eventType);
            var matchCommentary = fixture.Create<MatchCommentary>()
                .With(commentary => commentary.TimelineType, timelineEventType);

            var viewModel = new CommentaryItemViewModel(matchCommentary, dependencyResolver);

            var resource = viewModel.GetColorResourceName();

            Assert.Equal("CommentaryColor", resource);
        }

        [Fact]
        public void BuildMatchTime_StopageTimeNull_ReturnOnlyMatchTime()
        {
            var matchCommentary = fixture.Create<MatchCommentary>()
                .With(commentary => commentary.StoppageTime, string.Empty);
            var viewModel = new CommentaryItemViewModel(matchCommentary, dependencyResolver);

            var matchTime = viewModel.BuildMatchTime();

            Assert.Equal($"{matchCommentary.MatchTime}'", matchTime);
        }

        [Fact]
        public void BuildMatchTime_StoppageTimeNotNull_ReturnMatchTimePlusStoppageTime()
        {
            var matchCommentary = fixture.Create<MatchCommentary>();
            var viewModel = new CommentaryItemViewModel(matchCommentary, dependencyResolver);

            var matchTime = viewModel.BuildMatchTime();

            Assert.Equal($"{matchCommentary.MatchTime}+{matchCommentary.StoppageTime}'", matchTime);
        }

        [Theory]
        [InlineData("yellow_red_card", "images/common/red_yellow_card.svg")]
        [InlineData("red_card", "images/common/red_card.svg")]
        [InlineData("penalty_missed", "images/common/missed_penalty_goal.svg")]
        [InlineData("yellow_card", "images/common/yellow_card.svg")]
        public void BuildImageSource_ReturnCorrectImageSource(string eventType, string expectedImageSource)
        {
            var timelineEventType = Enumeration.FromDisplayName<EventType>(eventType);
            var matchCommentary = fixture.Create<MatchCommentary>()
                .With(commentary => commentary.TimelineType, timelineEventType);

            var viewModel = new CommentaryItemViewModel(matchCommentary, dependencyResolver);

            var imageSource = viewModel.BuildImageSource();

            Assert.Equal(expectedImageSource, imageSource);
        }
    }
}

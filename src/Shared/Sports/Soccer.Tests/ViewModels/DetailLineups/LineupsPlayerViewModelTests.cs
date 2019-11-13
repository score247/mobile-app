using System;
using System.Collections.Generic;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.TimelineImages;
using LiveScore.Soccer.ViewModels.MatchDetails.LineUps;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailLineups
{
    public class LineupsPlayerViewModelTests
    {
        private readonly IDependencyResolver denpendacyResolver;

        public LineupsPlayerViewModelTests()
        {
            denpendacyResolver = Substitute.For<IDependencyResolver>();
        }

        [Fact]
        public void ApplyAwayEventImage_OneEventTimelineValueGreaterThanOne_AwayEventOneVisible()
        {
            // Arrange
            var awayEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 3 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                null,
                awayEventTimelines);

            // Act

            // Assert
            Assert.Equal(3, lineupsPlayer.AwayEventOneCount);
            Assert.True(lineupsPlayer.AwayEventOneVisible);
        }

        [Fact]
        public void ApplyAwayEventImage_OneEventTimelineValueLessThanTwo_AwayEventOneInvisible()
        {
            // Arrange
            var awayEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 1 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                null,
                awayEventTimelines);

            // Act

            // Assert
            Assert.Equal(1, lineupsPlayer.AwayEventOneCount);
            Assert.False(lineupsPlayer.AwayEventOneVisible);
        }

        [Fact]
        public void ApplyAwayEventImage_TwoEventTimelineValueGreaterThanOne_AwayEventTwoVisible()
        {
            // Arrange
            var awayEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 3 },
                { EventType.ScoreChangeByOwnGoal, 3 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                null,
                awayEventTimelines);

            // Act

            // Assert
            Assert.Equal(3, lineupsPlayer.AwayEventTwoCount);
            Assert.True(lineupsPlayer.AwayEventTwoVisible);
        }

        [Fact]
        public void ApplyAwayEventImage_TwoEventTimelineValueLessThanTwo_AwayEventTwoInvisible()
        {
            // Arrange
            var awayEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 1 },
                { EventType.ScoreChangeByOwnGoal, 1 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                null,
                awayEventTimelines);

            // Act

            // Assert
            Assert.Equal(1, lineupsPlayer.AwayEventTwoCount);
            Assert.False(lineupsPlayer.AwayEventTwoVisible);
        }

        [Fact]
        public void ApplyAwayEventImage_ThreeEventTimelineValueGreaterThanOne_AwayEventThreeVisible()
        {
            // Arrange
            var awayEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 3 },
                { EventType.ScoreChangeByOwnGoal, 3 },
                { EventType.Substitution, 1 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                null,
                awayEventTimelines);

            // Act

            // Assert
            Assert.Equal(1, lineupsPlayer.AwayEventOneCount);
            Assert.False(lineupsPlayer.AwayEventOneVisible);
        }

        [Fact]
        public void ApplyAwayEventImage_ThreeEventTimelineValueLessThanTwo_AwayEventThreeInvisible()
        {
            // Arrange
            var awayEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 1 },
                { EventType.ScoreChangeByOwnGoal, 1 },
                { EventType.Substitution, 0 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                null,
                awayEventTimelines);

            // Act

            // Assert
            Assert.Equal(1, lineupsPlayer.AwayEventOneCount);
            Assert.False(lineupsPlayer.AwayEventOneVisible);
        }

        //Home
        [Fact]
        public void ApplyHomeEventImage_OneEventTimelineValueGreaterThanOne_HomeEventOneVisible()
        {
            // Arrange
            var homeEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 3 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                homeEventTimelines,
                null);

            // Act

            // Assert
            Assert.Equal(3, lineupsPlayer.HomeEventOneCount);
            Assert.True(lineupsPlayer.HomeEventOneVisible);
        }

        [Fact]
        public void ApplyHomeEventImage_OneEventTimelineValueLessThanTwo_HomeEventOneInvisible()
        {
            // Arrange
            var homeEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 1 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                homeEventTimelines,
                null);

            // Act

            // Assert
            Assert.Equal(1, lineupsPlayer.HomeEventOneCount);
            Assert.False(lineupsPlayer.HomeEventOneVisible);
        }

        [Fact]
        public void ApplyHomeEventImage_TwoEventTimelineValueGreaterThanOne_HomeEventTwoVisible()
        {
            // Arrange
            var homeEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 3 },
                { EventType.ScoreChangeByOwnGoal, 3 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                homeEventTimelines,
                null);

            // Act

            // Assert
            Assert.Equal(3, lineupsPlayer.HomeEventTwoCount);
            Assert.True(lineupsPlayer.HomeEventTwoVisible);
        }

        [Fact]
        public void ApplyHomeEventImage_TwoEventTimelineValueLessThanTwo_HomeEventTwoInvisible()
        {
            // Arrange
            var homeEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 1 },
                { EventType.ScoreChangeByOwnGoal, 1 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                homeEventTimelines,
                null);

            // Act

            // Assert
            Assert.Equal(1, lineupsPlayer.HomeEventTwoCount);
            Assert.False(lineupsPlayer.HomeEventTwoVisible);
        }

        [Fact]
        public void ApplyHomeEventImage_ThreeEventTimelineValueGreaterThanOne_HomeEventThreeVisible()
        {
            // Arrange
            var homeEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChangeByPenalty, 3 },
                { EventType.ScoreChangeByOwnGoal, 3 },
                { EventType.Substitution, 1 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                homeEventTimelines,
                null);

            // Act

            // Assert
            Assert.Equal(1, lineupsPlayer.HomeEventOneCount);
            Assert.False(lineupsPlayer.HomeEventOneVisible);
        }

        [Fact]
        public void ApplyHomeEventImage_ThreeEventTimelineValueLessThanTwo_HomeEventThreeInvisible()
        {
            // Arrange
            var homeEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 1 },
                { EventType.ScoreChangeByOwnGoal, 1 },
                { EventType.Substitution, 0 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                homeEventTimelines,
                null);

            // Act

            // Assert
            Assert.Equal(1, lineupsPlayer.HomeEventOneCount);
            Assert.False(lineupsPlayer.HomeEventOneVisible);
        }

        [Fact]
        public void BuildEventTimelineImage_EventScoreChange_ScoreImage()
        {
            // Arrange
            var scoreImage = "scoreImage";
            var timelineEventImageBuilder = Substitute.For<ITimelineEventImageBuilder>();
            timelineEventImageBuilder.BuildImageSource(Arg.Is<TimelineEventImage>(x => x.Type == EventType.ScoreChange)).Returns(scoreImage);
            denpendacyResolver.Resolve<ITimelineEventImageBuilder>(EventType.ScoreChange.Value.ToString()).Returns(timelineEventImageBuilder);
            var homeEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChange, 1 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                homeEventTimelines,
                null);

            // Act

            // Assert
            Assert.Equal(scoreImage, lineupsPlayer.HomeEventOneImageSource);
        }

        [Fact]
        public void BuildEventTimelineImage_EventOwnGoal_OwnGoalImage()
        {
            // Arrange
            var ownGoalImage = "own goal";
            var timelineEventImageBuilder = Substitute.For<ITimelineEventImageBuilder>();
            timelineEventImageBuilder.BuildImageSource(Arg.Is<TimelineEventImage>(x => x.Type == EventType.ScoreChangeByOwnGoal)).Returns(ownGoalImage);
            denpendacyResolver.Resolve<ITimelineEventImageBuilder>(EventType.ScoreChangeByOwnGoal.Value.ToString()).Returns(timelineEventImageBuilder);
            var homeEventTimelines = new Dictionary<EventType, int>
            {
                { EventType.ScoreChangeByOwnGoal, 1 }
            };

            var lineupsPlayer = new LineupsPlayerViewModel(
                denpendacyResolver,
                "playerHome",
                "playerAway",
                1,
                10,
                homeEventTimelines,
                null);

            // Act

            // Assert
            Assert.Equal(ownGoalImage, lineupsPlayer.HomeEventOneImageSource);
        }

        //[Fact]
        //public void BuildEventTimelineImage_EventSubstitutionIn_SubstitutionInImage()
        //{
        //    // Arrange
        //    var substitutionIn = "scoreImage";
        //    var timelineEventImageBuilder = Substitute.For<ITimelineEventImageBuilder>();
        //    timelineEventImageBuilder.BuildImageSource(Arg.Is<TimelineEventImage>(x => x.Type == EventType.SubstitutionIn)).Returns(substitutionIn);
        //    var homeEventTimelines = new Dictionary<EventType, int>
        //    {
        //        { EventType.ScoreChange, 1 }
        //    };

        //    var lineupsPlayer = new LineupsPlayerViewModel(
        //        denpendacyResolver,
        //        "playerHome",
        //        "playerAway",
        //        1,
        //        10,
        //        homeEventTimelines,
        //        null);

        //    // Act

        //    // Assert
        //    Assert.Equal(substitutionIn, lineupsPlayer.HomeEventOneImageSource);
        //}
    }
}
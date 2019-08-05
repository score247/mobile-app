namespace LiveScore.Core.Tests.Enumerations
{
    using LiveScore.Core.Enumerations;
    using Xunit;

    public class MatchStatusTests
    {
        [Fact]
        public void NotStartedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.NotStarted;

            // Assert
            Assert.Equal(1, actual.Value);
            Assert.Equal("not_started", actual.DisplayName);          
        }

        [Fact]
        public void LiveStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Live;

            // Assert
            Assert.Equal(5, actual.Value);
            Assert.Equal("live", actual.DisplayName);
        }

        [Fact]
        public void FirstHaftStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.FirstHalf;

            // Assert
            Assert.Equal(6, actual.Value);
            Assert.Equal("1st_half", actual.DisplayName);
        }

        [Fact]
        public void SecondHaftStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.SecondHalf;

            // Assert
            Assert.Equal(7, actual.Value);
            Assert.Equal("2nd_half", actual.DisplayName);
        }

        [Fact]
        public void OvertimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Overtime;

            // Assert
            Assert.Equal(8, actual.Value);
            Assert.Equal("overtime", actual.DisplayName);
        }

        [Fact]
        public void FirstHaftExtraStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.FirstHalfExtra;

            // Assert
            Assert.Equal(9, actual.Value);
            Assert.Equal("1st_extra", actual.DisplayName);
        }

        [Fact]
        public void SecondHaftExtraStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.SecondHalfExtra;

            // Assert
            Assert.Equal(10, actual.Value);
            Assert.Equal("2nd_extra", actual.DisplayName);
        }

        [Fact]
        public void AwaitingPenaltiesStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.AwaitingPenalties;

            // Assert
            Assert.Equal(11, actual.Value);
            Assert.Equal("awaiting_penalties", actual.DisplayName);
        }

        [Fact]
        public void PenaltiesStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Penalties;

            // Assert
            Assert.Equal(12, actual.Value);
            Assert.Equal("penalties", actual.DisplayName);
        }

        [Fact]
        public void PauseStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Pause;

            // Assert
            Assert.Equal(13, actual.Value);
            Assert.Equal("pause", actual.DisplayName);
        }

        [Fact]
        public void AwaitingExtraTimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.AwaitingExtraTime;

            // Assert
            Assert.Equal(14, actual.Value);
            Assert.Equal("awaiting_extra_time", actual.DisplayName);
        }

        [Fact]
        public void InterruptedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Interrupted;

            // Assert
            Assert.Equal(15, actual.Value);
            Assert.Equal("interrupted", actual.DisplayName);
        }

        [Fact]
        public void AbandonedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Abandoned;

            // Assert
            Assert.Equal(20, actual.Value);
            Assert.Equal("abandoned", actual.DisplayName);
        }

        [Fact]
        public void PostponedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Postponed;

            // Assert
            Assert.Equal(2, actual.Value);
            Assert.Equal("postponed", actual.DisplayName);
        }

        [Fact]
        public void DelayedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Delayed;

            // Assert
            Assert.Equal(19, actual.Value);
            Assert.Equal("delayed", actual.DisplayName);
        }

        [Fact]
        public void EndedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Ended;

            // Assert
            Assert.Equal(22, actual.Value);
            Assert.Equal("ended", actual.DisplayName);
        }

        [Fact]
        public void ClosedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Closed;

            // Assert
            Assert.Equal(23, actual.Value);
            Assert.Equal("closed", actual.DisplayName);
        }

        [Fact]
        public void HalftimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.Halftime;

            // Assert
            Assert.Equal(16, actual.Value);
            Assert.Equal("halftime", actual.DisplayName);
        }

        [Fact]
        public void FullTimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.FullTime;

            // Assert
            Assert.Equal(17, actual.Value);
            Assert.Equal("full-time", actual.DisplayName);
        }

        [Fact]
        public void ExtraTimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.ExtraTime;

            // Assert
            Assert.Equal(18, actual.Value);
            Assert.Equal("extra_time", actual.DisplayName);
        }

        [Fact]
        public void EndedExtraTimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.EndedExtraTime;

            // Assert
            Assert.Equal(24, actual.Value);
            Assert.Equal("aet", actual.DisplayName);
        }

        [Fact]
        public void EndedAfterPenaltiesStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.EndedAfterPenalties;

            // Assert
            Assert.Equal(25, actual.Value);
            Assert.Equal("ap", actual.DisplayName);
        }

        [Fact]
        public void StartDelayedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.StartDelayed;

            // Assert
            Assert.Equal(3, actual.Value);
            Assert.Equal("start_delayed", actual.DisplayName);
        }
    }
}
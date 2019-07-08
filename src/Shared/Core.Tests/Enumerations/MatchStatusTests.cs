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
            var actual = MatchStatus.NotStartedStatus;

            // Assert
            Assert.Equal("not_started", actual.Value);
            Assert.Equal("NotStarted", actual.DisplayName);
        }

        [Fact]
        public void LiveStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.LiveStatus;

            // Assert
            Assert.Equal("live", actual.Value);
            Assert.Equal("Live", actual.DisplayName);
        }

        [Fact]
        public void FirstHaftStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.FirstHaftStatus;

            // Assert
            Assert.Equal("1st_half", actual.Value);
            Assert.Equal("FirstHaft", actual.DisplayName);
        }

        [Fact]
        public void SecondHaftStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.SecondHaftStatus;

            // Assert
            Assert.Equal("2nd_half", actual.Value);
            Assert.Equal("SecondHaft", actual.DisplayName);
        }

        [Fact]
        public void OvertimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.OvertimeStatus;

            // Assert
            Assert.Equal("overtime", actual.Value);
            Assert.Equal("Overtime", actual.DisplayName);
        }

        [Fact]
        public void FirstHaftExtraStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.FirstHaftExtraStatus;

            // Assert
            Assert.Equal("1st_extra", actual.Value);
            Assert.Equal("FirstHaftExtra", actual.DisplayName);
        }

        [Fact]
        public void SecondHaftExtraStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.SecondHaftExtraStatus;

            // Assert
            Assert.Equal("2nd_extra", actual.Value);
            Assert.Equal("SecondHaftExtra", actual.DisplayName);
        }

        [Fact]
        public void AwaitingPenaltiesStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.AwaitingPenaltiesStatus;

            // Assert
            Assert.Equal("awaiting_penalties", actual.Value);
            Assert.Equal("AwaitingPenalties", actual.DisplayName);
        }

        [Fact]
        public void PenaltiesStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.PenaltiesStatus;

            // Assert
            Assert.Equal("penalties", actual.Value);
            Assert.Equal("Penalties", actual.DisplayName);
        }

        [Fact]
        public void PauseStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.PauseStatus;

            // Assert
            Assert.Equal("pause", actual.Value);
            Assert.Equal("Pause", actual.DisplayName);
        }

        [Fact]
        public void AwaitingExtraTimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.AwaitingExtraTimeStatus;

            // Assert
            Assert.Equal("awaiting_extra_time", actual.Value);
            Assert.Equal("AwaitingExtraTime", actual.DisplayName);
        }

        [Fact]
        public void InterruptedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.InterruptedStatus;

            // Assert
            Assert.Equal("interrupted", actual.Value);
            Assert.Equal("Interrupted", actual.DisplayName);
        }

        [Fact]
        public void AbandonedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.AbandonedStatus;

            // Assert
            Assert.Equal("abandoned", actual.Value);
            Assert.Equal("Abandoned", actual.DisplayName);
        }

        [Fact]
        public void PostponedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.PostponedStatus;

            // Assert
            Assert.Equal("postponed", actual.Value);
            Assert.Equal("Postponed", actual.DisplayName);
        }

        [Fact]
        public void DelayedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.DelayedStatus;

            // Assert
            Assert.Equal("delayed", actual.Value);
            Assert.Equal("Delayed", actual.DisplayName);
        }

        [Fact]
        public void EndedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.EndedStatus;

            // Assert
            Assert.Equal("ended", actual.Value);
            Assert.Equal("Ended", actual.DisplayName);
        }

        [Fact]
        public void ClosedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.ClosedStatus;

            // Assert
            Assert.Equal("closed", actual.Value);
            Assert.Equal("Closed", actual.DisplayName);
        }

        [Fact]
        public void HalftimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.HalftimeStatus;

            // Assert
            Assert.Equal("halftime", actual.Value);
            Assert.Equal("Halftime", actual.DisplayName);
        }

        [Fact]
        public void FullTimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.FullTimeStatus;

            // Assert
            Assert.Equal("full-time", actual.Value);
            Assert.Equal("FullTime", actual.DisplayName);
        }

        [Fact]
        public void ExtraTimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.ExtraTimeStatus;

            // Assert
            Assert.Equal("extra_time", actual.Value);
            Assert.Equal("ExtraTime", actual.DisplayName);
        }

        [Fact]
        public void EndedExtraTimeStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.EndedExtraTimeStatus;

            // Assert
            Assert.Equal("aet", actual.Value);
            Assert.Equal("EndedExtraTime", actual.DisplayName);
        }

        [Fact]
        public void EndedAfterPenaltiesStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.EndedAfterPenaltiesStatus;

            // Assert
            Assert.Equal("ap", actual.Value);
            Assert.Equal("EndedAfterPenalties", actual.DisplayName);
        }

        [Fact]
        public void StartDelayedStatus_Always_CreateCorrectType()
        {
            // Act
            var actual = MatchStatus.StartDelayedStatus;

            // Assert
            Assert.Equal("start_delayed", actual.Value);
            Assert.Equal("StartDelayed", actual.DisplayName);
        }
    }
}
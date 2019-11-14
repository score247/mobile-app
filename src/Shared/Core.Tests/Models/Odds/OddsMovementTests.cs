using System;
using AutoFixture;
using LiveScore.Core.Models.Odds;
using LiveScore.Core.Tests.Fixtures;
using Xunit;

namespace LiveScore.Core.Tests.Models.Odds
{
    public class OddsMovementTests
    {
        private readonly Fixture fixture;

        public OddsMovementTests() 
        {
            fixture = new Fixture();
        }

        [Fact]
        public void Equals_Null_ReturnFalse() 
        {
            var oddsMovement = fixture.Create<OddsMovement>();

            var result = oddsMovement.Equals(null);

            Assert.False(result);
        }

        [Fact]
        public void Equals_DifferentType_ReturnFalse()
        {
            var oddsMovement1 = fixture.Create<OddsMovement>();
            var bookmaker = fixture.Create<Bookmaker>();

            var result = oddsMovement1.Equals(bookmaker);

            Assert.False(result);
        }

        [Fact]
        public void Equals_DifferentValue_ReturnFalse()
        {
            var oddsMovement1 = fixture.Create<OddsMovement>();
            var oddsMovement2 = fixture.Create<OddsMovement>();

            var result = oddsMovement1.Equals(oddsMovement2);

            Assert.False(result);
        }

        [Fact]
        public void Equals_UpdateTimeAndMatchTime_ReturnTrue()
        {
            var updateTime = DateTimeOffset.Now;
            var oddsMovement1 = fixture.Create<OddsMovement>()
                .With(movement => movement.UpdateTime, updateTime)
                .With(movement => movement.MatchTime, "3'");

            var oddsMovement2 = fixture.Create<OddsMovement>()
                .With(movement => movement.UpdateTime, updateTime)
                .With(movement => movement.MatchTime, "3'");

            var result = oddsMovement1.Equals(oddsMovement2);

            Assert.True(result);
        }

        [Fact]
        public void EqualsOperator_UpdateTimeAndMatchTime_ReturnTrue()
        {
            var updateTime = DateTimeOffset.Now;
            var oddsMovement1 = fixture.Create<OddsMovement>()
                .With(movement => movement.UpdateTime, updateTime)
                .With(movement => movement.MatchTime, "3'");

            var oddsMovement2 = fixture.Create<OddsMovement>()
                .With(movement => movement.UpdateTime, updateTime)
                .With(movement => movement.MatchTime, "3'");

            var result = oddsMovement1 == oddsMovement2;

            Assert.True(result);
        }

        [Fact]
        public void NotEqualOperator_DifferentValue_ReturnTrue()
        {
            var oddsMovement1 = fixture.Create<OddsMovement>();
            var oddsMovement2 = fixture.Create<OddsMovement>();

            var result = oddsMovement1 != oddsMovement2;

            Assert.True(result);
        }

        [Fact]
        public void NotEqualOperator_Null_ReturnTrue()
        {
            var oddsMovement1 = fixture.Create<OddsMovement>();

            var result = oddsMovement1 != null;

            Assert.True(result);
        }
    }
}

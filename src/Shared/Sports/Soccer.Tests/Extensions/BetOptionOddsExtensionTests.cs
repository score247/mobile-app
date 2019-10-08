using System.Globalization;
using AutoFixture;
using LiveScore.Soccer.Extensions;
using Xunit;

namespace Soccer.Tests.Extensions
{
    public class BetOptionOddsExtensionTests
    {
        private const string OddsNumberFormat = "0.00";
        private readonly Fixture fixture;

        public BetOptionOddsExtensionTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void String_ToOddsFormat_Always_ExpectedValue()
        {
            var odd = fixture.Create<decimal>();

            var actual = odd.ToOddsFormat();

            Assert.Equal(odd.ToString(OddsNumberFormat), actual);
        }

        [Fact]
        public void String_ToOddsOptionFormat_CannotParseInputStringToFloat_ReturnInputString()
        {
            string odd = "a string is not a float";

            var actual = odd.ToOddsOptionFormat();

            Assert.Equal(odd, actual);
        }

        [Fact]
        public void String_ToOddsOptionFormat_ParseInputStringToInvariantCulture()
        {
            string odd = fixture.Create<float>().ToString();

            var actual = odd.ToOddsOptionFormat();

            Assert.Equal(odd.ToString(CultureInfo.InvariantCulture), actual);
        }
    }
}
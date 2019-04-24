using LiveScore.Common.Services;
using NSubstitute;
using System;
using Xunit;

namespace LiveScore.Common.Tests.Services
{
    public class LocalStorageTests
    {
        const string DATE_FORMAT = "yyyy-MM-ddTHH:mm:sszzz";

        private readonly IEssentialsService mockEssentials;
        private readonly LocalStorage cache;

        public LocalStorageTests()
        {
            mockEssentials = Substitute.For<IEssentialsService>();

            cache = new LocalStorage(mockEssentials);
        }

        [Fact]
        public void CacheDuration_LongTerm_ReturnCorrectValue()
        {
            // Arrange
            var expectedDuration = DateTime.Now.AddMinutes(120);

            // Act            
            var actual = cache.CacheDuration(CacheDurationTerm.Long);

            // Assert
            Assert.Equal(expectedDuration.ToString(DATE_FORMAT), actual.ToString(DATE_FORMAT));
        }

        [Fact]
        public void CacheDuration_ShortTerm_ReturnCorrectValue()
        {
            // Arrange
            var expectedDuration = DateTime.Now.AddMinutes(2);

            // Act            
            var actual = cache.CacheDuration(CacheDurationTerm.Short);

            // Assert
            Assert.Equal(expectedDuration.ToString(DATE_FORMAT), actual.ToString(DATE_FORMAT));
        }
    }
}

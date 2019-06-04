namespace LiveScore.Common.Tests.Configuration
{
    using LiveScore.Common.Configuration;
    using Xunit;

    public class ConfigurationTests
    {
        [Fact]
        public void LocalEndPoint_Always_GetExpectedValue()
        {
            // Assert
            Assert.Equal("https://api.nexdev.net/V1/api/", Configuration.LocalEndPoint);
        }

        [Fact]
        public void SentryDsn_Always_GetExpectedValue()
        {
            // Assert
            Assert.Equal("https://a75e3e7b51ea4de8baa2c27b67bbede3@sentry.nexdev.net/34", Configuration.SentryDsn);
        }
    }
}
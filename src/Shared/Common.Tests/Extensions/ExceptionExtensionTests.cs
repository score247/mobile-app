using System;
using System.Threading.Tasks;
using AutoFixture;
using LiveScore.Common.Extensions;
using Xunit;

namespace LiveScore.Common.Tests.Extensions
{
    public class ExceptionExtensionTests
    {
        private readonly Fixture fixture;

        public ExceptionExtensionTests()
        {
            fixture = new Fixture();
        }

        [Fact]
        public void IsIgnore_OperationCanceledException_ReturnsTrue()
        {
            var exception = fixture.Create<OperationCanceledException>();

            Assert.True(exception.IsIgnore());
        }

        [Fact]
        public void IsIgnore_TaskCanceledException_ReturnsTrue()
        {
            var exception = fixture.Create<TaskCanceledException>();

            Assert.True(exception.IsIgnore());
        }

        [Fact]
        public void IsIgnore_InvalidOperationException_ReturnsFalse()
        {
            var exception = fixture.Create<InvalidOperationException>();

            Assert.False(exception.IsIgnore());
        }
    }
}

using LiveScore.Core;
using NSubstitute;
using Prism.Events;
using Prism.Navigation;
using Xunit;

namespace Soccer.Tests.ViewModels.Leagues
{
    public class LeagueDetailViewModelTests
    {
        private readonly INavigationService subNavigationService;
        private readonly IDependencyResolver subDependencyResolver;
        private readonly IEventAggregator subEventAggregator;

        public LeagueDetailViewModelTests()
        {
            this.subNavigationService = Substitute.For<INavigationService>();
            this.subDependencyResolver = Substitute.For<IDependencyResolver>();
            this.subEventAggregator = Substitute.For<IEventAggregator>();
        }

        [Fact]
        public void Initialize_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
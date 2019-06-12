namespace Soccer.Tests.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Core.Services;
    using LiveScore.Soccer.ViewModels;
    using NSubstitute;
    using Prism.Events;
    using Prism.Navigation;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class MatchDetailViewModelTests
    {
        private readonly INavigationService subNavigationService;
        private readonly IDependencyResolver subDependencyResolver;
        private readonly IEventAggregator subEventAggregator;
        private readonly IHubService subHubService;

        public MatchDetailViewModelTests()
        {
            subNavigationService = Substitute.For<INavigationService>();
            subDependencyResolver = Substitute.For<IDependencyResolver>();
            subEventAggregator = Substitute.For<IEventAggregator>();
            subHubService = Substitute.For<IHubService>();
        }

        [Fact]
        public void OnNavigatingTo_ParametersIsNotNull_BuildMatchViewModel()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task OnNavigatedTo_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
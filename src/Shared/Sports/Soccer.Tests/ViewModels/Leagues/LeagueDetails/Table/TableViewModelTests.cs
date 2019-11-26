using LiveScore.Core;
using NSubstitute;
using Prism.Navigation;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.ViewModels.Leagues.LeagueDetails.Table
{
    public class TableViewModelTests
    {
        private readonly INavigationService subNavigationService;
        private readonly IDependencyResolver subDependencyResolver;
        private readonly DataTemplate subDataTemplate;

        public TableViewModelTests()
        {
            this.subNavigationService = Substitute.For<INavigationService>();
            this.subDependencyResolver = Substitute.For<IDependencyResolver>();
            this.subDataTemplate = Substitute.For<DataTemplate>();
        }

        [Fact]
        public void TestMethod1()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
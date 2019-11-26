using LiveScore.Core;
using LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table;
using NSubstitute;
using Prism.Navigation;
using System;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.ViewModels.Leagues.LeagueDetails.Table
{
    public class TableViewModelTests
    {
        private INavigationService subNavigationService;
        private IDependencyResolver subDependencyResolver;
        private DataTemplate subDataTemplate;

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
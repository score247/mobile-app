namespace LiveScore.Core.Tests.ViewModels
{
    using LiveScore.Core;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Core.ViewModels;
    using Prism.Navigation;
    using Xunit;

    internal class MockViewModel : ViewModelBase
    {
        public MockViewModel(
            INavigationService navigationService,
            IDependencyResolver depdendencyResolver)
            : base(navigationService, depdendencyResolver)
        {
            Title = "Title";
        }
    }

    public class ViewModelBaseTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly MockViewModel viewModel;

        public ViewModelBaseTests(ViewModelBaseFixture baseFixture)
        {
            viewModel = new MockViewModel(baseFixture.NavigationService, baseFixture.DepdendencyResolver);
        }

        [Fact]
        public void Title_Always_GetExpectedValue()
        {
            // Act
            var actual = viewModel.Title;

            // Assert
            Assert.Equal("Title", actual);
        }

        [Fact]
        public void OnNavigatedFrom_DoNothing()
        {
            // Act
            viewModel.OnNavigatedFrom(null);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void OnNavigatedTo_StateUnderTest_ExpectedBehavior()
        {
            // Act
            viewModel.OnNavigatedTo(null);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void OnNavigatingTo_StateUnderTest_ExpectedBehavior()
        {
            // Act
            viewModel.OnNavigatingTo(null);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void Destroy_StateUnderTest_ExpectedBehavior()
        {
            // Act
            viewModel.Destroy();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void OnResume_StateUnderTest_ExpectedBehavior()
        {
            // Act
            viewModel.OnResume();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void OnSleep_StateUnderTest_ExpectedBehavior()
        {
            // Act
            viewModel.OnSleep();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void OnAppearing_StateUnderTest_ExpectedBehavior()
        {
            // Act
            viewModel.OnAppearing();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void OnDisappearing_StateUnderTest_ExpectedBehavior()
        {
            // Act
            viewModel.OnDisappearing();

            // Assert
            Assert.True(true);
        }
    }
}
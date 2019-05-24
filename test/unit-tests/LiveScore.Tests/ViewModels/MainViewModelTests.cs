using LiveScore.Core;
using LiveScore.ViewModels;
using NSubstitute;
using Prism.Navigation;

namespace LiveScore.Tests.ViewModels
{
    public class MainViewModelTests
    {
        private readonly INavigationService mockNavigation;
        private readonly IDependencyResolver mockResolver;

        private readonly MainViewModel viewModel;

        public MainViewModelTests()
        {
            mockNavigation = Substitute.For<INavigationService>();
            mockResolver = Substitute.For<IDependencyResolver>();

            viewModel = new MainViewModel(mockNavigation, mockResolver);
        }
    }
}

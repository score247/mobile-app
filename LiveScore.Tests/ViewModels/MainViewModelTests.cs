using LiveScore.Core;
using LiveScore.ViewModels;
using NSubstitute;
using Prism.Navigation;
using System.Threading.Tasks;
using Xunit;

namespace LiveScore.Tests.ViewModels
{
    public class MainViewModelTests
    {
        private readonly INavigationService mockNavigation;
        private readonly IDepdendencyResolver mockResolver;

        private readonly MainViewModel viewModel;

        public MainViewModelTests()
        {
            mockNavigation = Substitute.For<INavigationService>();
            mockResolver = Substitute.For<IDepdendencyResolver>();

            viewModel = new MainViewModel(mockNavigation, mockResolver);
        }
    }
}

using LiveScore.Core;
using NSubstitute;

namespace Soccer.Tests.ViewModels.DetailLineups
{
    public class LineupsPlayerViewModelTests
    {
        private readonly IDependencyResolver denpendacyResolver;

        public LineupsPlayerViewModelTests()
        {
            denpendacyResolver = Substitute.For<IDependencyResolver>();
        }
    }
}
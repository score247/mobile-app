using System.Collections.Generic;
using NSubstitute;
using Prism.Ioc;
using Xunit;

namespace LiveScore.Core.Tests
{
    public class dependencyResolverTests
    {
        private readonly IContainerProvider subContainerProvider;
        private readonly IDependencyResolver dependencyResolver;

        public dependencyResolverTests()
        {
            subContainerProvider = Substitute.For<IContainerProvider>();
            dependencyResolver = new DependencyResolver(subContainerProvider);
        }

        [Fact]
        public void Resolve_Always_CallResolveFromContainerProvider()
        {
            // Act
            dependencyResolver.Resolve<IList<int>>();

            // Assert
            subContainerProvider.Received(1).Resolve<IList<int>>();
        }

        [Fact]
        public void ResolveWithName_Always_CallResolveFromContainerProvider()
        {
            // Act
            dependencyResolver.Resolve<IList<int>>("1");

            // Assert
            subContainerProvider.ReceivedWithAnyArgs(1).Resolve<IList<int>>("1");
        }
    }
}
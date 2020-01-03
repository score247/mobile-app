using System.Collections.Generic;
using NSubstitute;
using Prism.Ioc;
using Xunit;

namespace LiveScore.Core.Tests
{
    public class DependencyResolverTests
    {
        private readonly IContainerProvider subContainerProvider;
        private readonly IDependencyResolver dependencyResolver;

        public DependencyResolverTests()
        {
            subContainerProvider = Substitute.For<IContainerProvider>();
            dependencyResolver = new DependencyResolver(subContainerProvider);
        }

#pragma warning disable S2699 // Tests should include assertions

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

#pragma warning restore S2699 // Tests should include assertions
    }
}
using System;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Caching;
using LiveScore.Common.Services;
using NSubstitute;
using Xunit;

namespace LiveScore.Common.Tests.Services
{
    public class CacheManagerTests
    {
        private readonly ICacheService cacheService;
        private readonly ICacheManager cacheManager;
        private readonly ILoggingService loggingService;
        private readonly Fixture Any;
        private readonly CacheItemOptions cacheItemOptions;
        private readonly string CacheKey;

        public CacheManagerTests()
        {
            loggingService = Substitute.For<ILoggingService>();
            cacheService = Substitute.For<ICacheService>();
            cacheManager = new CacheManager(cacheService, loggingService);
            Any = new Fixture();

            cacheItemOptions = Any.Create<CacheItemOptions>();
            CacheKey = Any.Create<string>();
        }

        [Fact]
        public async Task GetOrSetAsync_CacheKeyIsNullOrWhiteSpace_ReturnsDefault()
        {
            // Arrange
            var factory = Substitute.For<Func<Task<object>>>();

            // Act
            var actual = await cacheManager.GetOrSetAsync(key: string.Empty, factory, cacheItemOptions, true);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task GetOrSetAsync_ForceFetchLatestDataIsTrue_AlwaysInvokeFactory()
        {
            // Arrange
            var factory = Substitute.For<Func<Task<object>>>();

            // Act
            await cacheManager.GetOrSetAsync(CacheKey, factory, cacheItemOptions, forceFetchLatestData: true);

            // Assert
            await factory.Received().Invoke();
        }

        [Fact]
        public async Task GetOrSetAsync_ForceFetchLatestDataIsTrue_AlwaysInvokeCachingSetAsync()
        {
            // Arrange
            var value = Any.Create<int>();
            Task<int> factory() => Task.FromResult(value);

            // Act
            await cacheManager.GetOrSetAsync(CacheKey, factory, cacheItemOptions, forceFetchLatestData: true);

            // Assert
            await cacheService.Received().SetAsync(CacheKey, value, cacheItemOptions);
        }

        [Fact]
        public async Task GetOrSetAsync_ForceFetchLatestDataIsFalse_FanexServiceInvokesGetAsync()
        {
            // Arrange
            Task<int> factory() => Task.FromResult(Any.Create<int>());

            // Act
            await cacheManager.GetOrSetAsync(CacheKey, factory, cacheItemOptions, forceFetchLatestData: false);

            // Assert
            await cacheService.Received().GetAsync<int>(CacheKey);
        }

        [Fact]
        public async Task GetOrSetAsync_DataFromCacheIsNull_InvokeFactory()
        {
            // Arrange
            var factory = Substitute.For<Func<Task<object>>>();
            object dataFromCache = null;
            cacheService.GetAsync<object>(CacheKey).Returns(Task.FromResult(dataFromCache));

            // Act
            await cacheManager.GetOrSetAsync(CacheKey, factory, cacheItemOptions, forceFetchLatestData: false);

            // Assert
            await factory.Received().Invoke();
        }

        [Fact]
        public async Task GetOrSetAsync_DataIsInCache_ReturnTheData()
        {
            // Arrange
            var value = Any.Create<object>();
            var factory = Substitute.For<Func<Task<object>>>();
            cacheService.GetAsync<object>(CacheKey).Returns(Task.FromResult(value));

            // Act
            var actual = await cacheManager.GetOrSetAsync(CacheKey, factory, cacheItemOptions, forceFetchLatestData: false);

            // Assert
            Assert.Equal(value, actual);
        }

        [Fact]
        public async Task GetOrSetAsync_TaskCanceledExceptionIsThrown_InvokeLogExceptionAsync()
        {
            // Arrange
            var exception = new TaskCanceledException();
            Task<int> factory() => throw exception;

            // Act
            await cacheManager.GetOrSetAsync(CacheKey, factory, cacheItemOptions, forceFetchLatestData: true);

            // Assert
            await loggingService.Received().LogExceptionAsync(exception);
        }
    }
}
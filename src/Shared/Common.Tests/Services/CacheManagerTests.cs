using System;
using System.Threading.Tasks;
using Fanex.Caching;
using LiveScore.Common.Services;
using NSubstitute;
using Xunit;

namespace LiveScore.Common.Tests.Services
{
    public class CacheManagerTests
    {
        private readonly INetworkConnection subNetworkConnectionManager;
        private readonly ICacheService cacheService;
        private ICacheManager cacheManager;
        private ILoggingService loggingService;

        public CacheManagerTests()
        {
            subNetworkConnectionManager = Substitute.For<INetworkConnection>();
            loggingService = Substitute.For<ILoggingService>();
            cacheService = new CacheService();
            cacheManager = new CacheManager(cacheService, subNetworkConnectionManager, loggingService);
        }

        [Fact]
        public async Task GetOrSetAsync_ConnectionIsOk_GetLatestData_CallFactoryMethodAndSetDataToCache()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = new Func<Task<int>>(() => Task.FromResult(1));

            // Act
            var result = await cacheManager.GetOrSetAsync("key1", factory, new CacheItemOptions(), true);

            // Assert
            Assert.Equal(1, result);
            Assert.Equal(1, cacheService.Get<int>("key1"));
        }

        [Fact]
        public async Task GetOrSetAsync_ConnectionIsOk_NotGetLatestData_NotHaveCachedData_CallFactoryMethodAndSetDataToCache()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = new Func<Task<int>>(() => Task.FromResult(1));

            // Act
            var result = await cacheManager.GetOrSetAsync("key2", factory, new CacheItemOptions(), false);

            // Assert
            Assert.Equal(1, result);
            Assert.Equal(1, cacheService.Get<int>("key2"));
        }

        [Fact]
        public async Task GetOrSetAsync_ConnectionIsOk_NotGetLatestData_HaveCachedData_ReturnDataFromCache()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = new Func<Task<int>>(() => Task.FromResult(1));
            cacheService.Set("key3", 1);

            // Act
            var result = await cacheManager.GetOrSetAsync("key3", factory, new CacheItemOptions(), false);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetOrSetAsync_ConnectionIsOk_GetLatestData_GetTimeOutException_ReturnDataFromCacheAndPublishConnectionTimeoutEvent()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = Substitute.For<Func<Task<int>>>();
            factory.When(x => x.Invoke()).Throw(new TaskCanceledException());
            cacheService.Set("key4", 2);

            // Act
            var result = await cacheManager.GetOrSetAsync("key4", factory, new CacheItemOptions(), true);

            // Assert
            Assert.Equal(2, result);
            subNetworkConnectionManager.Received(1).PublishConnectionTimeoutEvent();
        }

        [Fact]
        public async Task GetOrSetAsync_ConnectionIsOk_GetLatestData_GetOtherException_ThrowException()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = Substitute.For<Func<Task<int>>>();
            factory.When(x => x.Invoke()).Throw(new Exception());
            cacheService.Set("key5", 2);

            // Assert
            await Assert.ThrowsAsync<Exception>(() => cacheManager.GetOrSetAsync("key5", factory, new CacheItemOptions(), true));
        }

        [Fact]
        public async Task GetOrSetAsync_ConnectionIsOk_NotGetLatestData_NotHaveCachedData_GetTimeOutException_ReturnDefaultValueAndPublishConnectionTimeoutEvent()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = Substitute.For<Func<Task<int>>>();
            factory.When(x => x.Invoke()).Throw(new TaskCanceledException());

            // Act
            var result = await cacheManager.GetOrSetAsync("key6", factory, new CacheItemOptions(), false);

            // Assert
            Assert.Equal(0, result);
            subNetworkConnectionManager.Received(1).PublishConnectionTimeoutEvent();
        }

        [Fact]
        public async Task GetOrSetAsync_ConnectionIsOk_NotGetLatestData_NotHaveCachedData_GetOtherException_ThrowException()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = Substitute.For<Func<Task<int>>>();
            factory.When(x => x.Invoke()).Throw(new Exception());

            // Assert
            await Assert.ThrowsAsync<Exception>(() => cacheManager.GetOrSetAsync("key7", factory, new CacheItemOptions(), false));
        }

        [Fact]
        public async Task GetOrSetAsync_ConnectionIsNotOk_PublishNetworkConnectionEventAndReturnDataFromCache()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(false);
            var factory = new Func<Task<int>>(() => Task.FromResult(1));
            cacheService.Set("key8", 2);

            // Act
            var result = await cacheManager.GetOrSetAsync("key8", factory, new CacheItemOptions(), false);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetOrSetAsync_FactoryReturnNull_NotSetToCache()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = new Func<Task<string>>(() => Task.FromResult<string>(null));
            cacheService.Set("key9", 2);
            var cacheOptions = new CacheItemOptions();

            // Act
            var result = await cacheManager.GetOrSetAsync("key9", factory, cacheOptions, true);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetOrSetAsync_SetAbsoluteExpiredTime_ExpectDataFromCache()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = new Func<Task<int>>(() => Task.FromResult(2));

            // Act
            await cacheManager.GetOrSetAsync("key10", factory, 120, true);

            // Assert
            Assert.Equal(2, cacheService.Get<int>("key10"));
        }

        [Fact]
        public async Task GetOrSetAsync_WithFuncNotReturnTask_ExpectDataFromCache()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = new Func<int>(() => 2);

            // Act
            await cacheManager.GetOrSetAsync("key11", factory, new CacheItemOptions(), true);

            // Assert
            Assert.Equal(2, cacheService.Get<int>("key11"));
        }

        [Fact]
        public async Task InvalidateAll_RemoveAllCache()
        {
            // Arrange
            subNetworkConnectionManager.IsSuccessfulConnection().Returns(true);
            var factory = new Func<Task<string>>(() => Task.FromResult("2"));
            await cacheManager.GetOrSetAsync("key12", factory, new CacheItemOptions(), true);
            await cacheManager.GetOrSetAsync("key13", factory, new CacheItemOptions(), true);

            // Act
            await cacheManager.InvalidateAll();

            // Assert
            Assert.Null(cacheService.Get<string>("key12"));
            Assert.Null(cacheService.Get<string>("key13"));
        }
    }
}
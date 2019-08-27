﻿namespace LiveScore.Common.Tests.Services
{
    using System.Threading.Tasks;
    using Akavache;
    using LiveScore.Common.Services;
    using NSubstitute;
    using Xunit;

    public class LocalStorageTests
    {
        private readonly IEssential mockEssentials;
        private readonly IBlobCache mockLocalMachine;
        private readonly IBlobCache mockUserAccount;
        private readonly IBlobCache mockInMemory;

        private readonly CachingService cache;

        public LocalStorageTests()
        {
            mockEssentials = Substitute.For<IEssential>();
            mockLocalMachine = Substitute.For<IBlobCache>();
            mockUserAccount = Substitute.For<IBlobCache>();
            mockInMemory = Substitute.For<IBlobCache>();

            cache = new CachingService(mockEssentials, mockLocalMachine, mockUserAccount, mockInMemory);
        }

        [Fact]
        public async Task CleanAllExpired_ShouldInjectBlobCache()
        {
            // Arrange

            // Act
            await cache.CleanAllExpired();

            // Assert
            mockLocalMachine.Received(1).Vacuum();
        }

        [Fact]
        public async Task Invalidate_ShouldInjectBlobCache()
        {
            // Arrange
            const string imageLink = "https://country.flags";

            // Act
            await cache.Invalidate(imageLink);

            // Assert
            mockLocalMachine.Received(1).Invalidate(Arg.Any<string>());
        }

        [Fact]
        public void Shutdown_LocalMachineShouldFlush()
        {
            // Arrange

            // Act
            cache.Shutdown();

            // Assert
            mockLocalMachine.Received(1).Flush();
        }

        [Fact]
        public void Shutdown_UserAccountShouldFlush()
        {
            // Arrange

            // Act
            cache.Shutdown();

            // Assert
            mockUserAccount.Received(1).Flush();
        }

        [Fact]
        public async Task GetAndFetchLatestValue_Always_CallLocalMachineCache()
        {
            // Arrange
            Task<MockModel> fetchFunc() => Task.FromResult(new MockModel());

            // Act
            await cache.GetAndFetchLatestValue("cacheKey", fetchFunc);

            // Assert
            mockLocalMachine.Received(1).GetAndFetchLatest("cacheKey", fetchFunc);
        }

        [Fact]
        public async Task GetOrFetchValue_Always_CallLocalMachineCache()
        {
            // Arrange
            Task<MockModel> fetchFunc() => Task.FromResult(new MockModel());

            // Act
            await cache.GetOrFetchValue("cacheKey", fetchFunc);

            // Assert
            mockLocalMachine.Received(1).GetOrFetchObject("cacheKey", fetchFunc);
        }

        [Fact]
        public void AddOrUpdateValue_Always_CallLocalMachineCache()
        {
            // Arrange

            // Act
            cache.AddOrUpdateValueToUserAccount("cacheKey", 1);

            // Assert
            mockUserAccount.ReceivedWithAnyArgs(1).InsertObject("cacheKey", 1);
        }

        [Fact]
        public void GetValueOrDefault_Always_CallLocalMachineCache()
        {
            // Arrange

            // Act
            cache.GetValueOrDefaultFromUserAccount("cacheKey", 1);

            // Assert
            mockUserAccount.ReceivedWithAnyArgs(1).GetOrCreateObject("cacheKey", () => 1);
        }
    }

    public class MockModel { }
}
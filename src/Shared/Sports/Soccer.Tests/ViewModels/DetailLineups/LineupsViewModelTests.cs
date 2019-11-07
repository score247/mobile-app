using System;
using System.Threading.Tasks;
using AutoFixture;
using LiveScore.Common;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Enumerations;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Models.Teams;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.MatchDetails.LineUps;
using NSubstitute;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailLineups
{
    public class LineupsViewModelTests
    {
        private readonly ISoccerMatchService soccerMatchService;
        private readonly IDeviceInfo deviceInfo;
        private readonly string matchId = "match_id";
        private readonly Action<Action> beginInvokeOnMainThreadFunc;
        private readonly LineupsViewModel lineupsViewModel;
        private readonly INavigationService navigationService;
        private readonly IDependencyResolver serviceLocator;
        private readonly IEventAggregator eventAggregator;
        private readonly ISettings settings;
        private readonly Fixture fixture;

        public LineupsViewModelTests()
        {
            navigationService = Substitute.For<INavigationService>();
            serviceLocator = Substitute.For<IDependencyResolver>();
            eventAggregator = Substitute.For<IEventAggregator>();
            soccerMatchService = Substitute.For<ISoccerMatchService>();
            deviceInfo = Substitute.For<IDeviceInfo>();
            settings = Substitute.For<ISettings>();
            var networkConnectionManager = Substitute.For<INetworkConnection>();
            networkConnectionManager.IsSuccessfulConnection().Returns(true);

            beginInvokeOnMainThreadFunc = ac => ac();

            serviceLocator.Resolve<ISoccerMatchService>().Returns(soccerMatchService);
            serviceLocator.Resolve<IDeviceInfo>().Returns(deviceInfo);
            serviceLocator.Resolve<Action<Action>>(FuncNameConstants.BeginInvokeOnMainThreadFuncName).Returns(beginInvokeOnMainThreadFunc);
            serviceLocator.Resolve<INetworkConnection>().Returns(networkConnectionManager);
            serviceLocator.Resolve<ISettings>().Returns(settings);
            settings.CurrentSportType.Returns(SportType.Soccer);
            fixture = new Fixture();

            lineupsViewModel = new LineupsViewModel(matchId, navigationService, serviceLocator, eventAggregator, new DataTemplate());
        }

        [Fact]
        public async Task LoadLineUpsAsync_LineupIsEmpty_SetHasDataToFalse()
        {
            await lineupsViewModel.LoadLineUpsAsync();

            Assert.False(lineupsViewModel.HasData);
        }

        [Fact]
        public async Task LoadLineUpsAsync_LineupIsEmpty_SetHasDataToTrue()
        {
            var matchLineups = fixture.Create<MatchLineups>();
            soccerMatchService.GetMatchLineupsAsync(matchId, Language.English).Returns(Task.FromResult(matchLineups));

            await lineupsViewModel.LoadLineUpsAsync();

            Assert.True(lineupsViewModel.HasData);
        }
    }
}
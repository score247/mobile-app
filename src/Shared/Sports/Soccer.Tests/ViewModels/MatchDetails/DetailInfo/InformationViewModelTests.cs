using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using KellermanSoftware.CompareNetObjects;
using LiveScore.Common.Services;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.PubSubEvents.Matches;
using LiveScore.Core.Services;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.Services;
using LiveScore.Soccer.ViewModels.MatchDetails.Information;
using NSubstitute;
using Prism.Events;
using Prism.Navigation;
using Xunit;

namespace Soccer.Tests.ViewModels.MatchDetailInfo
{
    public class InformationViewModelTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly InformationViewModel viewModel;
        private readonly ISoccerMatchService matchService;
        private readonly CompareLogic comparer;
        private readonly IEventAggregator eventAggregator;

        private readonly SoccerMatch match;
        private readonly Fixture fixture;

        public InformationViewModelTests(ViewModelBaseFixture baseFixture)
        {
            comparer = baseFixture.CommonFixture.Comparer;
            fixture = baseFixture.CommonFixture.Specimens;
            matchService = Substitute.For<ISoccerMatchService>();
            var networkConnectionManager = Substitute.For<INetworkConnection>();
            networkConnectionManager.IsSuccessfulConnection().Returns(true);

            baseFixture.DependencyResolver.Resolve<ISoccerMatchService>().Returns(matchService);
            baseFixture.DependencyResolver.Resolve<IHubService>("1").Returns(baseFixture.HubService);
            baseFixture.DependencyResolver.Resolve<INetworkConnection>().Returns(networkConnectionManager);

            eventAggregator = Substitute.For<IEventAggregator>();
            eventAggregator.GetEvent<ConnectionChangePubSubEvent>().Returns(new ConnectionChangePubSubEvent());
            eventAggregator.GetEvent<MatchEventPubSubEvent>().Returns(new MatchEventPubSubEvent());

            match = fixture.Create<SoccerMatch>();

            viewModel = new InformationViewModel(
                match,
                baseFixture.NavigationService,
                baseFixture.DependencyResolver,
                eventAggregator,
                null);

            var parameters = new NavigationParameters { { "Match", match } };
            viewModel.Initialize(parameters);
        }

        [Fact]
        public async Task LoadMatchInfoData_Null_ReturnNull()
        {
            var returnMatch = fixture.Create<MatchInfo>()
                .With(matchInfo => matchInfo.Match, match)
                .With(matchInfo => matchInfo.TimelineEvents, null);

            matchService.GetMatchAsync(match.Id, Language.English, Arg.Any<DateTime>()).Returns(returnMatch);

            // Act
            await viewModel.LoadMatchInfoData();

            Assert.Null(viewModel.InfoItemViewModels);
        }

        [Fact]
        public async Task LoadMatchInfoData_Always_AssignedReferee()
        {
            var returnMatch = fixture.Create<MatchInfo>()
                .With(matchInfo => matchInfo.Match, match);

            matchService.GetMatchAsync(match.Id, Language.English, Arg.Any<DateTime>()).Returns(returnMatch);

            // Act
            await viewModel.LoadMatchInfoData();

            // Assert
            Assert.Equal(returnMatch.Referee, viewModel.DisplayReferee);
        }

        [Fact]
        public async Task LoadMatchInfoData_Always_AssignedVenue()
        {
            var returnMatch = fixture.Create<MatchInfo>()
                .With(matchInfo => matchInfo.Match, match)
                .With(matchInfo => matchInfo.Venue, fixture.Create<Venue>());

            matchService.GetMatchAsync(match.Id, Language.English, Arg.Any<DateTime>()).Returns(returnMatch);

            // Act
            await viewModel.LoadMatchInfoData();

            // Assert
            Assert.Equal(returnMatch.Venue.Name, viewModel.DisplayVenue);
        }

        [Fact]
        public async Task LoadMatchInfoData_AttendanceGreaterThanZero_AssignedAttendance()
        {
            var returnMatch = fixture.Create<MatchInfo>()
                .With(matchInfo => matchInfo.Match, match)
                .With(matchInfo => matchInfo.Attendance, 123456);

            matchService.GetMatchAsync(match.Id, Language.English, Arg.Any<DateTime>()).Returns(returnMatch);

            // Act
            await viewModel.LoadMatchInfoData();

            // Assert
            Assert.Equal(returnMatch.Attendance.ToString("0,0"), viewModel.DisplayAttendance);
        }

        [Fact]
        public async Task LoadMatchInfoData_Always_AssignedEventDate()
        {
            var returnMatch = fixture.Create<MatchInfo>()
                .With(matchInfo => matchInfo.Match, match);

            matchService.GetMatchAsync(match.Id, Language.English, Arg.Any<DateTime>()).Returns(returnMatch);

            // Act
            await viewModel.LoadMatchInfoData();

            // Assert
            Assert.Equal(returnMatch.Match.EventDate.ToString("HH:mm dd MMM, yyyy"), viewModel.DisplayEventDate);
        }

        [Fact]
        public async Task LoadMatchInfoData_NoAttendanceInfo_AssignedAttendance()
        {
            var returnMatch = fixture.Create<MatchInfo>()
                .With(matchInfo => matchInfo.Match, match)
                .With(matchInfo => matchInfo.Attendance, 0);

            matchService.GetMatchAsync(match.Id, Language.English, Arg.Any<DateTime>()).Returns(returnMatch);

            // Act
            await viewModel.LoadMatchInfoData();

            // Assert
            Assert.Empty(viewModel.DisplayAttendance);
        }

        [Fact]
        public async Task LoadMatchInfoData_Always_ReturnCorrectDetailEvents()
        {
            List<TimelineEvent> returnTimelines = StubMainTimelineEvents();

            var returnMatch = fixture.Create<MatchInfo>()
                .With(matchInfo => matchInfo.Match, match)
                .With(matchInfo => matchInfo.TimelineEvents, returnTimelines);

            matchService.GetMatchAsync(match.Id, Language.English, Arg.Any<DateTime>()).Returns(returnMatch);

            // Act
            await viewModel.LoadMatchInfoData();

            var actualInfoItemViewModels = viewModel.InfoItemViewModels;
            Assert.Equal(8, actualInfoItemViewModels.Count);
        }

        private List<TimelineEvent> StubMainTimelineEvents()
        => new List<TimelineEvent>
            {
                StubTimeline(EventType.RedCard, new DateTime(2019, 01, 01, 18, 00, 00)),
                StubTimeline(EventType.Substitution, new DateTime(2019, 01, 01, 17, 15, 00 )),
                StubTimeline(EventType.YellowRedCard, new DateTime(2019, 01, 01, 17, 00, 00 )),
                StubTimeline(EventType.BreakStart, new DateTime(2019, 01, 01, 17, 50, 00 )),
                StubTimeline(EventType.PenaltyMissed, new DateTime(2019, 01, 01, 18, 30, 00 )),
                StubTimeline(EventType.ScoreChange, new DateTime(2019, 01, 01, 17, 55, 00 )),
                StubTimeline(EventType.MatchEnded, new DateTime(2019, 01, 01, 19, 50, 00 )),
                StubTimeline(EventType.PenaltyShootout, new DateTime(2019, 01, 01, 19, 00, 00 ))
            };

        private TimelineEvent StubTimeline(EventType type, DateTime time)
        => fixture.Create<TimelineEvent>()
                .With(timeline => timeline.Type, type)
                .With(timeline => timeline.Time, time);


    }
}
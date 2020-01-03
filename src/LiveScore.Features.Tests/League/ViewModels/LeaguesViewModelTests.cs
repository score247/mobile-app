using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveScore.Core.Enumerations;
using LiveScore.Core.Models.Leagues;
using LiveScore.Core.Services;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Features.League.ViewModels;
using NSubstitute;
using Xunit;

namespace LiveScore.Features.Tests.League.ViewModels
{
    public class LeaguesViewModelTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly ViewModelBaseFixture baseFixture;
        private readonly LeaguesViewModel leaguesViewModel;
        private readonly ILeagueService leagueService;

        public LeaguesViewModelTests(ViewModelBaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
            leagueService = Substitute.For<ILeagueService>();
            baseFixture.DependencyResolver.Resolve<ILeagueService>(default).ReturnsForAnyArgs(leagueService);

            leaguesViewModel = new LeaguesViewModel(baseFixture.NavigationService, baseFixture.DependencyResolver);
        }

        [Fact]
        public void IsActive_Init_False()
        {
            // Assert
            Assert.False(leaguesViewModel.IsActive);
        }

        [Fact]
        public async Task LoadLeagues_NoLeagues_HasDataIsFalse()
        {
            // Arrange
            leagueService.GetMajorLeaguesAsync(Arg.Any<Language>()).Returns(new List<ILeague>());

            // Act
            await leaguesViewModel.LoadLeagues();

            // Assert
            Assert.False(leaguesViewModel.HasData);
        }

        [Fact]
        public async Task LoadLeagues_HasLeagues_HasDataIsTrue()
        {
            // Arrange
            var topLeague = Substitute.For<ILeague>();
            IEnumerable<ILeague> topleagues = new List<ILeague>()
            {
                topLeague
            };
            leagueService.GetMajorLeaguesAsync(default, default).ReturnsForAnyArgs(topleagues);

            // Act
            await leaguesViewModel.LoadLeagues();

            // Assert
            Assert.True(leaguesViewModel.HasData);
        }

        [Fact]
        public async Task LoadLeagues_LeagueMoreThan6_TopLeaguesLimitBy6()
        {
            // Arrange
            var inRangeDate = DateTime.Now.AddDays(-6);
            var seasonDate = new LeagueSeasonDates(DateTime.Now.AddDays(-99), inRangeDate);

            var league1 = Substitute.For<ILeague>();
            league1.Name.Returns("league1");
            league1.SeasonDates.Returns(seasonDate);
            league1.Order.Returns(1);
            var league2 = Substitute.For<ILeague>();
            league2.Name.Returns("league2");
            league2.SeasonDates.Returns(seasonDate);
            league2.Order.Returns(2);
            var league3 = Substitute.For<ILeague>();
            league3.Name.Returns("league3");
            league3.SeasonDates.Returns(seasonDate);
            league3.Order.Returns(3);
            var league4 = Substitute.For<ILeague>();
            league4.Name.Returns("league4");
            league4.SeasonDates.Returns(seasonDate);
            league4.Order.Returns(4);
            var league5 = Substitute.For<ILeague>();
            league5.Name.Returns("league5");
            league5.SeasonDates.Returns(seasonDate);
            league5.Order.Returns(5);
            var league6 = Substitute.For<ILeague>();
            league6.Name.Returns("league6");
            league6.SeasonDates.Returns(seasonDate);
            league6.Order.Returns(6);
            var league7 = Substitute.For<ILeague>();
            league7.Name.Returns("league7");
            league7.SeasonDates.Returns(seasonDate);
            league7.Order.Returns(7);

            IEnumerable<ILeague> topleagues = new List<ILeague>()
            {
                league1,league2,league3,league4,league5,league6,league7
            };
            leagueService.GetMajorLeaguesAsync(default, default).ReturnsForAnyArgs(topleagues);

            // Act
            await leaguesViewModel.LoadLeagues();
            var topLeagueViewModels = leaguesViewModel.LeagueGroups[0].ToList();

            // Assert
            Assert.Equal(6, topLeagueViewModels.Count);
        }

        [Fact]
        public async Task LoadLeagues_LeaguesSeasonEndDateInRange7Days_TopLeaguesOrdeyByOrder()
        {
            // Arrange
            var inRangeDate = DateTime.Now.AddDays(-6);
            var seasonDate = new LeagueSeasonDates(DateTime.Now.AddDays(-99), inRangeDate);

            var league1 = Substitute.For<ILeague>();
            league1.Name.Returns("league1");
            league1.SeasonDates.Returns(seasonDate);
            league1.Order.Returns(1);
            var league2 = Substitute.For<ILeague>();
            league2.Name.Returns("league2");
            league2.SeasonDates.Returns(seasonDate);
            league2.Order.Returns(2);
            var league3 = Substitute.For<ILeague>();
            league3.Name.Returns("league3");
            league3.SeasonDates.Returns(seasonDate);
            league3.Order.Returns(3);
            var league4 = Substitute.For<ILeague>();
            league4.Name.Returns("league4");
            league4.SeasonDates.Returns(seasonDate);
            league4.Order.Returns(4);
            var league5 = Substitute.For<ILeague>();
            league5.Name.Returns("league5");
            league5.SeasonDates.Returns(seasonDate);
            league5.Order.Returns(5);
            var league6 = Substitute.For<ILeague>();
            league6.Name.Returns("league6");
            league6.SeasonDates.Returns(seasonDate);
            league6.Order.Returns(6);
            var league7 = Substitute.For<ILeague>();
            league7.Name.Returns("league7");
            league7.SeasonDates.Returns(seasonDate);
            league7.Order.Returns(7);

            IEnumerable<ILeague> topleagues = new List<ILeague>()
            {
                league1,league2,league3,league4,league5,league6,league7
            };
            leagueService.GetMajorLeaguesAsync(default, default).ReturnsForAnyArgs(topleagues);

            // Act
            await leaguesViewModel.LoadLeagues();
            var topLeagueViewModels = leaguesViewModel.LeagueGroups[0].ToList();

            // Assert
            Assert.Equal("LEAGUE1", topLeagueViewModels[0].LeagueName);
            Assert.Equal("LEAGUE2", topLeagueViewModels[1].LeagueName);
            Assert.Equal("LEAGUE3", topLeagueViewModels[2].LeagueName);
            Assert.Equal("LEAGUE4", topLeagueViewModels[3].LeagueName);
            Assert.Equal("LEAGUE5", topLeagueViewModels[4].LeagueName);
            Assert.Equal("LEAGUE6", topLeagueViewModels[5].LeagueName);
        }

        [Fact]
        public async Task LoadLeagues_LeaguesSeasonEndDateNotInRange7Days_RemoveFromTopLeagues()
        {
            // Arrange
            var inRangeDate = DateTime.Now.AddDays(-6);
            var outRangeDate = DateTime.Now.AddDays(-7);
            var seasonDate = new LeagueSeasonDates(DateTime.Now.AddDays(-99), inRangeDate);
            var seasonDateOutRange = new LeagueSeasonDates(DateTime.Now.AddDays(-99), outRangeDate);

            var league1 = Substitute.For<ILeague>();
            league1.Name.Returns("league1");
            league1.SeasonDates.Returns(seasonDate);
            league1.Order.Returns(1);
            var league2 = Substitute.For<ILeague>();
            league2.Name.Returns("league2");
            league2.SeasonDates.Returns(seasonDate);
            league2.Order.Returns(2);
            var league3 = Substitute.For<ILeague>();
            league3.Name.Returns("league3");
            league3.SeasonDates.Returns(seasonDate);
            league3.Order.Returns(3);
            var league4 = Substitute.For<ILeague>();
            league4.Name.Returns("league4");
            league4.SeasonDates.Returns(seasonDateOutRange);
            league4.Order.Returns(4);
            var league5 = Substitute.For<ILeague>();
            league5.Name.Returns("league5");
            league5.SeasonDates.Returns(seasonDate);
            league5.Order.Returns(5);
            var league6 = Substitute.For<ILeague>();
            league6.Name.Returns("league6");
            league6.SeasonDates.Returns(seasonDate);
            league6.Order.Returns(6);
            var league7 = Substitute.For<ILeague>();
            league7.Name.Returns("league7");
            league7.SeasonDates.Returns(seasonDate);
            league7.Order.Returns(7);

            IEnumerable<ILeague> topleagues = new List<ILeague>()
            {
                league1,league2,league3,league4,league5,league6,league7
            };
            leagueService.GetMajorLeaguesAsync(default, default).ReturnsForAnyArgs(topleagues);

            // Act
            await leaguesViewModel.LoadLeagues();
            var seasonOutRangeLeague = leaguesViewModel.LeagueGroups[0].FirstOrDefault(vm => vm.LeagueName == "League4");

            // Assert
            Assert.Null(seasonOutRangeLeague);
        }

        [Fact]
        public async Task SearchCommandExecuted_StringEmpty_NotChangeLeagues()
        {
            // Arrange
            var inRangeDate = DateTime.Now.AddDays(-6);
            var seasonDate = new LeagueSeasonDates(DateTime.Now.AddDays(-99), inRangeDate);

            var league1 = Substitute.For<ILeague>();
            league1.Name.Returns("league1");
            league1.SeasonDates.Returns(seasonDate);
            league1.Order.Returns(1);
            var league2 = Substitute.For<ILeague>();
            league2.Name.Returns("league2");
            league2.SeasonDates.Returns(seasonDate);
            league2.Order.Returns(2);
            var league3 = Substitute.For<ILeague>();
            league3.Name.Returns("league3");
            league3.SeasonDates.Returns(seasonDate);
            league3.Order.Returns(3);
            var league4 = Substitute.For<ILeague>();
            league4.Name.Returns("league4");
            league4.SeasonDates.Returns(seasonDate);
            league4.Order.Returns(4);
            var league5 = Substitute.For<ILeague>();
            league5.Name.Returns("league5");
            league5.SeasonDates.Returns(seasonDate);
            league5.Order.Returns(5);
            var league6 = Substitute.For<ILeague>();
            league6.Name.Returns("league6");
            league6.SeasonDates.Returns(seasonDate);
            league6.Order.Returns(6);
            var league7 = Substitute.For<ILeague>();
            league7.Name.Returns("league7");
            league7.SeasonDates.Returns(seasonDate);
            league7.Order.Returns(7);

            IEnumerable<ILeague> topleagues = new List<ILeague>()
            {
                league1,league2,league3,league4,league5,league6,league7
            };
            leagueService.GetMajorLeaguesAsync(default, default).ReturnsForAnyArgs(topleagues);

            await leaguesViewModel.LoadLeagues();
            var oldLeagues = leaguesViewModel.LeagueGroups;

            // Act
            leaguesViewModel.SearchCommand.Execute("");
            var newLeagues = leaguesViewModel.LeagueGroups;

            // Assert
            Assert.Equal(oldLeagues, newLeagues);
        }

        [Fact]
        public async Task SearchCommandExecuted_ValidString_MatchedLeagues()
        {
            // Arrange
            var inRangeDate = DateTime.Now.AddDays(-6);
            var seasonDate = new LeagueSeasonDates(DateTime.Now.AddDays(-99), inRangeDate);

            var league1 = Substitute.For<ILeague>();
            league1.Name.Returns("league1");
            league1.SeasonDates.Returns(seasonDate);
            league1.Order.Returns(1);
            var league12 = Substitute.For<ILeague>();
            league12.Name.Returns("league12");
            league12.SeasonDates.Returns(seasonDate);
            league12.Order.Returns(2);
            var league3 = Substitute.For<ILeague>();
            league3.Name.Returns("league3");
            league3.SeasonDates.Returns(seasonDate);
            league3.Order.Returns(3);
            var league4 = Substitute.For<ILeague>();
            league4.Name.Returns("league4");
            league4.SeasonDates.Returns(seasonDate);
            league4.Order.Returns(4);
            var league5 = Substitute.For<ILeague>();
            league5.Name.Returns("league5");
            league5.SeasonDates.Returns(seasonDate);
            league5.Order.Returns(5);
            var league6 = Substitute.For<ILeague>();
            league6.Name.Returns("league6");
            league6.SeasonDates.Returns(seasonDate);
            league6.Order.Returns(6);
            var league7 = Substitute.For<ILeague>();
            league7.Name.Returns("league7");
            league7.SeasonDates.Returns(seasonDate);
            league7.Order.Returns(7);

            IEnumerable<ILeague> topleagues = new List<ILeague>()
            {
                league1,league12,league3,league4,league5,league6,league7
            };
            leagueService.GetMajorLeaguesAsync(default, default).ReturnsForAnyArgs(topleagues);

            await leaguesViewModel.LoadLeagues();

            // Act
            leaguesViewModel.SearchCommand.Execute("league1");
            var newLeagues = leaguesViewModel.LeagueGroups[0].ToList();

            // Assert
            Assert.Equal(2, newLeagues.Count);
        }
    }
}
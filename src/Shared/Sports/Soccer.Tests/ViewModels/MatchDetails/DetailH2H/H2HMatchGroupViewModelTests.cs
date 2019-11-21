using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using LiveScore.Core.Converters;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.ViewModels.MatchDetails.HeadToHead;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.ViewModels.DetailH2H
{
    public class H2HMatchGroupViewModelTests
    {
        private readonly Fixture fixture;
        private readonly Func<string, string> buildFlagUrl;
        private readonly IMatchDisplayStatusBuilder matchDisplayStatusBuilder;

        public H2HMatchGroupViewModelTests()
        {
            fixture = new Fixture();
            buildFlagUrl = Substitute.For<Func<string, string>>();
            matchDisplayStatusBuilder = Substitute.For<IMatchDisplayStatusBuilder>();
        }

        [Fact]
        public void Init_Null_CorrectAssginedValueAndFormat()
        {
            var viewModel = new H2HMatchGroupViewModel(new List<H2HMatchViewModel>(), buildFlagUrl);

            Assert.True(string.IsNullOrEmpty(viewModel.CountryFlag));
        }

        [Fact]
        public void Init_Matches_AlwaysGetLeagueOfFirst()
        {
            var viewModels = new List<H2HMatchViewModel>
            {
                new H2HMatchViewModel(true, string.Empty,fixture.Create<SoccerMatch>(), matchDisplayStatusBuilder),
                new H2HMatchViewModel(true, string.Empty,fixture.Create<SoccerMatch>(), matchDisplayStatusBuilder)
            };

            var viewModel = new H2HMatchGroupViewModel(viewModels.ToList(), buildFlagUrl);

            Assert.Equal(viewModels.First().Match.LeagueGroupName, viewModel.LeagueName);
        }
    }
}
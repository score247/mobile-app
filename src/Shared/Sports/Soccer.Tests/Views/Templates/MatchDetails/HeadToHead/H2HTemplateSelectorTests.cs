using AutoFixture;
using LiveScore.Core.Converters;
using LiveScore.Core.Models.Matches;
using LiveScore.Core.Tests.Fixtures;
using LiveScore.Soccer.Models.Matches;
using LiveScore.Soccer.ViewModels.Matches.MatchDetails.HeadToHead;
using LiveScore.Soccer.Views.Matches.Templates.MatchDetails.HeadToHead;
using LiveScore.Soccer.Views.Matches.Templates.MatchDetails.HeadToHead.CollectionViewTemplates;
using NSubstitute;
using Xunit;

namespace Soccer.Tests.Views.Templates.MatchDetails.HeadToHead
{
    public class H2HTemplateSelectorTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly IMatch match;
        private readonly IMatchDisplayStatusBuilder matchStatusBuilder;

        private readonly H2HTemplateSelector h2hTemplateSelector;

        public H2HTemplateSelectorTests(ViewModelBaseFixture baseFixture)
        {
            match = baseFixture.CommonFixture.Specimens.Create<SoccerMatch>();

            matchStatusBuilder = Substitute.For<IMatchDisplayStatusBuilder>();
            baseFixture.DependencyResolver.Resolve<IMatchDisplayStatusBuilder>("1").Returns(matchStatusBuilder);

            h2hTemplateSelector = new H2HTemplateSelector();
        }

        [Fact]
        public void OnSelectTemplate_Always_ReturnH2HTemplate()
        {
            var template = h2hTemplateSelector.SelectTemplate(new object(), null);

            Assert.IsType<H2HMatchItemTemplate>(template);
        }

        [Fact]
        public void OnSelectTemplate_NotH2H_ReturnTeamMatchItemTemplate()
        {
            var template = h2hTemplateSelector.SelectTemplate(new H2HMatchViewModel(false, "sr:home", match, matchStatusBuilder), null);

            Assert.IsType<TeamMatchItemTemplate>(template);
        }

        [Fact]
        public void OnSelectTemplate_H2H_ReturnH2HMatchItemTemplate()
        {
            var template = h2hTemplateSelector.SelectTemplate(new H2HMatchViewModel(true, "sr:home", match, matchStatusBuilder), null);

            Assert.IsType<H2HMatchItemTemplate>(template);
        }
    }
}
namespace LiveScore.Core.Tests.Views.Selectors
{
    using LiveScore.Core.Enumerations;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Core.ViewModels;
    using LiveScore.Core.Views.Selectors;
    using NSubstitute;
    using Xamarin.Forms;
    using Xunit;

    public class MatchItemTemplateSelectorTests : IClassFixture<ViewModelBaseFixture>
    {
        private readonly BindableObject bindableObject;
        private readonly IDependencyResolver dependencyResolver;

        public MatchItemTemplateSelectorTests(ViewModelBaseFixture baseFixture)
        {
            bindableObject = Substitute.For<BindableObject>();
            dependencyResolver = baseFixture.DependencyResolver;
            bindableObject.BindingContext = new ViewModelBase(baseFixture.NavigationService, dependencyResolver);
        }

        [Fact]
        public void OnSelectTemplate_MatchItemTemplateIsNull_ReturnExpectedTemplate()
        {
            // Arrange

            var templateSelector = new MatchItemTemplateSelector();
            var expectedTemplate = new DataTemplate();
            dependencyResolver.Resolve<DataTemplate>(SportTypes.Soccer.Value).Returns(expectedTemplate);

            // Act
            var actualTemplate = templateSelector.SelectTemplate(1, bindableObject);

            // Assert
            Assert.Equal(expectedTemplate, actualTemplate);
        }
    }
}
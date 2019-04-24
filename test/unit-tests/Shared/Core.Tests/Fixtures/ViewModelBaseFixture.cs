namespace LiveScore.Core.Tests.Fixtures
{
    using LiveScore.Core.Services;
    using NSubstitute;
    using Prism.Events;
    using Prism.Navigation;

    public class ViewModelBaseFixture
    {
        public ViewModelBaseFixture()
        {
            AppSettingsFixture = new AppSettingsFixture();

            DepdendencyResolver = Substitute.For<IDepdendencyResolver>();
            DepdendencyResolver.Resolve<ISettingsService>().Returns(AppSettingsFixture.SettingsService);

            NavigationService = Substitute.For<INavigationService>();
        }

        public IDepdendencyResolver DepdendencyResolver { get; }

        public INavigationService NavigationService { get; }

        public IEventAggregator EventAggregator => Substitute.For<IEventAggregator>();

        public AppSettingsFixture AppSettingsFixture { get; }
    }
}
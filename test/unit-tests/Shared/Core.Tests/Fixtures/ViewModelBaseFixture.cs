namespace LiveScore.Core.Tests.Fixtures
{
    using LiveScore.Core.Services;
    using NSubstitute;
    using Prism.Behaviors;
    using Prism.Common;
    using Prism.Events;
    using Prism.Ioc;
    using Prism.Logging;
    using Prism.Navigation;
    using System.Threading.Tasks;

    public class ViewModelBaseFixture
    {
        public ViewModelBaseFixture()
        {
            AppSettingsFixture = new AppSettingsFixture();
            CommonFixture = new CommonFixture();
            EventAggregator = new EventAggregator();
            DepdendencyResolver = Substitute.For<IDependencyResolver>();
            DepdendencyResolver.Resolve<ISettingsService>().Returns(AppSettingsFixture.SettingsService);

            NavigationService = new FakeNavigationService();
        }

        public IDependencyResolver DepdendencyResolver { get; }

        public INavigationService NavigationService { get; }

        public IEventAggregator EventAggregator { get; }

        public AppSettingsFixture AppSettingsFixture { get; }

        public CommonFixture CommonFixture { get; set; }
    }

    public class FakeNavigationService : PageNavigationService
    {
        public FakeNavigationService(IContainerExtension container,
            IApplicationProvider applicationProvider,
            IPageBehaviorFactory pageBehaviorFactory,
            ILoggerFacade logger) : base(container, applicationProvider, pageBehaviorFactory, logger)
        {
        }

        public string NavigationPath { get; private set; }

        public bool? UseModalNavigation { get; private set; }

        public INavigationParameters Parameters { get; private set; }

        public FakeNavigationService() : base(null, null, null, null)
        {
        }

        protected override Task<INavigationResult> NavigateInternal(
            string name, INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            NavigationPath = name;
            UseModalNavigation = useModalNavigation;
            Parameters = parameters;

            return Task.FromResult<INavigationResult>(new NavigationResult());
        }
    }
}
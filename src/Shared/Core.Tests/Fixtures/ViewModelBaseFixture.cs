namespace LiveScore.Core.Tests.Fixtures
{
    using System;
    using System.Threading.Tasks;
    using LiveScore.Core.Services;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.AspNetCore.SignalR.Protocol;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using Prism.Behaviors;
    using Prism.Common;
    using Prism.Events;
    using Prism.Ioc;
    using Prism.Logging;
    using Prism.Navigation;

    public class ViewModelBaseFixture
    {
        public ViewModelBaseFixture()
        {
            AppSettingsFixture = new AppSettingsFixture();
            CommonFixture = new CommonFixture();
            EventAggregator = new EventAggregator();

            DependencyResolver = Substitute.For<IDependencyResolver>();
            DependencyResolver.Resolve<ISettingsService>().Returns(AppSettingsFixture.SettingsService);
            DependencyResolver.Resolve<IEventAggregator>().Returns(EventAggregator);

            NavigationService = new FakeNavigationService();
            HubService = Substitute.For<IHubService>();
            HubConnection = Substitute.For<FakeHubConnection>();
            //HubService.BuildMatchEventHubConnection().Returns(HubConnection);
            //HubService.BuildOddsEventHubConnection().Returns(HubConnection);
        }

        public IDependencyResolver DependencyResolver { get; }

        public INavigationService NavigationService { get; }

        public IEventAggregator EventAggregator { get; }

        public AppSettingsFixture AppSettingsFixture { get; }

        public CommonFixture CommonFixture { get; set; }

        public IHubService HubService { get; set; }

        public FakeHubConnection HubConnection { get; set; }
    }

    public class FakeHubConnection : HubConnection
    {
        public FakeHubConnection() : base(
            Substitute.For<IConnectionFactory>(),
            Substitute.For<IHubProtocol>(),
            Substitute.For<IServiceProvider>(),
            Substitute.For<ILoggerFactory>())
        {
        }
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

        public bool IsGoBack { get; private set; }

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

        protected override Task<INavigationResult> GoBackInternal(INavigationParameters parameters, bool? useModalNavigation, bool animated)
        {
            UseModalNavigation = useModalNavigation;
            Parameters = parameters;
            IsGoBack = true;
            return Task.FromResult<INavigationResult>(new NavigationResult());
        }
    }
}
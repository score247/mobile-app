using System;
using Fanex.Caching;
using LiveScore.Common;
using LiveScore.Common.Helpers;
using LiveScore.Common.Services;
using LiveScore.Core;
using LiveScore.Core.Controls.SearchPage;
using LiveScore.Core.Services;
using LiveScore.Core.ViewModels;
using LiveScore.Core.Views;
using LiveScore.Soccer.Services;
using LiveScore.ViewModels;
using LiveScore.Views;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Ioc;
using Refit;
using Xamarin.Forms;

namespace LiveScore.Configurations
{
    public static class Registration
    {
        private static IContainerProvider Container;

        public static IContainerRegistry UseContainerInstance(this IContainerRegistry containerRegistry, IContainerProvider container)
        {
            containerRegistry.RegisterInstance(container);
            Container = container;

            return containerRegistry;
        }

        public static IContainerRegistry RegisterServices(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IHttpService>(new HttpService(new Uri(Configuration.ApiEndPoint)));
            containerRegistry.RegisterSingleton<ICacheManager, CacheManager>();
            containerRegistry.RegisterSingleton<ICacheService, CacheService>();

            containerRegistry.RegisterSingleton<IEssential, Essential>();
            containerRegistry.RegisterSingleton<INetworkConnection, NetworkConnection>();

            var logService = new LoggingService(
                Container.Resolve<IEssential>(),
                Container.Resolve<INetworkConnection>(),
                null,
                Configuration.SentryDsn,
                Configuration.Environment);

            containerRegistry.RegisterInstance<ILoggingService>(logService);



            containerRegistry.RegisterSingleton<IApiPolicy, ApiPolicy>();
            containerRegistry.RegisterSingleton<IApiService, ApiService>();
            containerRegistry.Register<IHubConnectionBuilder, HubConnectionBuilder>();

            containerRegistry.RegisterSingleton<ISportService, SportService>();
            containerRegistry.RegisterSingleton<IMatchService, MatchService>();
            containerRegistry.RegisterSingleton<ISettings, Settings>();

            containerRegistry.RegisterInstance(new RefitSettings
            {
                ContentSerializer = new MessagePackContentSerializer()
            });
            containerRegistry.RegisterSingleton<IDependencyResolver, DependencyResolver>();
            containerRegistry.RegisterInstance<Func<string, string>>((countryCode)
                 => string.IsNullOrWhiteSpace(countryCode)
                     ? "images/flag_league/default_flag.svg"
                     : $"{Configuration.AssetsEndPoint}flags/{countryCode}.svg",
                FuncNameConstants.BuildFlagUrlFuncName);

            CompositeResolver.RegisterAndSetAsDefault(
                SoccerModelResolver.Instance,
                CoreModelResolver.Instance,
                BuiltinResolver.Instance,
                PrimitiveObjectResolver.Instance);

            return containerRegistry;
        }

        public static IContainerRegistry RegisterNavigation(this IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<SearchNavigationPage>();
            containerRegistry.RegisterForNavigation<MenuTabbedView, MenuTabbedViewModel>();
            containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
            containerRegistry.RegisterForNavigation<SelectSportView, SelectSportViewModel>();
            containerRegistry.RegisterForNavigation<SearchView, SearchViewModel>();

            return containerRegistry;
        }
    }
}
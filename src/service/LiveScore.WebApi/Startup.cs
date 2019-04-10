namespace LiveScore.WebApi
{
    using System.Collections.Generic;
    using LiveScore.Features.Leagues;
    using LiveScore.Features.Matches;
    using LiveScore.Shared;
    using LiveScore.Shared.Configurations;
    using LiveScore.Shared.SportRadarApi.Models;
    using LiveScore.Soccers.Features.Leagues;
    using LiveScore.Soccers.Features.Matches;
    using LiveScore.Soccers.Features.Matches.DataProviders;
    using LiveScore.WebApi.Shared.Configurations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            var sportRadarDataProviderSettings = new SportRadarDataProviderSettings();
            Configuration.GetSection("LiveScores:SportRadarSetting").Bind(sportRadarDataProviderSettings);

            var appSettings = new AppSettings(hostingEnvironment, Configuration, sportRadarDataProviderSettings);
            services.AddSingleton<IAppSettings>(appSettings);
            services.AddSingleton<LeagueService, LeagueServiceImpl>();
            services.AddSingleton<ILeagueApi, StaticLeagueApi>();
            services.AddSingleton<MatchService, MatchServiceImpl>();
            services.AddSingleton<InstanceFactory>(new InstanceFactoryImpl(
                new Dictionary<int, MatchDataAccess>
                {
                    {
                        1,
                        new MatchDataAccessImpl(appSettings.EnabledStaticData
                        ? (IMatchApi)new StaticMatchApi(appSettings)
                        : (IMatchApi)new SportRadarMatchApi())
                    }
                },
                new Dictionary<int, LeagueDataAccess>
                {
                    { 1, new LeagueDataAccessImpl() }
                }
                ));

            //
            services.AddSingleton(
                appSettings.EnabledStaticData
                ? (IMatchApi)new StaticMatchApi(appSettings)
                : (IMatchApi)new SportRadarMatchApi());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Live Scores API", Version = "v1" });
            });
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"../swagger/v1/swagger.json", "Chatbot API Docs");
                })
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}");
                });
        }
    }
}
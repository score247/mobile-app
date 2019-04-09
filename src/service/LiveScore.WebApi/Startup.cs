namespace LiveScore.WebApi
{
    using LiveScore.Features.Leagues;
    using LiveScore.Features.Matches;
    using LiveScore.Features.Matches.DataProviders;
    using LiveScore.Shared.Configurations;
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
            var appSettings = new AppSettings(this.hostingEnvironment);
            services.AddSingleton<IAppSettings>(appSettings);
            services.AddSingleton<LeagueService, LeagueServiceImpl>();
            services.AddSingleton<ILeagueApi, StaticLeagueApi>();
            services.AddSingleton<LeagueDataAccess, LeagueDataAccessImpl>();
            services.AddSingleton<MatchService, MatchServiceImpl>();
            services.AddSingleton<MatchDataAccess, MatchDataAccessImpl>();
            //
            services.AddSingleton<IMatchApi>(
                appSettings.IsUseStaticData
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
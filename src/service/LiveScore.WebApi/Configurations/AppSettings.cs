﻿namespace LiveScore.WebApi.Configurations
{
    using LiveScore.Shared.Configurations;
    using Microsoft.AspNetCore.Hosting;

    public class AppSettings : IAppSettings
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public AppSettings(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public string AppPath => hostingEnvironment.ContentRootPath;
    }
}
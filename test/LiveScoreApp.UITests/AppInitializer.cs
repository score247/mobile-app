using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace LiveScoreApp.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            return ConfigureApp
                .iOS   
                .EnableLocalScreenshots()            
                .StartApp();
        }
    }
}

namespace LiveScoreApp.UITests
{
    using Xamarin.UITest;

    public static class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            return ConfigureApp
                .iOS
                .EnableLocalScreenshots()
                .StartApp(Xamarin.UITest.Configuration.AppDataMode.DoNotClear);
        }
    }
}

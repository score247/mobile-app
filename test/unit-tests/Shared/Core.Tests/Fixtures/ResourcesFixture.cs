namespace LiveScore.Core.Tests.Fixtures
{
    using LiveScore.Core.Tests.Mocks;
    using Xamarin.Forms;

    public class ResourcesFixture
    {
        public ResourcesFixture()
        {
            MockXamForms.Init();

            var app = new ApplicationMock();
            app.Resources["GeneralFontSize"] = 12;
            app.Resources["SmallTextFontSize"] = 11;
            app.Resources["MediumTextFontSize"] = 10;
            app.Resources["UppercaseTextFontSize"] = 10;
            app.Resources["SmalleastTextFontSize"] = 10;

            app.Resources["FunctionGroupHeight"] = 10;

            app.Resources["FunctionGroupColor"] = Color.FromHex("#F24822");
            app.Resources["PrimaryColor"] = Color.FromHex("#F24822");
            app.Resources["PrimaryAccentColor"] = Color.FromHex("#F24822");
            app.Resources["FourthTextColor"] = Color.FromHex("#939393");
            app.Resources["SecondColor"] = Color.FromHex("#1D2133");
            app.Resources["LineColor"] = Color.FromHex("#1D2133");
            app.Resources["HomeTeamColor"] = Color.FromHex("#1D2133");
            app.Resources["AwayTeamColor"] = Color.FromHex("#1D2133");
            app.Resources["PrimaryTextColor"] = Color.FromHex("#1D2133");
            app.Resources["SecondTextColor"] = Color.FromHex("#1D2133");
            app.Resources["SecondAccentColor"] = Color.FromHex("#1D2133");

            app.Resources["LiveIcon"] = new Style(typeof(Label));
            app.Resources["WinnerInfoIcon"] = new Style(typeof(Image));

            app.Resources["FlexLayoutHorizontalCenter"] = new Style(typeof(FlexLayout));
            app.Resources["FlexLayoutAlignLeft"] = new Style(typeof(FlexLayout));
        }
    }
}
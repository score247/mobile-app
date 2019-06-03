﻿namespace LiveScore.Core.Tests.Fixtures
{
    using LiveScore.Core.Tests.Mocks;
    using Xamarin.Forms;

    public class ResourcesFixture
    {
        public ResourcesFixture()
        {
            MockXamForms.Init();

            var app = new ApplicationMock();

            app.Resources["PrimaryAccentColor"] = Color.FromHex("#F24822");
            app.Resources["FourthTextColor"] = Color.FromHex("#939393");
        }
    }
}
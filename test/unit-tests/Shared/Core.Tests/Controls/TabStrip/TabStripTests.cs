namespace LiveScore.Core.Tests.Controls.TabStrip
{
    using KellermanSoftware.CompareNetObjects;
    using LiveScore.Core.Controls.TabStrip;
    using LiveScore.Core.Tests.Fixtures;
    using LiveScore.Core.Tests.Mocks;
    using LiveScore.Core.ViewModels;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms;
    using Xunit;

    public class TabStripTests : IClassFixture<ViewModelBaseFixture>, IClassFixture<ResourcesFixture>
    {
        private readonly List<TabModel> tabs;

        public TabStripTests()
        {
            tabs = new List<TabModel>
            {
                new TabModel
                {
                    Name = "Info",
                    Template = new ContentView(),
                    ViewModel = Substitute.For<ViewModelBase>()
                },
                 new TabModel
                {
                    Name = "Tracker",
                    Template = new ContentView(),
                    ViewModel = Substitute.For<ViewModelBase>()
                }
            };
        }
    }
}
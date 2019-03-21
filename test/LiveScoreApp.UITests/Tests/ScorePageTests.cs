﻿namespace LiveScoreApp.UITests.Tests
{
    using NUnit.Framework;
    using Xamarin.UITest;

    public class ScorePageTests : BaseTest
    {
        public ScorePageTests(Platform platform) : base(platform)
        {
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            scorePage.WaitForPageToLoad();
        }

        [Test]
        public void ScorePage_Launches_DisplayTabBars()
        {
            var totalTabs = scorePage.tabBars.Length;

            Assert.AreEqual(5, totalTabs, "Tab bars binding incorrect!");
        }

        [Test]
        public void ScorePageLaunches()
        {
            app.Screenshot("ScorePage");
        }

        [Ignore("render elements tree, only use for building tests")]
        [Test]
        public void Repl()
        {
            app.Repl();
        }
    }
}

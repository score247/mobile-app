using System;
using System.Collections.Generic;
using System.Text;
using LiveScore.Common.LangResources;
using LiveScore.Core.ViewModels;

namespace LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Fixtures
{
    public class FixturesViewModel : ViewModelBase
    {
        public FixturesViewModel()
        {
            Title = AppResources.Fixtures;
        }
    }
}
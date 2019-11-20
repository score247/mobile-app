using System;
using System.Collections.Generic;
using System.Text;
using LiveScore.Common.LangResources;
using LiveScore.Core.ViewModels;

namespace LiveScore.Soccer.ViewModels.Leagues.LeagueDetails.Table
{
    public class TableViewModel : ViewModelBase
    {
        public TableViewModel()
        {
            Title = AppResources.Table;
        }
    }
}
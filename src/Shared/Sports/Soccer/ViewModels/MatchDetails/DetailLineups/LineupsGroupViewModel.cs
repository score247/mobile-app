﻿using System.Collections.Generic;

namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailLineups
{
    public class LineupsGroupViewModel : List<LineupsItemViewModel>
    {
        public LineupsGroupViewModel(string name, IEnumerable<LineupsItemViewModel> lineupsItems)
            : base(lineupsItems)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
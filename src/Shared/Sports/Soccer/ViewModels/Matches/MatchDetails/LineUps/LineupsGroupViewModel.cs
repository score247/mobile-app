using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LiveScore.Soccer.ViewModels.Matches.MatchDetails.LineUps
{
    public class LineupsGroupViewModel : ObservableCollection<LineupsItemViewModel>
    {
        public LineupsGroupViewModel(string name, IEnumerable<LineupsItemViewModel> lineupsItems)
            : base(lineupsItems)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
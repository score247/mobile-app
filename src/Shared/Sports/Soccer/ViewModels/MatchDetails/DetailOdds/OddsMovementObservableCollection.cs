namespace LiveScore.Soccer.ViewModels.MatchDetails.DetailOdds
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using OddItems;

    public class OddsMovementObservableCollection : ObservableCollection<BaseMovementItemViewModel>
    {
        public OddsMovementObservableCollection(string heading)
        {
            Heading = heading;
        }

        public OddsMovementObservableCollection(string heading, IEnumerable<BaseMovementItemViewModel> items) : base(items)
        {
            Heading = heading;
        }

        public string Heading { get; }

        public ObservableCollection<BaseMovementItemViewModel> ItemViews => this;
    }
}
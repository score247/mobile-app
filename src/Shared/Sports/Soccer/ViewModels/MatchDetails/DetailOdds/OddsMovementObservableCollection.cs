namespace LiveScore.Soccer.ViewModels.DetailOdds
{
    using System.Collections.ObjectModel;
    using LiveScore.Soccer.ViewModels.DetailOdds.OddItems;

    public class OddsMovementObservableCollection : ObservableCollection<BaseMovementItemViewModel>
    {
        public OddsMovementObservableCollection(string heading)
        {
            Heading = heading;
        }

        public string Heading { get; private set; }

        public ObservableCollection<BaseMovementItemViewModel> ItemViews => this;
    }
}

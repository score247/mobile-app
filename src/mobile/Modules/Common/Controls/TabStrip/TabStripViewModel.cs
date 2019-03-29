namespace Common.Controls.TabStrip
{
    using System.Collections.ObjectModel;
    using Prism.Mvvm;

    public class TabStripViewModel : BindableBase
    {
        private ObservableCollection<TabModel> tabs;

        public ObservableCollection<TabModel> Tabs
        {
            get { return tabs; }
            set { SetProperty(ref tabs, value); }
        }

        private int currentPosition;

        public int CurrentPosition
        {
            get { return currentPosition; }
            set { SetProperty(ref currentPosition, value); }
        }
    }
}

namespace LiveScore.ViewModels
{
    using System.Collections.ObjectModel;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using LiveScore.Models;
    using Prism.Navigation;

    public class TabMoreViewModel : ViewModelBase
    {
        public ObservableCollection<TabItem> TabItems { get; set; }

        public TabMoreViewModel(INavigationService navigationService, IGlobalFactoryProvider globalFactory, ISettingsService settingsService)
            : base(navigationService, globalFactory, settingsService)
        {
            TabItems = new ObservableCollection<TabItem> 
            { 
                new TabItem("News"), 
                new TabItem("TV") 
            };
        }
    }
}

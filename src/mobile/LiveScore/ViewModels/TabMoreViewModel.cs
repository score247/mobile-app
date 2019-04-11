namespace LiveScore.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using LiveScore.Common.Extensions;
    using LiveScore.Core.Factories;
    using LiveScore.Core.Services;
    using LiveScore.Core.ViewModels;
    using LiveScore.Models;
    using LiveScore.News.Views;
    using LiveScore.TVSchedule.Views;
    using Prism.Navigation;

    public class TabMoreViewModel : ViewModelBase
    {
        public ObservableCollection<TabItem> TabItems { get; set; }

        public DelegateAsyncCommand<TabItem> ItemTappedCommand { get; set; }

        public TabMoreViewModel(INavigationService navigationService, IGlobalFactoryProvider globalFactory, ISettingsService settingsService)
            : base(navigationService, globalFactory, settingsService)
        {
            TabItems = new ObservableCollection<TabItem>
            {
                new TabItem("News", nameof(EmptyNewsView)),
                new TabItem("TV", nameof(EmptyTVScheduleView))
            };

            ItemTappedCommand = new DelegateAsyncCommand<TabItem>(ItemTapped);
        }

        private async Task ItemTapped(TabItem item)
        {
            if (item != null)
            {
                await NavigationService.NavigateAsync(item.View);
            }
        }
    }
}

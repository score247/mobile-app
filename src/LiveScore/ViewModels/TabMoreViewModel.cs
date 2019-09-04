namespace LiveScore.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Core;
    using LiveScore.Core.ViewModels;
    using LiveScore.Features.News.Views;
    using LiveScore.Features.TVSchedule.Views;
    using Models;
    using Prism.Navigation;

    public class TabMoreViewModel : ViewModelBase
    {
        public TabMoreViewModel(INavigationService navigationService, IDependencyResolver serviceLocator) : base(navigationService, serviceLocator)
        {
            TabItems = new ObservableCollection<TabItem>
            {
                new TabItem("TV", nameof(EmptyTVScheduleView), "images/common/tv.png"),
                new TabItem("News", nameof(EmptyNewsView), "images/common/news.png")
            };

            ItemTappedCommand = new DelegateAsyncCommand<TabItem>(ItemTapped);
        }

        public ObservableCollection<TabItem> TabItems { get; }

        public DelegateAsyncCommand<TabItem> ItemTappedCommand { get; }

        private async Task ItemTapped(TabItem item)
        {
            if (item != null)
            {
                await NavigationService.NavigateAsync(item.View);
            }
        }
    }
}
using System.Collections;
using System.Windows.Input;

namespace LiveScore.Core.Controls.CustomListView
{
    using Xamarin.Forms;

    public class CustomListView : ListView
    {
        public CustomListView() : base(ListViewCachingStrategy.RecycleElementAndDataTemplate)
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }

#if  AUTOTEST

        public CustomListView(ListViewCachingStrategy cachingStrategy) : base(ListViewCachingStrategy.RetainElement)
        {
        }

#else

        public CustomListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }

#endif

        public static readonly BindableProperty LoadMoreCommandProperty
            = BindableProperty.Create(
                nameof(LoadMoreCommand),
                typeof(ICommand),
                typeof(CustomListView));

        public ICommand LoadMoreCommand
        {
            get => (ICommand)GetValue(LoadMoreCommandProperty);
            set => SetValue(LoadMoreCommandProperty, value);
        }

        public static readonly BindableProperty TriggerLoadMoreIndexProperty
            = BindableProperty.Create(
                nameof(TriggerLoadMoreIndex),
                typeof(int),
                typeof(CustomListView),
                defaultValue: 5);

        public int TriggerLoadMoreIndex
        {
            get => (int)GetValue(TriggerLoadMoreIndexProperty);
            set => SetValue(TriggerLoadMoreIndexProperty, value);
        }

        private void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (ItemsSource is IList items
                && (items.Count < TriggerLoadMoreIndex || e.Item == items[^TriggerLoadMoreIndex])
                && LoadMoreCommand?.CanExecute(null) == true)
            {
                LoadMoreCommand.Execute(null);
            }
        }
    }
}
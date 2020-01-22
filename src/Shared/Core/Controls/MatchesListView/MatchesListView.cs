using System;
using System.Collections;
using System.Windows.Input;
using LiveScore.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace LiveScore.Core.Controls.MatchesListView
{
    public class MatchesListView : ListView
    {
        public MatchesListView() : base(ListViewCachingStrategy.RecycleElementAndDataTemplate)
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }

#if  AUTOTEST

        public MatchesListView(ListViewCachingStrategy cachingStrategy) : base(ListViewCachingStrategy.RetainElement)
        {
        }

#else

        public MatchesListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }

#endif

        public static readonly BindableProperty LoadMoreCommandProperty
            = BindableProperty.Create(
                nameof(LoadMoreCommand),
                typeof(ICommand),
                typeof(MatchesListView));

        public ICommand LoadMoreCommand
        {
            get => (ICommand)GetValue(LoadMoreCommandProperty);
            set => SetValue(LoadMoreCommandProperty, value);
        }

        public static readonly BindableProperty TriggerLoadMoreIndexProperty
            = BindableProperty.Create(
                nameof(TriggerLoadMoreIndex),
                typeof(int),
                typeof(MatchesListView),
                defaultValue: 5);

        public int TriggerLoadMoreIndex
        {
            get => (int)GetValue(TriggerLoadMoreIndexProperty);
            set => SetValue(TriggerLoadMoreIndexProperty, value);
        }

        public static readonly BindableProperty FooterHeightProperty
            = BindableProperty.Create(
                nameof(FooterHeight),
                typeof(double),
                typeof(MatchesListView),
                propertyChanged: OnFooterHeightChanged);

        private static void OnFooterHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MatchesListView listView && newValue is double height && listView.BindingContext is MatchesViewModel viewModel)
            {
                viewModel.FooterHeight = height;
            }
        }

        public double FooterHeight
        {
            get => (double)GetValue(FooterHeightProperty);
            set => SetValue(FooterHeightProperty, value);
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

        public event EventHandler ScrollToTopEvent;

        public void ScrollToTop()
        {
            ScrollToTopEvent?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ScrollToFirstItemEvent;

        public void ScrollToFirstItem()
        {
            ScrollToFirstItemEvent?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler AdjustFooterHeightEvent;

        public void AdjustFooterHeight()
        {
            AdjustFooterHeightEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
namespace LiveScore.Core.Controls.TabStrip
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabStrip : ContentView
    {
        private static int currentTabIndex;

        public TabStrip()
        {
            InitializeComponent();
            TabHeader.BindingContext = this;
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            "ItemsSource",
            typeof(IEnumerable<TabModel>),
            typeof(TabStrip),
            propertyChanged: OnItemsSourceChanged);

        public IEnumerable<TabModel> ItemsSource
        {
            get { return (IEnumerable<TabModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TabStrip)bindable;

            if (control != null)
            {
                var tabs = (IEnumerable<TabModel>)newValue;

                if (tabs != null)
                {
                    tabs.First().ViewModel.OnAppearing();
                    control.TabContent.Content = tabs.First().ContentTemplate;
                    control.TabContent.Content.BindingContext = tabs.First().ViewModel;

                    MessagingCenter.Subscribe<string, string>("Tab", "TabChange", (_, index) =>
                    {
                        try
                        {
                            var tab = tabs.ToArray()[int.Parse(index)];
                            currentTabIndex = int.Parse(index);
                            //var currentViewModel = control.TabContent.BindingContext as ViewModelBase;
                            //currentViewModel.OnDisappearing();
                            tab.ViewModel.OnAppearing();

                            control.TabContent.Content = tab.ContentTemplate;
                            control.TabContent.BindingContext = tab.ViewModel;
                        }
                        catch (Exception ex)
                        {
                        }
                    });
                }
            }
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            int index;

            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    index = currentTabIndex + 1;

                    if (index < ItemsSource.Count())
                    {
                        MessagingCenter.Send("Tab", "TabChange", index.ToString());
                    }
                    break;

                case SwipeDirection.Right:
                    index = currentTabIndex - 1;

                    if (index >= 0)
                    {
                        MessagingCenter.Send("Tab", "TabChange", index.ToString());
                    }
                    break;
            }
        }
    }
}
namespace LiveScore.Core.Controls.TabStrip
{
    using System.Collections.Generic;
    using System.Linq;
    using LiveScore.Core.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabStrip : ContentView
    {
        private const string TabChangeEvent = "TabChange";
        private static int currentTabIndex;

        private const int InAnimationDuration = 400;        
        private const int FadeDuration = 100;
        private const int InLeft = -1000;
        private const int InRight = 600;

        public TabStrip()
        {
            var currentInstance = this;

            InitializeComponent();
            TabHeader.BindingContext = currentInstance;
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
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

            if (control == null || newValue == null)
            {
                MessagingCenter.Unsubscribe<string, int>(nameof(TabStrip), TabChangeEvent);
                currentTabIndex = 0;
                return;
            }

            var tabs = (IEnumerable<TabModel>)newValue;

            if (oldValue == null)
            {
                InitDefaultTab(control, tabs);
            }

            SubscribeTabChange(control, tabs);
        }

        private static void InitDefaultTab(TabStrip control, IEnumerable<TabModel> tabs)
        {
            control.TabContent.Children.Clear();
            control.TabContent.Children.Add(new ContentView
            {
                Content = tabs.First().Template,
                BindingContext = tabs.First().ViewModel
            });
            tabs.First().ViewModel.OnAppearing();
        }

        private static void SubscribeTabChange(TabStrip control, IEnumerable<TabModel> tabs)
        {
            MessagingCenter.Subscribe<string, int>(nameof(TabStrip), "TabChange", async (_, index) =>
            {

                var inTranslationX = currentTabIndex < index ? InRight : InLeft;

                currentTabIndex = index;

                var tab = tabs.ToArray()[index];

                ((ContentView)control.TabContent.Children[0]).Opacity = 1;
                
                await ((ContentView)control.TabContent.Children[0]).FadeTo(0, FadeDuration);

                control.TabContent.Children.ToList()
                    .ForEach(c => (c.BindingContext as ViewModelBase)?.OnDisappearing());
                control.TabContent.Children.Clear();

                var tabContentView = new ContentView
                {
                    Content = tab.Template,
                    BindingContext = tab.ViewModel,
                    TranslationX = inTranslationX
                };

                control.TabContent.Children.Add(tabContentView);

                await tabContentView.TranslateTo(0, 0, InAnimationDuration, Easing.SinOut);

                tab.ViewModel.OnAppearing();
            });
        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            if (e.Direction == SwipeDirection.Left)
            {
                var newIndex = currentTabIndex + 1;

                if (newIndex < ItemsSource.Count())
                {
                    MessagingCenter.Send(nameof(TabStrip), TabChangeEvent, newIndex);
                }
            }
            else
            {
                if (e.Direction == SwipeDirection.Right)
                {
                    var newIndex = currentTabIndex - 1;

                    if (newIndex >= 0)
                    {
                        MessagingCenter.Send(nameof(TabStrip), TabChangeEvent, newIndex);
                    }
                }
            }
        }
    }
}
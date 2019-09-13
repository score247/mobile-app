namespace LiveScore.Core.Controls.DateBar.Views
{
    using System;
    using System.Windows.Input;
    using EventArgs;
    using LiveScore.Common.LangResources;
    using MethodTimer;
    using Xamarin.Forms;

    public partial class DateBar : ContentView
    {
        private const int LiveIndex = 0;

        public DateBar()
        {
            InitializeComponent();
        }

        [Time]
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            BuildDateBar();
        }

        private void BuildDateBar()
        {
            DateBarLayout.Children.Clear();

            CalendarIndex = NumberDisplayDays * 2 + 2;

            AddLiveItem();

            AddDateItems();

            AddCalendarItem();
        }

        public static readonly BindableProperty NumberDisplayDaysProperty
          = BindableProperty.Create(
              nameof(NumberDisplayDays),
              typeof(int),
              typeof(DateBar));

        public int NumberDisplayDays
        {
            get => (int)GetValue(NumberDisplayDaysProperty);
            set => SetValue(NumberDisplayDaysProperty, value);
        }

        public static readonly BindableProperty SelectedIndexProperty
            = BindableProperty.Create(
                nameof(SelectedIndexProperty),
                typeof(int),
                typeof(DateBar),
                propertyChanged: OnSelectedIndexChanged);

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public static readonly BindableProperty ItemTappedCommandProperty
            = BindableProperty.Create(nameof(ItemTappedCommand), typeof(ICommand), typeof(DateBar), null);

        public ICommand ItemTappedCommand
        {
            get => GetValue(ItemTappedCommandProperty) as ICommand;
            set => SetValue(ItemTappedCommandProperty, value);
        }

        public int CalendarIndex { get; private set; }

        private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DateBar)bindable;
            var selectedIndex = (int)newValue;
            var oldIndex = (int)oldValue;

            if (control != null)
            {
                var dateBarLayout = control.Content as FlexLayout;

                if (dateBarLayout.Children?.Count == 0)
                {
                    return;
                }

                RemoveSelectedColor(control, oldIndex, dateBarLayout);

                AddSelectedColor(control, selectedIndex, dateBarLayout);
            }
        }

        private void AddLiveItem()
        {
            var liveIcon = new Label
            {
                Style = (Style)Resources["LiveIcon"]
            };

            liveIcon.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = BuildTapDateBarItemCommand(DateTime.Today, 0)
            });

            DateBarLayout.Children.Add(liveIcon);
        }

        private void AddCalendarItem()
        {
            var calendarIcon = new Label
            {
                Style = (Style)Resources["CalendarIcon"]
            };

            calendarIcon.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = BuildTapDateBarItemCommand(DateTime.Today, CalendarIndex)
            });

            DateBarLayout.Children.Add(calendarIcon);
        }

        private void AddDateItems()
        {
            var currentIndex = 1;

            for (int i = -NumberDisplayDays; i <= NumberDisplayDays; i++)
            {
                var dateBarItem = BuildDateBarItem(DateTime.Today.AddDays(i), currentIndex);

                DateBarLayout.Children.Add(dateBarItem);
                currentIndex++;
            }
        }

        private StackLayout BuildDateBarItem(DateTime date, int index)
        {
            var dateBarItemLayout = new StackLayout { Style = (Style)Resources["DateBarItemLayout"] };

            var dayNameLabel = new Label
            {
                Text = (date == DateTime.Today ? AppResources.Today : date.Date.ToString("ddd")).ToUpperInvariant(),
                Style = (Style)Resources["DateBarDayNameLabel"],
            };

            var dayMonthLabel = new Label
            {
                Text = date.ToString("dd MMM").ToUpperInvariant(),
                Style = (Style)Resources["DateBarDayMonthLabel"]
            };

            if (index == SelectedIndex)
            {
                SetSelectedTextColor(dayNameLabel);
                SetSelectedTextColor(dayMonthLabel);
            }

            dateBarItemLayout.Children.Add(dayNameLabel);
            dateBarItemLayout.Children.Add(dayMonthLabel);

            dateBarItemLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = BuildTapDateBarItemCommand(date, index)
            });

            return dateBarItemLayout;
        }

        private Command BuildTapDateBarItemCommand(DateTime date, int index)
        {
            return new Command(() =>
            {
                var args = new DateBarItemTappedEventArgs(index, date);
                ItemTappedCommand?.Execute(args);

                SelectedIndex = index;
            });
        }

        private static void AddSelectedColor(DateBar control, int selectedIndex, FlexLayout dateBarLayout)
        {
            if (selectedIndex == LiveIndex)
            {
                var liveItem = dateBarLayout.Children[selectedIndex] as Label;

                control.SetSelectedTextColor(liveItem);
            }
            else if (selectedIndex == control.CalendarIndex)
            {
                var calendarItem = dateBarLayout.Children[selectedIndex] as Label;

                control.SetSelectedTextColor(calendarItem);
            }
            else
            {
                var newItemLayout = dateBarLayout.Children[selectedIndex] as StackLayout;
                var dayNameLabel = newItemLayout.Children[0] as Label;
                var dayLabel = newItemLayout.Children[1] as Label;
                control.SetSelectedTextColor(dayNameLabel);
                control.SetSelectedTextColor(dayLabel);
            }
        }

        private static void RemoveSelectedColor(DateBar control, int oldIndex, FlexLayout dateBarLayout)
        {
            if (oldIndex == LiveIndex)
            {
                var liveItem = dateBarLayout.Children[oldIndex] as Label;

                liveItem.TextColor = (Color)control.Resources["DateBarLiveColor"];
            }
            else if (oldIndex == control.CalendarIndex)
            {
                var calendarItem = dateBarLayout.Children[oldIndex] as Label;

                control.SetTextColor(calendarItem);
            }
            else
            {
                var oldItemLayout = dateBarLayout.Children[oldIndex] as StackLayout;
                var dayNameLabel = oldItemLayout.Children[0] as Label;
                var dayLabel = oldItemLayout.Children[1] as Label;
                control.SetTextColor(dayNameLabel);
                control.SetTextColor(dayLabel);
            }
        }

        private void SetTextColor(Label item)
        {
            item.TextColor = (Color)Resources["DateBarTextColor"];
        }

        private void SetSelectedTextColor(Label item)
        {
            item.TextColor = (Color)Resources["DateBarSelectedTextColor"];
        }
    }
}
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

            AddLiveBox();

            AddDateBoxes();

            AddCalendarBox();
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

                if (oldIndex == 0)
                {
                    var liveItem = dateBarLayout.Children[oldIndex] as Label;

                    liveItem.TextColor = (Color)control.Resources["DateBarLiveColor"];
                }
                else
                {
                    var oldItemLayout = dateBarLayout.Children[oldIndex] as StackLayout;
                    var dayNameLabel = oldItemLayout.Children[0] as Label;
                    var dayLabel = oldItemLayout.Children[1] as Label;
                    control.SetTextColor(dayNameLabel);
                    control.SetTextColor(dayLabel);
                }

                if (selectedIndex == 0)
                {
                    var liveItem = dateBarLayout.Children[selectedIndex] as Label;

                    control.SetSelectedTextColor(liveItem);
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
        }

        private void AddLiveBox()
        {
            var liveIcon = new Label
            {
                Style = (Style)Resources["LiveIcon"]
            };

            DateBarLayout.Children.Add(liveIcon);
        }

        private void AddDateBoxes()
        {
            var currentIndex = 1;

            for (int i = -NumberDisplayDays; i <= NumberDisplayDays; i++)
            {
                var dateBarItem = BuildDateBarItem(DateTime.Today.AddDays(i), currentIndex);

                DateBarLayout.Children.Add(dateBarItem);
                currentIndex++;
            }
        }

        private void AddCalendarBox()
        {
            var labelIcon = new Label
            {
                Style = (Style)Resources["CalendarIcon"]
            };

            DateBarLayout.Children.Add(labelIcon);
        }

        private StackLayout BuildDateBarItem(DateTime date, int index)
        {
            var dateBarItemLayout = new StackLayout();

            var dayNumberLbl = new Label
            {
                Text = (date == DateTime.Today ? AppResources.Today : date.Date.ToString("ddd")).ToUpperInvariant(),
                Style = (Style)Resources["DateBarDayNumberLabel"],
            };

            var dayNameLbl = new Label
            {
                Text = date.ToString("dd MMM").ToUpperInvariant(),
                Style = (Style)Resources["DateBarDayNameLabel"]
            };

            if (index == SelectedIndex)
            {
                SetSelectedTextColor(dayNumberLbl);
                SetSelectedTextColor(dayNameLbl);
            }

            dateBarItemLayout.Children.Add(dayNumberLbl);
            dateBarItemLayout.Children.Add(dayNameLbl);

            dateBarItemLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = BuildTapDateBarItemCommand(date, index, dayNumberLbl, dayNameLbl)
            });

            return dateBarItemLayout;
        }

        private Command BuildTapDateBarItemCommand(DateTime date, int index, Label dayNumberLbl, Label dayNameLbl)
        {
            return new Command(() =>
            {
                var args = new DateBarItemTappedEventArgs(index, date);
                ItemTappedCommand?.Execute(args);

                SelectedIndex = index;
            });
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
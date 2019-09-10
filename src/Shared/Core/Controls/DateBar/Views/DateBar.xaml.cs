using System.Windows.Input;
using LiveScore.Core.Controls.DateBar.EventArgs;
using PanCardView;

namespace LiveScore.Core.Controls.DateBar.Views
{
    using ViewModels;
    using Xamarin.Forms;
    using System;

    public partial class DateBar : ContentView
    {
        public DateBar()
        {
            InitializeComponent();
        }

        public DateBarViewModel ViewModel { get; set; }

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

            if (control != null)
            {
                control.BuildDateBar();
            }
        }

        private void AddLiveBox()
        {
            var liveBox = new StackLayout { Style = (Style)Resources["DateBarBox"] };
            liveBox.Children.Add(new Label
            {
                Style = (Style)Resources["HomeIcon"]
            });

            DateBarLayout.Children.Add(liveBox);
        }

        private void AddDateBoxes()
        {
            var currentIndex = 0;

            for (int i = -NumberDisplayDays; i <= NumberDisplayDays; i++)
            {
                var dateBarItem = BuildDateBarItem(DateTime.Today.AddDays(i), currentIndex);

                DateBarLayout.Children.Add(dateBarItem);
                currentIndex++;
            }
        }

        private void AddCalendarBox()
        {
            var calendarBox = new StackLayout { Style = (Style)Resources["DateBarBox"] };
            calendarBox.Children.Add(new Label
            {
                Style = (Style)Resources["CalendarIcon"]
            });

            DateBarLayout.Children.Add(calendarBox);
        }

        private StackLayout BuildDateBarItem(DateTime date, int index)
        {
            var dateBarItemLayout = new StackLayout { Style = (Style)Resources["DateBarBox"] };
            var dayNumber = new Label
            {
                Text = date.Day.ToString(),
                Style = (Style)Resources["DateBarDayNumberLabel"],
            };

            var dayName = new Label
            {
                Text = date.Date.DayOfWeek.ToString().Substring(0, 3).ToUpperInvariant(),
                Style = (Style)Resources["DateBarDayNameLabel"]
            };

            if (index == SelectedIndex)
            {
                dayNumber.TextColor = (Color)Resources["DateBarSelectedColor"];
                dayName.TextColor = (Color)Resources["DateBarSelectedColor"];
            }

            dateBarItemLayout.Children.Add(dayNumber);
            dateBarItemLayout.Children.Add(dayName);

            dateBarItemLayout.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    var args = new DateBarItemTappedEventArgs(index, date);
                    ItemTappedCommand?.Execute(args);
                    SelectedIndex = index;
                    BuildDateBar();
                })
            });

            return dateBarItemLayout;
        }
    }
}
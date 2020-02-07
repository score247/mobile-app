using System;
using System.Windows.Input;
using Xamarin.Forms;
using static System.Math;

namespace LiveScore.Core.Controls.ExpandableView
{
    public class ExpandableView : StackLayout
    {
        public const string ExpandAnimationName = nameof(ExpandAnimationName);

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        public event Action Tapped;

        public static readonly BindableProperty PrimaryViewProperty
            = BindableProperty.Create(nameof(PrimaryView), typeof(View), typeof(ExpandableView), propertyChanged: (bindable, oldValue, newValue) =>
        {
            (bindable as ExpandableView).SetPrimaryView(oldValue as View);
            (bindable as ExpandableView).OnTouchHandlerViewChanged();
        });

        public static readonly BindableProperty SecondaryViewProperty
            = BindableProperty.Create(nameof(SecondaryView), typeof(View), typeof(ExpandableView), propertyChanged: (bindable, oldValue, newValue) =>
        {
            (bindable as ExpandableView).SetSecondaryView(oldValue as View, newValue as View);
        });

        public static readonly BindableProperty SecondaryViewTemplateProperty
            = BindableProperty.Create(nameof(SecondaryViewTemplate), typeof(DataTemplate), typeof(ExpandableView), propertyChanged: (bindable, oldValue, newValue) =>
        {
            (bindable as ExpandableView).SetSecondaryView(true);
        });

        public static readonly BindableProperty IsExpandedProperty
            = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(ExpandableView), default(bool), BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
        {
            (bindable as ExpandableView).SetSecondaryView();
        });

        public static readonly BindableProperty TouchHandlerViewProperty
            = BindableProperty.Create(nameof(TouchHandlerView), typeof(View), typeof(ExpandableView), propertyChanged: (bindable, oldValue, newValue) =>
        {
            (bindable as ExpandableView).OnTouchHandlerViewChanged();
        });

        public static readonly BindableProperty IsTouchToExpandEnabledProperty
            = BindableProperty.Create(nameof(IsTouchToExpandEnabled), typeof(bool), typeof(ExpandableView), true);

        public static readonly BindableProperty SecondaryViewHeightRequestProperty
            = BindableProperty.Create(nameof(SecondaryViewHeightRequest), typeof(double), typeof(ExpandableView), -1.0);

        public static readonly BindableProperty ExpandAnimationLengthProperty
            = BindableProperty.Create(nameof(ExpandAnimationLength), typeof(uint), typeof(ExpandableView), 250u);

        public static readonly BindableProperty CollapseAnimationLengthProperty
            = BindableProperty.Create(nameof(CollapseAnimationLength), typeof(uint), typeof(ExpandableView), 250u);

        public static readonly BindableProperty ExpandAnimationEasingProperty
            = BindableProperty.Create(nameof(ExpandAnimationEasing), typeof(Easing), typeof(ExpandableView), Easing.SinOut);

        public static readonly BindableProperty CollapseAnimationEasingProperty
            = BindableProperty.Create(nameof(CollapseAnimationEasing), typeof(Easing), typeof(ExpandableView), Easing.SinIn);

        public static readonly BindableProperty StatusProperty
            = BindableProperty.Create(nameof(Status), typeof(ExpandStatus), typeof(ExpandableView), default(ExpandStatus), BindingMode.OneWayToSource);

        public static readonly BindableProperty CommandParameterProperty
            = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ExpandableView));

        public static readonly BindableProperty CommandProperty
            = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ExpandableView));

        public static readonly BindableProperty ForceUpdateSizeCommandProperty
            = BindableProperty.Create(nameof(ForceUpdateSizeCommand), typeof(ICommand), typeof(ExpandableView), defaultBindingMode: BindingMode.OneWayToSource);

        private readonly TapGestureRecognizer defaultTapGesture;
        private DataTemplate previousTemplate;
        private bool shouldIgnoreAnimation;
        private double lastVisibleHeight = -1;
        private double previousWidth = -1;
        private double startHeight;
        private double endHeight;

        public ExpandableView()
        {
            defaultTapGesture = new TapGestureRecognizer
            {
                CommandParameter = this,
                Command = new Command(p =>
                {
                    var view = (p as View).Parent;
                    while (view != null && !(view is Page))
                    {
                        if (view is ExpandableView ancestorExpandable)
                        {
                            ancestorExpandable.SecondaryView.HeightRequest = -1;
                        }
                        view = view.Parent;
                    }
                    Command?.Execute(CommandParameter);
                    Tapped?.Invoke();
                    if (!IsTouchToExpandEnabled)
                    {
                        return;
                    }
                    IsExpanded = !IsExpanded;
                })
            };

            ForceUpdateSizeCommand = new Command(ForceUpdateSize);
        }

        public View PrimaryView
        {
            get => GetValue(PrimaryViewProperty) as View;
            set => SetValue(PrimaryViewProperty, value);
        }

        public View SecondaryView
        {
            get => GetValue(SecondaryViewProperty) as View;
            set => SetValue(SecondaryViewProperty, value);
        }

        public DataTemplate SecondaryViewTemplate
        {
            get => GetValue(SecondaryViewTemplateProperty) as DataTemplate;
            set => SetValue(SecondaryViewTemplateProperty, value);
        }

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public View TouchHandlerView
        {
            get => GetValue(TouchHandlerViewProperty) as View;
            set => SetValue(TouchHandlerViewProperty, value);
        }

        public bool IsTouchToExpandEnabled
        {
            get => (bool)GetValue(IsTouchToExpandEnabledProperty);
            set => SetValue(IsTouchToExpandEnabledProperty, value);
        }

        public double SecondaryViewHeightRequest
        {
            get => (double)GetValue(SecondaryViewHeightRequestProperty);
            set => SetValue(SecondaryViewHeightRequestProperty, value);
        }

        public uint ExpandAnimationLength
        {
            get => (uint)GetValue(ExpandAnimationLengthProperty);
            set => SetValue(ExpandAnimationLengthProperty, value);
        }

        public uint CollapseAnimationLength
        {
            get => (uint)GetValue(CollapseAnimationLengthProperty);
            set => SetValue(CollapseAnimationLengthProperty, value);
        }

        public Easing ExpandAnimationEasing
        {
            get => (Easing)GetValue(ExpandAnimationEasingProperty);
            set => SetValue(ExpandAnimationEasingProperty, value);
        }

        public Easing CollapseAnimationEasing
        {
            get => (Easing)GetValue(CollapseAnimationEasingProperty);
            set => SetValue(CollapseAnimationEasingProperty, value);
        }

        public ExpandStatus Status
        {
            get => (ExpandStatus)GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public ICommand Command
        {
            get => GetValue(CommandProperty) as ICommand;
            set => SetValue(CommandProperty, value);
        }

        public ICommand ForceUpdateSizeCommand
        {
            get => GetValue(ForceUpdateSizeCommandProperty) as ICommand;
            set => SetValue(ForceUpdateSizeCommandProperty, value);
        }

        public void ForceUpdateSize()
        {
            lastVisibleHeight = -1;
            OnIsExpandedChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            lastVisibleHeight = -1;
            SetSecondaryView(true);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Abs(width - previousWidth) >= double.Epsilon)
            {
                ForceUpdateSize();
            }
            previousWidth = width;
        }

#pragma warning disable S1541 // Methods and properties should not be too complex
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high

        private void OnIsExpandedChanged()
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            if (SecondaryView == null || (!IsExpanded && !SecondaryView.IsVisible))
            {
                return;
            }

            SecondaryView.SizeChanged -= OnSecondaryViewSizeChanged;

            var isExpanding = SecondaryView.AnimationIsRunning(ExpandAnimationName);
            SecondaryView.AbortAnimation(ExpandAnimationName);

            startHeight = SecondaryView.IsVisible
                ? Max(SecondaryView.Height - GetSecoundViewPadding(), 0)
                : 0;

            if (IsExpanded)
            {
                SecondaryView.IsVisible = true;
            }

            endHeight = SecondaryViewHeightRequest >= 0
                ? SecondaryViewHeightRequest
                : lastVisibleHeight;

            var shouldInvokeAnimation = true;

            if (IsExpanded)
            {
                if (endHeight <= 0)
                {
                    shouldInvokeAnimation = false;
                    SecondaryView.HeightRequest = -1;
                    SecondaryView.SizeChanged += OnSecondaryViewSizeChanged;
                }
            }
            else
            {
                lastVisibleHeight = startHeight = SecondaryViewHeightRequest >= 0
                        ? SecondaryViewHeightRequest
#pragma warning disable S3358 // Ternary operators should not be nested
                            : !isExpanding ? SecondaryView.Height
                                             - GetSecoundViewPadding() : lastVisibleHeight;
#pragma warning restore S3358 // Ternary operators should not be nested
                endHeight = 0;
            }

            shouldIgnoreAnimation = Height < 0;

            if (shouldInvokeAnimation)
            {
                InvokeAnimation();
            }
        }

        private double GetSecoundViewPadding()
        {
            return (SecondaryView is Layout l
                                                ? l.Padding.Top + l.Padding.Bottom
                                                : 0);
        }

        private void OnTouchHandlerViewChanged()
        {
            var gesturesList = (TouchHandlerView ?? PrimaryView)?.GestureRecognizers;
            gesturesList?.Remove(defaultTapGesture);
            PrimaryView?.GestureRecognizers.Remove(defaultTapGesture);
            gesturesList?.Add(defaultTapGesture);
        }

        private void SetPrimaryView(View oldView)
        {
            if (oldView != null)
            {
                Children.Remove(oldView);
            }
            Children.Insert(0, PrimaryView);
        }

        private void SetSecondaryView(bool forceUpdate = false)
        {
            if (IsExpanded && (SecondaryView == null || forceUpdate))
            {
                SecondaryView = CreateSecondaryView() ?? SecondaryView;
            }
            OnIsExpandedChanged();
        }

        private void SetSecondaryView(View oldView, View newView)
        {
            if (oldView != null)
            {
                oldView.SizeChanged -= OnSecondaryViewSizeChanged;
                Children.Remove(oldView);
            }
            if (newView != null)
            {
                if (newView is Layout layout)
                {
                    layout.IsClippedToBounds = true;
                }
                newView.HeightRequest = 0;
                newView.IsVisible = false;
                Children.Add(newView);
            }
            SetSecondaryView(true);
        }

        private View CreateSecondaryView()
        {
            var template = SecondaryViewTemplate;
            while (template is DataTemplateSelector selector)
            {
                template = selector.SelectTemplate(BindingContext, this);
            }
            if (template == previousTemplate && SecondaryView != null)
            {
                return null;
            }
            previousTemplate = template;
            return template?.CreateContent() as View;
        }

        private void OnSecondaryViewSizeChanged(object sender, EventArgs e)
        {
            if (SecondaryView.Height <= 0)
            {
                return;
            }

            SecondaryView.SizeChanged -= OnSecondaryViewSizeChanged;
            SecondaryView.HeightRequest = 0;
            endHeight = SecondaryView.Height;
            InvokeAnimation();
        }

        private void InvokeAnimation()
        {
            RaiseStatusChanged(IsExpanded ? ExpandStatus.Expanding : ExpandStatus.Collapsing);

            if (shouldIgnoreAnimation)
            {
                RaiseStatusChanged(IsExpanded ? ExpandStatus.Expanded : ExpandStatus.Collapsed);
                SecondaryView.HeightRequest = endHeight;
                SecondaryView.IsVisible = IsExpanded;
                return;
            }

            var length = ExpandAnimationLength;
            var easing = ExpandAnimationEasing;
            if (!IsExpanded)
            {
                length = CollapseAnimationLength;
                easing = CollapseAnimationEasing;
            }

            if (lastVisibleHeight > 0)
            {
                length = Max((uint)(length * (Abs(endHeight - startHeight) / lastVisibleHeight)), 1);
            }

            new Animation(v => SecondaryView.HeightRequest = v, startHeight, endHeight)
                .Commit(SecondaryView, ExpandAnimationName, 16, length, easing, (value, interrupted) =>
                {
                    if (interrupted)
                    {
                        return;
                    }
                    if (!IsExpanded)
                    {
                        SecondaryView.IsVisible = false;
                        RaiseStatusChanged(ExpandStatus.Collapsed);
                        return;
                    }
                    RaiseStatusChanged(ExpandStatus.Expanded);
                });
        }

        private void RaiseStatusChanged(ExpandStatus status)
        {
            Status = status;
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }
    }
}
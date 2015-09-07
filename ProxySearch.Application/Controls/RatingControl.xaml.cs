using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for RatingControl.xaml
    /// </summary>
    public partial class RatingControl : UserControl
    {
        private const int MaxValue = 5;

        public delegate void RatingValueChangedEventHandler(object sender, RatingValueChangedEventArgs e);

        public class RatingValueChangedEventArgs : RoutedEventArgs
        {
            public int OldValue { get; private set; }
            public int NewValue { get; private set; }

            public RatingValueChangedEventArgs(RoutedEvent routedEvent, int oldValue, int newValue)
                : base(routedEvent)
            {
                OldValue = oldValue;
                NewValue = newValue;
            }
        }


        public static readonly DependencyProperty RatingValueProperty =
            DependencyProperty.Register("RatingValue", typeof(int), typeof(RatingControl),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(RatingValueChangedInternal)));

        public static readonly RoutedEvent RatingValueChangedEvent = EventManager.RegisterRoutedEvent("RatingValueChanged", RoutingStrategy.Bubble, typeof(RatingValueChangedEventHandler), typeof(RatingControl));

        public event RatingValueChangedEventHandler RatingValueChanged
        {
            add { AddHandler(RatingValueChangedEvent, value); }
            remove { RemoveHandler(RatingValueChangedEvent, value); }
        }

        public int RatingValue
        {
            get { return (int)GetValue(RatingValueProperty); }
            set
            {
                if (value < 0)
                {
                    SetValue(RatingValueProperty, 0);
                }
                else if (value > MaxValue)
                {
                    SetValue(RatingValueProperty, MaxValue);
                }
                else
                {
                    SetValue(RatingValueProperty, value);
                }
            }
        }

        public RatingControl()
        {
            InitializeComponent();
        }

        private static void RatingValueChangedInternal(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RatingControl parent = (RatingControl)sender;

            int ratingValue = (int)e.NewValue;
            UIElementCollection children = ((Grid)(parent.Content)).Children;
            ToggleButton button = null;

            for (int i = 0; i < ratingValue; i++)
            {
                button = children[i] as ToggleButton;
                if (button != null)
                    button.IsChecked = true;
            }

            for (int i = ratingValue; i < children.Count; i++)
            {
                button = children[i] as ToggleButton;
                if (button != null)
                    button.IsChecked = false;
            }
        }

        private void RatingButtonClickEventHandler(Object sender, RoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;

            int oldRating = RatingValue;
            int newRating = int.Parse((String)button.Tag);

            if (button.IsChecked.Value || newRating < RatingValue)
            {
                RatingValue = newRating;
            }
            else
            {
                RatingValue = newRating - 1;
            }

            e.Handled = true;

            if (RatingValue != oldRating)
                RaiseEvent(new RatingValueChangedEventArgs(RatingValueChangedEvent, oldRating, RatingValue));
        }
    }
}
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProxySearch.Console.Code.UI;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for ResizeControl.xaml
    /// </summary>
    public partial class ResizeControl : UserControl
    {
        private FrameworkElement frameworkElement;
        private Point startDragPoint;
        private double originalWidth;
        private double originalHeight;

        public static readonly DependencyProperty IsGripEnabledProperty = 
            DependencyProperty.Register("IsGripEnabled", typeof(bool), typeof(ResizeControl), new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty IsAutoSizeEnabledProperty = 
            DependencyProperty.Register("IsAutoSizeEnabled", typeof(bool), typeof(ResizeControl), new FrameworkPropertyMetadata(true));

        public static readonly DependencyProperty ResizeDirectionProperty = 
            DependencyProperty.Register("ResizeDirection", typeof(ResizeDirection), typeof(ResizeControl), new FrameworkPropertyMetadata(ResizeDirection.BottomRight));

        public static readonly RoutedEvent SizeUpdatedEvent = EventManager.RegisterRoutedEvent("SizeUpdated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ResizeControl));

        public bool IsGripEnabled
        {
            get
            {
                return (bool)GetValue(IsGripEnabledProperty);
            }
            set
            {
                SetValue(IsGripEnabledProperty, value);
            }
        }

        public bool IsAutoSizeEnabled
        {
            get
            {
                return (bool)GetValue(IsAutoSizeEnabledProperty);
            }
            set
            {
                SetValue(IsAutoSizeEnabledProperty, value);
            }
        }

        public ResizeDirection ResizeDirection
        {
            get
            {
                return (ResizeDirection)GetValue(ResizeDirectionProperty);
            }
            set
            {
                SetValue(ResizeDirectionProperty, value);
            }
        }

        public event RoutedEventHandler SizeUpdated
        {
            add
            {
                AddHandler(SizeUpdatedEvent, value);
            }
            remove
            {
                RemoveHandler(SizeUpdatedEvent, value);
            }
        }

        public ResizeControl()
        {
            InitializeComponent();
        }

        private void UpdateSize()
        {
            if (frameworkElement != null)
            {
                Point point = frameworkElement.PointToScreen(Mouse.GetPosition(frameworkElement));
                double widthDelta = 0;
                double heightDelta = 0;

                switch (ResizeDirection)
                {
                    case ResizeDirection.TopRight:
                        widthDelta = point.X - startDragPoint.X;
                        heightDelta = startDragPoint.Y - point.Y;
                        break;
                    case ResizeDirection.TopLeft:
                        widthDelta = startDragPoint.X - point.X;
                        heightDelta = startDragPoint.Y - point.Y;
                        break;
                    case ResizeDirection.BottomRight:
                        widthDelta = point.X - startDragPoint.X;
                        heightDelta = point.Y - startDragPoint.Y;
                        break;
                    case ResizeDirection.BottomLeft:
                        widthDelta = startDragPoint.X - point.X;
                        heightDelta = point.Y - startDragPoint.Y;
                        break;
                    default:
                        throw new InvalidOperationException(string.Concat("Unexpected ResizeDirection: ", ResizeDirection));
                }

                Width = Math.Max(0, originalWidth + widthDelta);
                Height = Math.Max(0, originalHeight + heightDelta);

                RaiseEvent(new RoutedEventArgs(SizeUpdatedEvent));
            }
        }

        private void Grip_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                frameworkElement = (FrameworkElement)sender;

                startDragPoint = frameworkElement.PointToScreen(Mouse.GetPosition(frameworkElement));
                originalWidth = ActualWidth;
                originalHeight = ActualHeight;
                frameworkElement.CaptureMouse();
                e.Handled = true;
            }
        }

        private void Grip_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement resizeGrip = (FrameworkElement)sender;

            if (resizeGrip.IsMouseCaptured)
            {
                frameworkElement = null;
                resizeGrip.ReleaseMouseCapture();
                e.Handled = true;
            }
        }

        private void Grip_MouseMove(object sender, MouseEventArgs e)
        {
            if (((FrameworkElement)sender).IsMouseCaptured)
            {
                UpdateSize();
                e.Handled = true;
            }
        }

        private void Grip_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (IsAutoSizeEnabled)
                {
                    Width = double.NaN;
                    Height = double.NaN;
                }

                e.Handled = true;
            }
        }
    }
}

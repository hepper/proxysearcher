using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for TabNameControl.xaml
    /// </summary>
    public partial class TabNameControl : UserControl
    {
        private Label label = new Label();
        private TextBox textBox = new TextBox();

        public static RoutedEvent DeleteEvent = EventManager.RegisterRoutedEvent("Delete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabNameControl));
        public static RoutedEvent MenuEvent = EventManager.RegisterRoutedEvent("Menu", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabNameControl));
        public static readonly DependencyProperty TabNameProperty = DependencyProperty.Register("TabName", typeof(string), typeof(TabNameControl));
        private string oldName;

        public TabNameControl()
        {
            InitializeComponent();

            Binding binding = new Binding("TabName")
            {
                Source = this,
                Mode = BindingMode.TwoWay,
            };

            label = new Label();
            label.SetBinding(Label.ContentProperty, binding);            
            label.MouseDoubleClick += label_MouseDoubleClick;

            textBox.LostFocus += textBox_LostFocus;
            textBox.KeyDown += textBox_KeyDown;
            textBox.SetBinding(TextBox.TextProperty, binding);
        }

        public event RoutedEventHandler Delete
        {
            add
            {
                AddHandler(DeleteEvent, value);
            }
            remove
            {
                RemoveHandler(DeleteEvent, value);
            }
        }

        public event RoutedEventHandler Menu
        {
            add
            {
                AddHandler(MenuEvent, value);
            }
            remove
            {
                RemoveHandler(MenuEvent, value);
            }
        }

        public string TabName
        {
            get
            {
                return (string)GetValue(TabNameProperty);
            }
            set
            {
                SetValue(TabNameProperty, value);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (NameContent.Content == null)
                NameContent.Content = label;

            base.OnRender(drawingContext);
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            ActionButton.IsExpanded = false;
            ActionButton.ContextMenu.IsEnabled = true;
            ActionButton.ContextMenu.PlacementTarget = ActionButton;
            ActionButton.ContextMenu.Placement = PlacementMode.Bottom;
            ActionButton.ContextMenu.IsOpen = true;

            RaiseEvent(new RoutedEventArgs()
            {
                RoutedEvent = MenuEvent
            });
        }
        
        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            BeginRename();
        }

        private void label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BeginRename();
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            EndRename();
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EndRename();
            }
            else if (e.Key == Key.Escape)
            {
                CancelRename();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs()
            {
                RoutedEvent = DeleteEvent
            });
        }

        private void BeginRename()
        {
            oldName = TabName;
            NameContent.Content = textBox;

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() =>
            {
                textBox.Focus();
                textBox.SelectionStart = 0;
                textBox.SelectionLength = textBox.Text.Length;
            }));
        }

        private void EndRename()
        {
            NameContent.Content = label;
        }

        private void CancelRename()
        {
            textBox.Text = oldName;            
            EndRename();
        }
    }
}

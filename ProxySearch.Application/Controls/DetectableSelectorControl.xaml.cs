using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using System.ComponentModel;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for DetectableSelectorControl.xaml
    /// </summary>
    public partial class DetectableSelectorControl : UserControl
    {
        public static readonly DependencyProperty SelectorNameProperty = DependencyProperty.Register("SelectorName", typeof(string), typeof(DetectableSelectorControl));
        public static readonly DependencyProperty DetectablesProperty = DependencyProperty.Register("Detectables", typeof(List<IDetectable>), typeof(DetectableSelectorControl));
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int?), typeof(DetectableSelectorControl));
        public static readonly DependencyProperty ArgumentsProperty = DependencyProperty.Register("Arguments", typeof(List<ParametersPair>), typeof(DetectableSelectorControl));

        public DetectableSelectorControl()
        {
            InitializeComponent();

            DependencyPropertyDescriptor.FromProperty(DetectablesProperty, typeof(DetectableSelectorControl)).AddValueChanged(this, EventHandler);
            DependencyPropertyDescriptor.FromProperty(SelectedIndexProperty, typeof(DetectableSelectorControl)).AddValueChanged(this, EventHandler);
            DependencyPropertyDescriptor.FromProperty(ArgumentsProperty, typeof(DetectableSelectorControl)).AddValueChanged(this, EventHandler);
        }

        private void EventHandler(object sender, EventArgs e)
        {
            ExtendedControl.GetBindingExpression(ContentControl.ContentProperty).UpdateTarget();
        }

        public string SelectorName
        {
            get
            {
                return (string)this.GetValue(SelectorNameProperty);
            }
            set
            {
                this.SetValue(SelectorNameProperty, value);
            }
        }

        public List<IDetectable> Detectables
        {
            get
            {
                return (List<IDetectable>)this.GetValue(DetectablesProperty);
            }
            set
            {
                this.SetValue(DetectablesProperty, value);
            }
        }

        public int? SelectedIndex
        {
            get
            {
                return (int?)this.GetValue(SelectedIndexProperty);
            }
            set
            {
                this.SetValue(SelectedIndexProperty, value);
            }
        }

        public string SelectedDescription
        {
            get
            {
                if (Detectables == null || !SelectedIndex.HasValue)
                {
                    return null;
                }

                return Detectables[SelectedIndex.Value].Description;
            }
        }

        public List<ParametersPair> Arguments
        {
            get
            {
                return (List<ParametersPair>)GetValue(ArgumentsProperty);
            }
            set
            {
                SetValue(ArgumentsProperty, value);
            }
        }

        public UserControl UserControl
        {
            get
            {
                if (Detectables == null || Arguments == null || !SelectedIndex.HasValue)
                {
                    return null;
                }

                ParametersPair pair = Arguments.FirstOrDefault(item => item.TypeName == Detectables[SelectedIndex.Value].Implementation.AssemblyQualifiedName);
                List<object> parameters = (pair == null) ? Detectables[SelectedIndex.Value].DefaultSettings : pair.Parameters;

                if (Detectables[DetectableComboBox.SelectedIndex].PropertyPage == null)
                {
                    return null;
                }

                return (UserControl)Activator.CreateInstance(Detectables[DetectableComboBox.SelectedIndex].PropertyPage, parameters);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HelpText.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            ExtendedControl.GetBindingExpression(ContentControl.ContentProperty).UpdateTarget();
        }
    }
}

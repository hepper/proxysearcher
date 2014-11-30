using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Engine.Checkers;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for SearchSpeedControl.xaml
    /// </summary>
    public partial class SearchSpeedControl : UserControl, INotifyPropertyChanged
    {
        public SearchSpeedControl()
        {
            InitializeComponent();
        }

        public List<IDetectable> ProxyCheckers
        {
            get
            {
                string proxyType = Context.Get<AllSettings>().SelectedTabSettings.ProxyType;

                return Context.Get<IDetectableManager>()
                              .Find<IProxyChecker>(proxyType)
                              .OrderByDescending(item => item.Order)
                              .ToList();
            }
        }

        public int SelectedIndex
        {
            get
            {
                return ProxyCheckers.FindIndex(item => item.GetType() == SelectedValue.GetType());
            }
            set
            {
                SelectedValue = ProxyCheckers[value];
                FireChangedProperty("SelectedIndex");
            }
        }

        public IDetectable SelectedValue
        {
            get
            {
                string selectedProxyCheckerType = Context.Get<AllSettings>().SelectedTabSettings.ProxyCheckerDetectableType;
                return ProxyCheckers.Single(item => item.GetType().AssemblyQualifiedName == selectedProxyCheckerType);
            }
            set
            {
                Context.Get<AllSettings>().SelectedTabSettings.ProxyCheckerDetectableType = value.GetType().AssemblyQualifiedName;
                FireChangedProperty("SelectedValue");
            }
        }

        public void UpdateBindings()
        {
            FireChangedProperty("ProxyCheckers");
            FireChangedProperty("SelectedIndex");
            FireChangedProperty("SelectedValue");
        }

        private void FireChangedProperty(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

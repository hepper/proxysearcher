using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ProxySearch.Console.Code;
using ProxySearch.Console.Code.Detectable.SearchEngines;
using ProxySearch.Console.Code.Interfaces;
using ProxySearch.Console.Code.Settings;
using ProxySearch.Console.Code.UI;
using ProxySearch.Engine.SearchEngines;

namespace ProxySearch.Console.Controls
{
    /// <summary>
    /// Interaction logic for MixedSearchEnginePropertyControl.xaml
    /// </summary>
    public partial class ParallelSearchEnginePropertyControl : UserControl
    {
        public List<ParametersPair> Arguments
        {
            get;
            private set;
        }

        public List<MixedSearchEngineDataView> Data
        {
            get
            {
                int index = 1;
                return Arguments.Select(item => new MixedSearchEngineDataView(index++, item))
                                   .ToList();
            }
        }

        public ParallelSearchEnginePropertyControl(List<object> arguments)
        {
            Arguments = arguments.Cast<ParametersPair>().ToList();

            InitializeComponent();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Arguments.RemoveAt((int)((Button)sender).Tag - 1);
            UpdateData();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            IDetectable defaultDetectable = Context.Get<IDetectableManager>()
                                                   .Find<ISearchEngine>(Data.First().ProxyType)
                                                   .First();
            Arguments.Add(new ParametersPair 
            {
                TypeName = defaultDetectable.GetType().AssemblyQualifiedName,
                Parameters = ParallelSearchEngineDetectableBase.GetSettingsList(Data.First().ProxyType).Cast<object>().ToList()
            });

            UpdateData();
        }

        private List<IDetectable> ProxyTypes
        {
            get
            {
                return Context.Get<IDetectableManager>().Find<IProxyType>();
            }
        }

        private void UpdateData()
        {
            dataControl.GetBindingExpression(ItemsControl.ItemsSourceProperty).UpdateTarget();
        }
    }
}

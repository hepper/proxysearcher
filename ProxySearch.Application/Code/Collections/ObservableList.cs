using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySearch.Console.Code.Collections
{
    public class ObservableList<T> : List<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public new void Add(T item)
        {
            base.Add(item);

            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            FireCountChanged();
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);

            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            FireCountChanged();
        }

        public new void Clear()
        {
            base.Clear();

            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            FireCountChanged();
        }

        private void FireCollectionChanged(NotifyCollectionChangedEventArgs arguments)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, arguments);
            }
        }

        private void FireCountChanged()
        {
            FirePropertyChanged("Count");
        }

        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

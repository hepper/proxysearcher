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

        public new void AddRange(IEnumerable<T> collection)
        {
            base.AddRange(collection);

            foreach (T item in collection)
            {
                FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }

            FireCountChanged();
        }

        public new bool Remove(T item)
        {
            bool result = base.Remove(item);

            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            FireCountChanged();

            return result;
        }

        public void Reset()
        {
            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
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
            FirePropertyChanged(null);
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

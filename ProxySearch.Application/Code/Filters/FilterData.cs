using System;
using System.ComponentModel;

namespace ProxySearch.Console.Code.Filters
{
    public class FilterData : INotifyPropertyChanged
    {
        private IComparable data;
        public IComparable Data
        {
            get
            {
                return data;
            }
            set 
            {
                if (data == value)
                    return;

                data = value;

                FireChangedProperty("Data");
                FireChangedProperty("DataCount");
            }
        }

        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                if (count == value)
                    return;

                count = value;

                FireChangedProperty("Count");
                FireChangedProperty("DataCount");
            }
        }

        public string DataCount
        {
            get 
            {
                return string.Format("{0} [{1}]", Data, Count);
            }
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

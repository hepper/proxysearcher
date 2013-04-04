using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySearch.Engine.Proxies
{
    public class ProxyDetails : INotifyPropertyChanged
    {
        public ProxyDetails(object details)
        {
            Details = details;
            IsUpdating = false;
        }

        public ProxyDetails(object details, Func<ProxyInfo, CancellationTokenSource, Task<object>> updateMethod)
            : this(details)
        {
            UpdateMethod = updateMethod;
        }

        private object details;
        public object Details
        {
            get
            {
                return details;
            }
            set
            {
                details = value;
                FirePropertyChanged("Details");
            }
        }

        private bool isUpdating;
        public bool IsUpdating
        {
            get
            {
                return isUpdating;
            }

            set
            {
                isUpdating = value;

                if (isUpdating == true)
                    CancellationToken = new CancellationTokenSource();

                FirePropertyChanged("IsUpdating");
            }
        }

        public CancellationTokenSource CancellationToken
        {
            get;
            set;
        }

        public Func<ProxyInfo, CancellationTokenSource, Task<object>> UpdateMethod
        {
            get;
            private set;
        }

        protected void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

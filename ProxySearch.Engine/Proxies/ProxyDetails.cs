using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using ProxySearch.Engine.Tasks;

namespace ProxySearch.Engine.Proxies
{
    public class ProxyDetails : INotifyPropertyChanged
    {
        public ProxyDetails(ProxyTypeDetails details)
        {
            Details = details;
            IsUpdating = false;
        }

        public ProxyDetails(ProxyTypeDetails details, Func<ProxyInfo, TaskItem, CancellationTokenSource, Task<ProxyTypeDetails>> updateMethod)
            : this(details)
        {
            UpdateMethod = updateMethod;
        }

        private ProxyTypeDetails details;
        public ProxyTypeDetails Details
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

        public Func<ProxyInfo, TaskItem, CancellationTokenSource, Task<ProxyTypeDetails>> UpdateMethod
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

        public override string ToString()
        {
            if (Details != null)
                return Details.ToString();

            return string.Empty;
        }
    }
}

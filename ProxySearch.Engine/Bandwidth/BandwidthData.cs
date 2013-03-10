using System;
using System.ComponentModel;
using System.Threading;

namespace ProxySearch.Engine.Bandwidth
{
    public class BandwidthData : INotifyPropertyChanged, IComparable
    {
        public BandwidthData()
        {
            State = BandwidthState.Ready;

            Progress = 0;
            ResponseTime = 0;
            Bandwidth = 0;
        }

        private BandwidthState state;
        public BandwidthState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
                FireNotifyPropertyChanged("State");
            }
        }

        private int progress;
        public int Progress
        {
            get
            {
                return progress;
            }

            set
            {
                progress = value;
                FireNotifyPropertyChanged("Progress");
            }
        }

        private double? responseTime;
        public double? ResponseTime
        {
            get
            {
                return responseTime;
            }

            set
            {
                responseTime = value;
                FireNotifyPropertyChanged("ResponseTime");
            }
        }

        private double bandwidth;
        public double Bandwidth
        {
            get
            {
                return bandwidth;
            }

            set
            {
                bandwidth = value;
                FireNotifyPropertyChanged("Bandwidth");
            }
        }

        public CancellationTokenSource CancellationToken
        {
            get;
            set;
        }

        private void FireNotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(object obj)
        {
            BandwidthData data = obj as BandwidthData;

            if (data == null)
                return 1;

            if ((int)state != (int)data.State)
                return (int)state - (int)data.State;

            switch (state)
            {
                case BandwidthState.Ready:
                    return 0;
                case BandwidthState.Progress:
                    return data.Progress - Progress;
                case BandwidthState.Completed:
                    if (Bandwidth == data.Bandwidth)
                        return 0;

                    if (Bandwidth < data.Bandwidth)
                        return 1;

                    return -1;
                case BandwidthState.Error:
                    return 0;
            }

            throw new NotSupportedException();
        }
    }
}

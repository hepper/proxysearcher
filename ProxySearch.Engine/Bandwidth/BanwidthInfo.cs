using System;

namespace ProxySearch.Engine.Bandwidth
{
    public class BanwidthInfo
    {
        public DateTime BeginTime
        {
            get;
            set;
        }

        public DateTime FirstTime
        {
            get;
            set;
        }

        public long FirstCount
        {
            get;
            set;
        }

        public DateTime EndTime
        {
            get;
            set;
        }

        public long EndCount
        {
            get;
            set;
        }
    }
}

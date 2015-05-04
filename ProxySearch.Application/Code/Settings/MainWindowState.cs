using System;
using System.Collections.Generic;
using System.Windows;

namespace ProxySearch.Console.Code.Settings
{
    [Serializable]
    public class MainWindowState
    {
        public MainWindowState()
        {
            IsMaximized = false;
            Size = new Size(850, 600);
        }

        public bool IsMaximized
        {
            get;
            set;
        }

        public Point Location
        {
            get;
            set;
        }

        public Size Size
        {
            get;
            set;
        }
    }
}
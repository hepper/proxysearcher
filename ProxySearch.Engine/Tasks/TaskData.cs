using System.ComponentModel;

namespace ProxySearch.Engine.Tasks
{
    public class TaskData : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                FireChangedProperty("Name");
            }
        }

        private string details;
        public string Details
        {
            get
            {
                return details;
            }
            set
            {
                details = value;
                FireChangedProperty("Details");
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

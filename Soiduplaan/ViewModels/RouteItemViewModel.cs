using System;
using System.ComponentModel;

namespace Soiduplaan
{
    public class RouteItemViewModel : INotifyPropertyChanged
    {

        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private string _dueTime;

        public string DueTime
        {
            get
            {
                return _dueTime;
            }
            set
            {
                if (value != _dueTime)
                {
                    _dueTime = value;
                    NotifyPropertyChanged("DueTime");
                }
            }
        }

        private string _nextDeparture;

        public string NextDeparture
        {
            get
            {
                return _nextDeparture;
            }
            set
            {
                if (value != _nextDeparture)
                {
                    _nextDeparture = value;
                    NotifyPropertyChanged("NextDeparture");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}

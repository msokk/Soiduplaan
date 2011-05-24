using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Soiduplaan
{
    public class ScheduleItemViewModel : INotifyPropertyChanged
    {
        private string _departure;

        public string Departure
        {
            get
            {
                return _departure;
            }
            set
            {
                if (value != _departure)
                {
                    _departure = value;
                    NotifyPropertyChanged("Departure");
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

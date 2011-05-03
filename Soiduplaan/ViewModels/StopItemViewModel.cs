using System;
using System.ComponentModel;

namespace Soiduplaan.ViewModels
{
    public class StopItemViewModel : INotifyPropertyChanged
    {

        private string _iconUrl;

        public string IconUrl
        {
            get
            {
                return _iconUrl;
            }
            set
            {
                if (value != _iconUrl)
                {
                    _iconUrl = value;
                    NotifyPropertyChanged("IconUrl");
                }
            }
        }

        private string _number;

        public string Number
        {
            get
            {
                return _number;
            }
            set
            {
                if (value != _number)
                {
                    _number = value;
                    NotifyPropertyChanged("Number");
                }
            }
        }

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

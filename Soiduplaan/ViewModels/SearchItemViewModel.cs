using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace Soiduplaan
{
    public class SearchItemViewModel : INotifyPropertyChanged
    {
        private int _id;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("Id");
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

        private string _number;

        public string Number
        {
            get
            {
                return _number;
            }

            set
            {
                _number = value;
                NotifyPropertyChanged("Number");
            }
        }

        private string _iconUrl;

        public string IconUrl
        {
            get
            {
                return _iconUrl;
            }

            set
            {
                _iconUrl = value;
                NotifyPropertyChanged("IconUrl");
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
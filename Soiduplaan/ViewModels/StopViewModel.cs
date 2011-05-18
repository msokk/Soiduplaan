using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Device.Location;


namespace Soiduplaan
{
    public class StopViewModel : INotifyPropertyChanged
    {

        public StopViewModel()
        {
            _coordinates = CurrentLocation.Current.Location;         
        }


        public ObservableCollection<StopItemViewModel> RouteItems { get; private set; }

        private GeoCoordinate _coordinates;

        public GeoCoordinate Coordinates
        {
            get
            {
                return _coordinates;
            }
            set
            {
                if (value != _coordinates)
                {
                    _coordinates = value;
                    NotifyPropertyChanged("Coordinates");
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

using System;
using System.ComponentModel;
using System.Device.Location;

namespace Soiduplaan.ViewModels
{
    public class NearbyStopsViewModel : INotifyPropertyChanged
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

        private int _distance;

        public int Distance
        {
            get
            {
                return 0; //TODO pass coordinates through getDistance
            }
            set
            {
                if (value != _distance)
                {
                    _distance = value;
                    NotifyPropertyChanged("Distance");
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

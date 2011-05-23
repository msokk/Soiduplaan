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
            this.RouteItems = new ObservableCollection<StopItemViewModel>();
            _coordinates = CurrentLocation.Current.Location;
            Stop[] stops = Stop.LoadAll();
            for (int i=0;i<20;i++){
                string _iconUrl = "Images/" + "BusIcon.png";
                this.RouteItems.Add(new StopItemViewModel() { Title = "quack 17a 5554min", IconUrl = _iconUrl });
            }
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

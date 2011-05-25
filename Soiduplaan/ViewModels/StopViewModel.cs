using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Device.Location;


namespace Soiduplaan
{
    public class StopViewModel : INotifyPropertyChanged
    {

        public StopViewModel(int id)
        {
            //Vaja ümber teha kuidagi, sest suundi võib olla mitu
            //Ilmselt vaja teha list kus sees on kõik erinevad suunad (Collectionitega). Nupuvajutusel tuleb nendest järjest üle itereerida.
            this.RouteItems = new ObservableCollection<StopItemViewModel>();
            this.RouteItemsForward = new ObservableCollection<StopItemViewModel>();

            /*
            _coordinates = stop.SubStops[0].Coordinate;

            this.Title = stop.Title;
            for (int i = 0; i < 20; i++)
            {
                if (i < 10)
                {
                    this.RouteItemsForward.Add(new StopItemViewModel() { Title = "3 - Reisisadam (D-terminal)", DueTime = "160 min", IconUrl = "Images/BusIcon.png" });
                }
                else
                {
                    this.RouteItemsForward.Add(new StopItemViewModel() { Title = "17A - Reisisadam", DueTime = "6 min", IconUrl = "Images/TrammIcon.png" });
                }

            }
            this.RouteItems = this.RouteItemsForward;
             */
        }

        public void changeDirection()
        {
            //uus data laadida, listis kuvada.
            //this.RouteItems = this.UuteItemiteKollektsioon
        }

        public void changeDay()
        {
            //uus data laadida, listis kuvada.
            //this.RouteItems = this.UuteItemiteKollektsioon
        }

        public ObservableCollection<StopItemViewModel> RouteItemsForward { get; private set; }
        public ObservableCollection<StopItemViewModel> RouteItemsBackward { get; private set; }

        private ObservableCollection<StopItemViewModel> _routeItems;
        public ObservableCollection<StopItemViewModel> RouteItems
        {
            get
            {
                return _routeItems;
            }
            set
            {
                if (value != _routeItems)
                {
                    _routeItems = value;
                    NotifyPropertyChanged("RouteItems");
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

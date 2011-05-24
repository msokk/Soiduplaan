using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Device.Location;


namespace Soiduplaan
{
    public class RouteStopViewModel : INotifyPropertyChanged
    {

        public RouteStopViewModel()
        {
            this.Title = "Troll 3";
            this.Route = "Tedre - Kaubamaja";
            this.ScheduleItems = new ObservableCollection<ScheduleItemViewModel>();

            for (int i = 0; i < 20; i++)
            {
                if(i == 0)
                    this.ScheduleItems.Add(new ScheduleItemViewModel() { Departure = "16:20 (10 min)", IconUrl = "Images/TrollIcon.png" });
                else
                    this.ScheduleItems.Add(new ScheduleItemViewModel() { Departure = "16:20", IconUrl = "Images/TrollIcon.png"});
            }
        }

        public void changeDirection(bool backward)
        {
            //load new data
        }

        public void changeDay()
        {
            //load new data
        }

        public ObservableCollection<ScheduleItemViewModel> ScheduleItems { get; private set; }

        private string _route;

        public string Route
        {
            get
            {
                return _route;
            }
            set
            {
                if (value != _route)
                {
                    _route = value;
                    NotifyPropertyChanged("Route");
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
                if (value != _number)
                {
                    _number = value;
                    NotifyPropertyChanged("Number");
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

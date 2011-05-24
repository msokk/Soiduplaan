using System;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace Soiduplaan
{
    public class RouteViewModel : INotifyPropertyChanged
    {

        public RouteViewModel(int routeId)
        {
            /*Route route = Route.LoadById(routeId);
            Schedule[] schedules = Schedule.Load(route.Schedules[getDay(false)].Id);
            */
            this.RouteItemsForward = new ObservableCollection<RouteItemViewModel>();

            //laetakse algne data
            for (int i = 0; i <= 19; i++)
            {
                if (i == 0)
                    this.RouteItemsForward.Add(new RouteItemViewModel() { Title = "Reisisadam (D-terminal)", NextDeparture = "(12:30)", DueTime = "300 min" });
                else if (i == 1)
                    this.RouteItemsForward.Add(new RouteItemViewModel() { Title = "Keemia", NextDeparture = "(12:30)", DueTime = "300 min" });
                else if (i == 2)
                    this.RouteItemsForward.Add(new RouteItemViewModel() { Title = "Tallinn (Autobussijaam)", NextDeparture = "(12:30)", DueTime = "3 min" });
                else
                    this.RouteItemsForward.Add(new RouteItemViewModel() { Title = "Reisisadam (D-terminal)", NextDeparture = "(12:30)", DueTime = "3 min" });
            }

            this.Title = "Mustamäe - Kaubamaja";
            this.Vehicle = "Troll 3";
            this.RouteItems = this.RouteItemsForward;
        }

        private void reLoad(bool tom, int directionId)
        {
            
        }

        private int getDay(bool nextDay = false)
        {
            int today = (int)DateTime.Now.DayOfWeek-1;
            if (nextDay)
            {
                int next = today + 1;
                if (today + 1 > 6)
                {
                    next = 0;
                }
                return next;
            }
            else
            {
                return today;
            }
        }

        public void changeDirection(bool backward)
        {
            this.Title = (backward) ? "Kaubamaja - Mustamäe" : "Mustamäe - Kaubamaja";
            if (backward)
            {
                if (this.RouteItemsBackward == null)
                {
                    this.RouteItemsBackward = new ObservableCollection<RouteItemViewModel>();

                    //laetakse "teistpidi" data
                    for (int i = 0; i <= 19; i++)
                    {
                        this.RouteItemsBackward.Add(new RouteItemViewModel() { Title = "Kaubamaja", NextDeparture = "(11:30)", DueTime = "5 min" });
                    }
                }
                this.RouteItems = this.RouteItemsBackward;
            }
            else
            {
                this.RouteItems = this.RouteItemsForward;
            }
        }

        public void changeDay(bool tomorrow)
        {
            //ei teagi kuidas seda teha .. veel viimati kahte kollektsiooni vaja või??
            }

        public ObservableCollection<RouteItemViewModel> RouteItemsForward { get; private set; }
        public ObservableCollection<RouteItemViewModel> RouteItemsBackward { get; private set; }

        private ObservableCollection<RouteItemViewModel> _routeItems;

        public ObservableCollection<RouteItemViewModel> RouteItems
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

        private string _vehicle;

        public string Vehicle
        {
            get
            {
                return _vehicle;
            }
            set
            {
                if (value != _vehicle)
                {
                    _vehicle = value;
                    NotifyPropertyChanged("Vehicle");
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

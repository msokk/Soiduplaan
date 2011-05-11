using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Soiduplaan
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        public SearchViewModel()
        {
            this.AllItems = new ObservableCollection<SearchItemViewModel>();
            this.StopItems = new ObservableCollection<SearchItemViewModel>();
            this.RouteItems = new ObservableCollection<SearchItemViewModel>();
            Route[] routes = Route.LoadAll();
            foreach (var r in routes)
            {
                this.RouteItems.Add(new SearchItemViewModel() { Title = r.Vehicle + " " + r.Number + " - " + r.Title }); 
            }

            Stop[] stops = Stop.LoadAll();
            foreach (var s in stops)
            {
                this.StopItems.Add(new SearchItemViewModel() { Title = s.Title });
            }
        }

        public ObservableCollection<SearchItemViewModel> AllItems { get; private set; }
        public ObservableCollection<SearchItemViewModel> StopItems { get; private set; }
        public ObservableCollection<SearchItemViewModel> RouteItems { get; private set; }


        private string _input;

        public string Input
        {
            get
            {
                return _input;
            }
            set
            {
                if (value != _input)
                {
                    _input = value;
                    NotifyPropertyChanged("Input");
                }
            }
        }

        //TODO: Here be data loading with JSON.NET
        //And more



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
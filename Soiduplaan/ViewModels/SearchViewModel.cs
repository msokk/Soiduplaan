using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Collections.Generic;

namespace Soiduplaan
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        public SearchViewModel()
        {
            this.StopView = new CollectionViewSource();
            this.RouteView = new CollectionViewSource();
            this.StopItems = new ObservableCollection<SearchItemViewModel>();
            this.RouteItems = new ObservableCollection<SearchItemViewModel>();
            string[] types = { "bus", "trolleybus", "tram", "suburban_bus", "commercial_bus", "train" };

            for(int i = 0; i < types.Length; i++) {
                Data.fetchXML("routes", new Dictionary<string,string>() {
                  { "transport_id", types[i] }
                });
                Data.Done += new Data.XmlFetchEventHandler(Data_Done);
            }

            /*
            Stop[] stops = App.Stops;
            foreach (var s in stops)
            {
                string _iconUrl = "Images/" + "StopIcon.png";
                this.StopItems.Add(new SearchItemViewModel() { Title = s.Title, IconUrl = _iconUrl, Id = s.Id });
            }
            */
            RouteView.Source = RouteItems;
            this.PropertyChanged += new PropertyChangedEventHandler(SearchViewModel_PropertyChanged);
        }

        void Data_Done(object sender, Data.XmlFetchEventArgs e)
        {
            var routes = e.Xml.Descendants("route");
            foreach (var route in routes)
            {
                string iconName = "";
                switch (route.Element("vehicle").Value)
                {
                    case "Bus-p":
                        iconName = "Bus";
                        break;
                    case "Busexpress":
                        iconName = "Bus";
                        break;
                    default:
                        iconName = (route.Element("vehicle").Value.StartsWith("H")) ? "Marsa" : route.Element("vehicle").Value;
                        break;
                }
                string _iconUrl = "Images/" + iconName + "Icon.png";
                string title = route.Element("direction").Value;
                if (route.Element("number").Value != "")
                {
                    title = route.Element("number").Value + " - " + title;
                }


                string navigation = "schedule_id1=";

                this.RouteItems.Add(new SearchItemViewModel() { Title = title, IconUrl = _iconUrl });
            }
        }

        private void SearchViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Input")
            {
                if (Input == "")
                {
                    StopView.Source = null;
                    RouteView.Source = null;
                }
                else
                {
                    StopView.Source = StopItems;
                    RouteView.Source = RouteItems;
                    this.StopView.View.Filter = s =>
                    {
                        if (null == s) return true;
                        var sm = (SearchItemViewModel)s;
                        var meets = sm.Title.ToLowerInvariant().StartsWith(Input.ToLowerInvariant());
                        return meets;
                    };

                    this.RouteView.View.Filter = s =>
                    {
                        if (null == s) return true;
                        var sm = (SearchItemViewModel)s;
                        var meets = sm.Title.ToLowerInvariant().StartsWith(Input.ToLowerInvariant());
                        return meets;
                    };
                }
            }
        }


        void SearchView_Filter(object sender, FilterEventArgs e)
        {
            if (e.Item != null)
                e.Accepted = ((SearchItemViewModel)e.Item).Title.StartsWith(Input);
        }

        public ObservableCollection<SearchItemViewModel> StopItems { get; private set; }
        public ObservableCollection<SearchItemViewModel> RouteItems { get; private set; }
        public CollectionViewSource StopView { get; private set; }
        public CollectionViewSource RouteView { get; private set; }

        private string _input = "";

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
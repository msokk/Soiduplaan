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
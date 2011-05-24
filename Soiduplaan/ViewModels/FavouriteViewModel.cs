using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace Soiduplaan
{
    public class FavouriteViewModel : INotifyPropertyChanged
    {
        public FavouriteViewModel()
        {
            this.AllItems = new ObservableCollection<FavouriteItemViewModel>();
            this.StopItems = new ObservableCollection<FavouriteItemViewModel>();
            this.RouteItems = new ObservableCollection<FavouriteItemViewModel>();
            this.RouteStopItems = new ObservableCollection<FavouriteItemViewModel>();

            for (int i = 0; i < 20; i++) {

                this.AllItems.Add(new FavouriteItemViewModel() { Title = "Keemia", IconUrl = "stopIcon.png" });

            }


        }

        public ObservableCollection<FavouriteItemViewModel> AllItems { get; private set; }
        public ObservableCollection<FavouriteItemViewModel> StopItems { get; private set; }
        public ObservableCollection<FavouriteItemViewModel> RouteItems { get; private set; }
        public ObservableCollection<FavouriteItemViewModel> RouteStopItems { get; private set; }

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
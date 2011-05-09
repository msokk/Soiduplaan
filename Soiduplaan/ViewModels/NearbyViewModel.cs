using System;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace Soiduplaan
{
    public class NearbyViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NearbyStopsViewModel> NearbyStops { get; private set; }

        //TODO: Load all pins here from JSON


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

﻿using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Device.Location;


namespace Soiduplaan
{
    public class NearbyViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NearbyStopsViewModel> NearbyStops { get; private set; }

        public GeoCoordinate Current
        {
            get
            {
                if (CurrentLocation.Current.Location.IsUnknown)
                {
                    return null;
                }
                else
                {
                    return CurrentLocation.Current.Location;
                }
            }
        }


        public NearbyViewModel()
        {
            Stop[] stops = Stop.LoadAll();
            NearbyStops = new ObservableCollection<NearbyStopsViewModel>();
            for (int i = 0; i < stops.Length; i++)
            {
                for (int u = 0; u < stops[i].SubStops.Length; u++)
                {
                    double distance = CurrentLocation.getDistance(stops[i].SubStops[u].Coordinate);
                    if (distance != 0.0 && distance < 1500)
                    {
                        NearbyStops.Add(new NearbyStopsViewModel()
                        {
                            Title = stops[i].Title,
                            Coordinates = stops[i].SubStops[u].Coordinate,
                            Id = stops[i].SubStops[u].Id

                        });
                    }
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

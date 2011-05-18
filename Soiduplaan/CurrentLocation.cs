using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Device.Location;

namespace Soiduplaan
{
    public class CurrentLocation
    {
        private static GeoCoordinateWatcher watcher = null;
        private static GeoPosition<GeoCoordinate> current = null;

        public static GeoPosition<GeoCoordinate> Current
        {
            get 
            {
                if (watcher == null)
                {
                    watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                }
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
                watcher.Start();
                current = watcher.Position;
                watcher.Stop();
                return current;
            }
        }

        static void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            current = e.Position;
        }
            
    }
}

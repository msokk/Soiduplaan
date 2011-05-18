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

        private static double Deg2Rad(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private static double Rad2Deg(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        /// <summary>
        /// Calculates distance from your position (in metres)
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double getDistance(GeoCoordinate target)
        {
            double curLat = current.Location.Latitude;
            double curLon = current.Location.Longitude;
            double targetLat = target.Latitude;
            double targetLon = target.Longitude;

            double theta = curLon - targetLon;

            double distance = Math.Sin(Deg2Rad(curLat)) * Math.Sin(Deg2Rad(targetLat))
                + Math.Cos(Deg2Rad(curLat)) * Math.Cos(Deg2Rad(targetLat)) * Math.Cos(Deg2Rad(theta));

            distance = Math.Acos(distance);
            distance = Rad2Deg(distance);

            distance = distance * 60 * 1853.159616;


            return distance;
        }

            
    }
}

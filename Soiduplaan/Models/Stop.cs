using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Device.Location;

namespace Soiduplaan
{
    public class Stop
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
        }

        public class SubStop
        {
            private int _id;
            public int Id
            {
                get
                {
                    return _id;
                }
            }

            private GeoCoordinate _coord;
            public GeoCoordinate Coordinate
            {
                get
                {
                    return _coord;
                }
            }

            public SubStop(int id, GeoCoordinate coord)
            {
                _id = id;
                _coord = coord;
            }
        }

        private List<SubStop> _substops = new List<SubStop>();
        public SubStop[] SubStops
        {
            get
            {
                return _substops.ToArray();
            }
        }

        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
        }

        private void AddStop(int id, GeoCoordinate coord)
        {
            _substops.Add(new SubStop(id, coord));
        }



        public Stop(int id, string title)
        {
            _id = id;
            _title = title;
        }

        public static Stop[] LoadAll() 
        {
            JArray json = JArray.Parse(Data.loadJSON("stops.json"));
            List<Stop> stops = new List<Stop>();
            var i = 0;
            foreach(var s in json) {
                Stop tmpStop = new Stop(i, (string)s["title"]);

                foreach (var substop in s["stops"])
                {
                    int id = Int32.Parse((string)substop["id"]);
                    GeoCoordinate tmp = new GeoCoordinate(
                        Double.Parse((string)substop["lat"]),
                        Double.Parse((string)substop["lon"]));
                    tmpStop.AddStop(id, tmp);
                }

                stops.Add(tmpStop);
                i++;
            }

            return stops.ToArray();
        }

        public static Stop LoadById(int id)
        {
            Stop[] stops = LoadAll();
            return stops[id];
        }


    }
}

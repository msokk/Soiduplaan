using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Soiduplaan
{
    public class Route
    {
        private int _id;

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public class Schedule
        {
            private int _id;

            public int Id
            {
                get
                {
                    return _id;
                }
            }

            private int _direction_id;

            public int DirectionId
            {
                get
                {
                    return _direction_id;
                }
            }

            public DateTime _validfrom;

            public DateTime ValidFrom
            {
                get 
                {
                    return _validfrom;
                }
            }

            public Schedule(int id, int directionid, string validFrom)
            {
                _id = id;
                _direction_id = directionid;
                _validfrom = DateTime.Parse(validFrom);
            }
            
        }

        private string _number;

        public string Number
        {
            get
            {
                return _number;
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

        private string _vehicle;

        public string Vehicle
        {
            get
            {
                return _vehicle;
            }
        }

        private bool _lowfloor;

        public bool LowFloor
        {
            get
            {
                return _lowfloor;
            }
        }

        private Schedule[] _schedules = new Schedule[7];

        public Schedule[] Schedules
        {
            get
            {
                return _schedules;
            }
        }

        private void SetSchedule(int dayofweek, Schedule schedule)
        {
            _schedules[dayofweek] = schedule;
        }

        public Route(int id, string number, string title, bool lowfloor, string vehicle)
        {
            _id = id;
            _number = number;
            _title = title;
            _lowfloor = lowfloor;
            _vehicle = vehicle;
        }

        public static Route[] LoadAll() 
        {
            JArray json = JArray.Parse(Data.loadJSON("routes.json"));
            List<Route> routes = new List<Route>();
            var i = 0;
            foreach(var r in json) {
                bool lowfloor = ((string)r["lowfloor"] == "1")? true: false;
                string number = (r["number"].GetType() == typeof(JObject))? "": (string)r["number"];
                Route tmp = new Route(i, number, (string)r["title"], 
                    lowfloor, (string)r["vehicle"]);

                foreach (var sc in r["schedules"])
                {
                    tmp.SetSchedule((int)sc["day"], new Schedule(
                        Int32.Parse((string)sc["id"]),
                        Int32.Parse((string)sc["directionid"]),
                        (string)sc["validfrom"]
                    ));
                }

                routes.Add(tmp);
                i++;
            }

            return routes.ToArray();
        }

        public static Route LoadById(int id)
        {
            return App.Routes[id];
        }

        private Dictionary<string, string> vehicleNames = new Dictionary<string, string>()
        {
            { "Bus", "Buss" },
            { "Bus-p", "Buss" },
            { "Busexpress", "Ekspressbuss" },
            { "Harjull", "Harju" },
            { "Harjuvb", "Harju" },
            { "HBuss", "Harju" }
        };


        public string getRealName()
        {
            if (vehicleNames.ContainsKey(Vehicle))
            {
                return vehicleNames[Vehicle];
            }
            else
            {
                return Vehicle;
            }
        }
    }
}

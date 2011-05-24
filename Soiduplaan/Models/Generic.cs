using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace Soiduplaan
{
    public class GenericInfo
    {
        public class RouteInfo
        {
            private string _type;
            public string Type
            {
                get
                {
                    return _type;
                }
            }

            private int _count;
            public int Count
            {
                get
                {
                    return _count;
                }
            }

            private string _city;
            public string City
            {
                get
                {
                    return _city;
                }
            }

            public RouteInfo(string type, int count, string city)
            {
                _type = type;
                _count = count;
                _city = city;
            }
        }

        private List<RouteInfo> _routeinfo;

        public List<RouteInfo> RouteInfos
        {
            get
            {
                return _routeinfo;
            }
        }

        public GenericInfo()
        {
            _routeinfo = new List<RouteInfo>();
        }

        public static GenericInfo Load()
        {
            JObject json = JObject.Parse(Data.loadJSON("generic.json"));
            GenericInfo tmp =  new GenericInfo();

            var types = json["types"].Children();

            foreach (var t in types)
            {
                tmp._routeinfo.Add(new RouteInfo(
                    (string)t["type"], Int32.Parse((string)t["routes"]), (string)t["city"])
                );
            }
            return tmp;
        }

    }
}

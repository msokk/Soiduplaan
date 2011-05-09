using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Soiduplaan.Models
{
    public class Stop
    {
        
        public Stop()
        {

        }

        public static Stop[] LoadAll() 
        {
            JArray json = JArray.Parse(Data.loadJSON("stops.json"));
            List<Stop> stops = new List<Stop>();
            
            foreach(var s in json) {

                stops.Add(new Stop());
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

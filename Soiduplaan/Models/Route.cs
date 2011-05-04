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
using Newtonsoft.Json.Linq;

namespace Soiduplaan.Models
{
    public class Route
    {
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
            _schedules[(dayofweek-1)] = schedule;
        }

        public Route(string number, string title, bool lowfloor, string vehicle)
        {
            _number = number;
            _title = title;
            _lowfloor = lowfloor;
            _vehicle = vehicle;
        }

        public static Route[] LoadAll() {
            Route test = new Route("212A", "Kiviõli - Kivi", false, "Buss");
            test.SetSchedule(1, new Schedule(1, 1, "2011.01.01"));

            //Load file, parse json, 
            JObject json = JObject.Parse("{}");

            return null;
        }


    }
}

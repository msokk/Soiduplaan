﻿using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Soiduplaan.Models
{
    public class Schedule
    {

        public Schedule()
        {

        }

        public static Schedule Load(int id, int directionid)
        {
            JArray json = JArray.Parse(Data.loadJSON("schedules.json"));
            return null;
        }


    }
}
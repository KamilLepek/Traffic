﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Traffic.Utilities
{
    public static class Constants
    {

        //Driver attribute constants
        public const float MaximumVelocity = 120; // km/h
        public const float MaximumReactionTime = 500; // ms
        public const float MinimumDistanceHeld = 25; // m

        //Map constants
        public const float StreetWidth = 10; //m
        public const float StreetLength = 250; //m

        public const float IntersectionSize = 10; //m width/height
    }
}
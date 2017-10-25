using System;
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
        public const float InitialVelocity = 30; //km/h

        //Map constants
        public const float StreetWidth = 5; //m
        public const float StreetLength = 20; //m
        public const float IntersectionSize = 15; //m width/height

        //Time interval in which we spawn new vehicles if there is a need
        public const int TimeSpawnInterval = 2000; //ms

        public const string LogFile = @"logs.txt";

        //Car Lenght/width
        public const float CarLenght = 5; //m
        public const float CarWidth = 2; //m

        public enum Decision { Forward, Left, Right };
        public enum Orientation { Top, Right, Bottom, Left };

        //Graphics display constants
        public const float CameraMovementSpeed = 10.0f;
        public const float CameraZoomSpeed = 15.0f;
    }
}

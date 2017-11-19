namespace Traffic.Utilities
{
    public static class Constants
    {
        public const double DoubleErrorTolerance = 0.0001;

        //Driver attribute constants
        public const double MinimumMaximalVelocity = 40;
        public const double MaximumVelocity = 120;
        public const double MaximumReactionTime = 500;
        public const double MinimumDistanceHeld = 3; //m
        public const double DistanceHeldInterval = 5; //m
        public const double InitialVelocity = 30;
        public const double BeforeEnteringIntersectionDesiredVelocity = 40;
        public const double IntersectionDesiredVelocity = 25;
        public const double DriverAcceleratingOnStraightRoadMultiplier = 15;
        public const double VelocityDeceleratingFactorOnStreet = 0.8;
        public const double VelocityDeceleratingFactorOnIntersection = 1.2;
        public const double VelocityDependentCaution = 5;
        public const double VelocityDifferenceDependentCaution = 20;
        public const double VelocityDifferenceDeceleratingFactor = 2;
        public const double DistanceDifferenceDeceleratingFactor = 1;
        public const double DistanceToOmmitDistanceDifferenceDeceleratingFactor = 100;
        public const double DesiredVelocityMargin = 5;

        //Map constants
        public const double StreetWidth = 5; //m
        public const double StreetLength = 500; //m
        public const double IntersectionSize = 75; //m width/height
        public const double TrafficLightWidth = 3; //m
        public const double TrafficLightHeight = 7; //m

        //Time interval in which we spawn new vehicles if there is a need
        public const int TimeSpawnInterval = 1000; //ms

        public const int MinLightChangeInterval = 20; // s
        public const int MaxLightChangeInterval = 60; // s
        public const int AllLightsRedTime = 3; //s

        public const int TicksPerSecond = 120;

        public const string LogFile = @"logs.txt";

        public const double MaximumVehicleAngle = 30; //to check if we go in a specified direction +- this degrees, make sure it stays below 45 to make sense

        //Car Length/width
        public const double CarLength = 5; //m
        public const double CarWidth = 2; //m

        //Graphics display constants
        public const double CameraKeysMovementSpeed = 0.1;
        public const double CameraMouseMovementSpeed = 0.003;
        public const double CameraZoomSpeed = 40.0;
        public const double InitialCameraDistance = -300.0;
        public const int AmountOfLinesInCircle = 50;

        // Maneuver constants

        // how far (fraction of intersection size) from the intersection center should the driver start turning
        public const double TurnStartingPoint = 0.07;

        // how far (fraction of street length) from the beginning of street should the driver start breaking
        public const double BreakStartingPoint = 0.85;

        public const int NumberOfTextures = 5;
    }
}

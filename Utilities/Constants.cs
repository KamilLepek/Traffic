using System;

namespace Traffic.Utilities
{
    public static class Constants
    {
        #region Approximation handling

        public const double DoubleErrorTolerance = 0.0001;

        #endregion

        #region Vehicle related attributes constants

        #region Acceleration and deceleration constants

        public const double DriverAcceleratingOnStraightRoadMultiplier = 15;

        public const double VelocityDeceleratingFactorOnStreet = 0.8;

        public const double VelocityDeceleratingFactorOnIntersection = 1.2;

        public const double TrafficLightsDeceleration = 160;

        /// <summary>
        /// Parameter which determines how important is our velocity when computing search area in front of us for potential collision detection
        /// </summary>
        public const double VelocityDependentCaution = 5;

        /// <summary>
        /// Parameter which determines how important is velocity difference between 2 vehicles when computing 
        /// search area in front of us for potential collision detection
        /// </summary>
        public const double VelocityDifferenceDependentCaution = 40;

        /// <summary>
        /// Parameter which determines how important is velocity difference between 2 vehicles when computing
        /// deceleration to avoid collision
        /// </summary>
        public const double VelocityDifferenceDeceleratingFactor = 2;

        /// <summary>
        /// Parameter which determines how important is distance difference between 2 vehicles when computing
        /// deceleration to avoid collision
        /// </summary>
        public const double DistanceDifferenceDeceleratingFactor = 1;

        /// <summary>
        /// If distance between 2 vehicles is higher than this then we ommit <see cref="DistanceDifferenceDeceleratingFactor"/>
        /// </summary>
        public const double DistanceToOmmitDistanceDifferenceDeceleratingFactor = 100;

        #endregion

        #region Velocity constants

        /// <summary>
        /// We neither have to decelerate nor accelerate if our velicity is in [desiredVelocity - this, desiredVelocity]
        /// </summary>
        public const double DesiredVelocityMargin = 5;

        /// <summary>
        /// This is the minimum value that a vehicle can have as maximum reachable velocity
        /// </summary>
        public const double MinimumMaximalVelocity = 40;

        /// <summary>
        /// This is the maximum value that a vehicle can have as maximum reachable velocity
        /// </summary>
        public const double MaximumVelocity = 120;

        public const double InitialVelocity = 30;

        public const double BeforeEnteringIntersectionDesiredVelocity = 40;

        public const double IntersectionDesiredVelocity = 30;

        #endregion

        #region Vehicles sizes

        public const double CarLength = 5;

        public const double CarWidth = 2;

        #endregion

        #region Other constants

        [Obsolete]//We have to decide whether this is redundant
        public const double MaximumReactionTime = 500;

        /// <summary>
        /// Minimum value that is acceptable for distance held from the vehicle in front
        /// </summary>
        public const double MinimumDistanceHeld = 0.5;

        /// <summary>
        /// Maximum value that is acceptable for distance held from the vehicle in front of us is <see cref="MinimumDistanceHeld"/> + <see cref="DistanceHeldInterval"/>
        /// </summary>
        public const double DistanceHeldInterval = 4;

        #endregion

        #endregion

        #region Map constants

        public const double StreetWidth = 5;

        public const double StreetLength = 500;

        public const double IntersectionSize = 75; //both for width and height

        public const double TrafficLightWidth = 3;

        public const double TrafficLightHeight = 7;

        /// <summary>
        /// How far (fraction of intersection size) from the intersection center should the driver start turning
        /// </summary>
        public const double TurnStartingPoint = 0.07;

        /// <summary>
        /// How far (fraction of street length) from the beginning of street should the driver start breaking
        /// </summary>
        public const double BreakStartingPoint = 0.85;

        #endregion

        #region Time related constants

        /// <summary>
        /// Time interval in which we spawn new vehicles if there is a need
        /// </summary>
        public const int TimeSpawnInterval = 1000; //ms

        public const int MinLightChangeInterval = 20; // s

        public const int MaxLightChangeInterval = 60; // s

        public const int AllLightsRedTime = 3; //s

        public const int TicksPerSecond = 120;

        #endregion

        #region Graphics display constants

        public const double CameraKeysMovementSpeed = 0.1;

        public const double CameraMouseMovementSpeed = 0.003;

        public const double CameraZoomSpeed = 40.0;

        public const double InitialCameraDistance = -300.0;

        public const int AmountOfLinesInCircle = 50;

        #endregion

        #region Other constants

        /// <summary>
        /// Name of the file we log some data to
        /// </summary>
        public const string LogFile = @"logs.txt";

        public const double MaximumVehicleAngle = 30; //to check if we go in a specified direction +- this degrees, make sure it stays below 45 to make sense

        public const int NumberOfTextures = 5;

        #endregion
    }
}

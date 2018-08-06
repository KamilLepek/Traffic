namespace Traffic.Utilities
{
    /// <summary>
    /// Decision type depending which way we want to drive on the next intersection
    /// </summary>
    public enum Decision { Forward, Left, Right }

    /// <summary>
    /// Type determining whether we are on the top, right, bottom or left side od the map
    /// </summary>
    public enum Orientation { Top, Right, Bottom, Left }

    /// <summary>
    /// Maneuvers which driver can execute during his journey
    /// </summary>
    public enum Maneuver { Accelerate, DecelerateOnStreet, DecelerateOnIntersection, ForwardOnIntersect, TurnLeft, 
        TurnRight, CorrectAfterTurning, AvoidCollision, StopOnLights, WaitToEnterIntersection}

    /// <summary>
    /// Traffic Light colors
    /// </summary>
    public enum Light { Red, Green }
}

namespace Traffic.Utilities
{
    public enum Decision { Forward, Left, Right }
    public enum Orientation { Top, Right, Bottom, Left }
    public enum Maneuver { Accelerate, DecelerateOnStreet, DecelerateOnIntersection, ForwardOnIntersect, TurnLeft, TurnRight, CorrectAfterTurning, AvoidCollision}
    public enum Light { Red, Green }
}

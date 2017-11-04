namespace Traffic.Utilities
{
    public static class UnitConverter
    {
        public static void OrientationToRowColumnDiffrence(Orientation or, ref int horizontal, ref int vertical)
        {
            switch (or)
            {
                case Orientation.Bottom:
                    horizontal = 0;
                    vertical = 1;
                    break;
                case Orientation.Left:
                    horizontal = -1;
                    vertical = 0;
                    break;
                case Orientation.Right:
                    horizontal = 1;
                    vertical = 0;
                    break;
                case Orientation.Top:
                    horizontal = 0;
                    vertical = -1;
                    break;
                default:
                    horizontal = 0;
                    vertical = 0;
                    break;
            }
        }
    }
}

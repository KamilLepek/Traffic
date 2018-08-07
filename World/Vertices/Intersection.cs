using System.Collections.Generic;
using OpenTK;
using Traffic.Utilities;
using Traffic.World.Edges;

namespace Traffic.World.Vertices
{
    /// <summary>
    /// Represents intersections in our simulation
    /// </summary>
    public class Intersection : AbstractVertex
    {

        /// <summary>
        ///     Streets that come out of this intersection
        /// </summary>
        public List<Street> IntersectingStreets { get; }
        public Light VerticalTrafficLight { get; private set; }
        public Light HorizontalTrafficLight { get; private set; }

        private readonly int lightChangeInterval;
        private int lightChangeTimer;

        /// <summary>
        ///     If it's true, while all the lights are red, then next green lights will be on horizontal road, else on vertical.
        /// </summary>
        private bool horizontalLightShouldBeGreen;

        public Intersection(int row, int column) : base(row, column)
        {
            this.IntersectingStreets = new List<Street>();
            this.VerticalTrafficLight = (Light)RandomGenerator.Int(2);
            this.HorizontalTrafficLight = this.VerticalTrafficLight == Light.Green ? Light.Red : Light.Green;
            this.horizontalLightShouldBeGreen = this.HorizontalTrafficLight == Light.Green;
            this.lightChangeTimer = Constants.AllLightsRedTime * Constants.TicksPerSecond;
            this.lightChangeInterval =
                RandomGenerator.Int(Constants.MinLightChangeInterval, Constants.MaxLightChangeInterval + 1) *
                Constants.TicksPerSecond;
        }

        /// <summary>
        /// Traffic lights changing method
        /// </summary>
        public void PerformTimerTick()
        {
            this.lightChangeTimer = (this.lightChangeTimer + 1) % this.lightChangeInterval;
            if (this.lightChangeTimer == 0)
            {
                this.HorizontalTrafficLight = Light.Red;
                this.VerticalTrafficLight = Light.Red;
                this.horizontalLightShouldBeGreen = !this.horizontalLightShouldBeGreen;
            }
            else if (this.lightChangeTimer == Constants.AllLightsRedTime * Constants.TicksPerSecond)
            {
                if (this.horizontalLightShouldBeGreen)
                    this.HorizontalTrafficLight = Light.Green;
                else
                {
                    this.VerticalTrafficLight = Light.Green;
                }
            }
        }

        /// <summary>
        /// Gets traffic light color
        /// </summary>
        /// <param name="or">Orientation of the traffic light</param>
        /// <returns>Traffic light color</returns>
        public Light GetTrafficLight(Orientation or)
        {
            if (or == Orientation.Bottom || or == Orientation.Top)
                return this.VerticalTrafficLight;
            else
                return this.HorizontalTrafficLight;
        }

        public override Vector2 GetCoordinates()
        {
            var coordinates = new Vector2
            {
                X = (float) ((this.ColumnNumber / 2) * Constants.StreetLength
                             + ((this.ColumnNumber / 2) - 1) * Constants.IntersectionSize
                             + Constants.IntersectionSize / 2),
                Y = (float) ((this.RowNumber / 2) * Constants.StreetLength
                             + ((this.RowNumber / 2) - 1) * Constants.IntersectionSize
                             + Constants.IntersectionSize / 2)
            };

            return coordinates;
        }
    }
}

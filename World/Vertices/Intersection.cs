using System.Collections.Generic;
using Traffic.Utilities;
using Traffic.World.Edges;

namespace Traffic.World.Vertices
{
    public class Intersection : AbstractVertex
    {

        /// <summary>
        /// Streets that come out of this intersection
        /// </summary>
        public List<Street> IntersectingStreets { get; private set; }
        public Light VerticalTrafficLight { get; private set; }
        public Light HorizontalTrafficLight { get; private set; }

        private int lightChangeInterval;
        private int lightChangeTimer;

        /// <summary>
        /// If it's true, while all the lights are red, then next green lights will be on horizontal road, else on vertical.
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
    }
}

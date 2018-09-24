using System;

namespace Traffic.Graphics
{
    class FpsCounter
    {
        /// <summary>
        /// Variable which stores amount of rendered frames.
        /// </summary>
        private double frameCount;

        /// <summary>
        /// Variable which stores how many frames were rendered in 1 sec interval.
        /// </summary>
        public double Fps { get; private set; }

        /// <summary>
        /// Variable which stores amount of time that passed since computer was turned on in milliseconds.
        /// Used to determine whether 1 second elapsed.
        /// </summary>
        private double lastTick;

        public void OnNewFrame()
        {
            this.frameCount++;
            if (Environment.TickCount - this.lastTick >= 1000)  //1000ms
            {
                this.Fps = (this.frameCount/(Environment.TickCount - this.lastTick))*1000; //1000ms
                this.frameCount = 0;
                this.lastTick = Environment.TickCount;
            }
        }
    }
}

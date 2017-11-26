using Traffic.Graphics;
using Traffic.Physics;
using Traffic.Utilities;

namespace Traffic
{
    internal class Traffic
    {
        private static void Main(string[] args)
        {
            int verticalLines = 3;
            int horizontalLines = 3;
            int desiredNumberOfVehicles = 1000;

            var simulationController =
                new SimulationController(horizontalLines, verticalLines, desiredNumberOfVehicles);
            simulationController.InitSimulation();

            var graphicsController = new GraphicsController(simulationController.World,
                simulationController.PerformSimulationTick);
            graphicsController.Run(Constants.TicksPerSecond);
        }
    }
}
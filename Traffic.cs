using Traffic.Graphics;
using Traffic.Physics;
using Traffic.Utilities;

namespace Traffic
{
    internal class Traffic
    {
        private static void Main(string[] args)
        {
            int verticalLines = 4;
            int horizontalLines = 4;
            int desiredNumberOfVehicles = 300;

            var simulationController =
                new SimulationController(horizontalLines, verticalLines, desiredNumberOfVehicles);
            simulationController.InitSimulation();

            var graphicsController = new GraphicsController(simulationController.World,
                simulationController.PerformSimulationTick);
            graphicsController.Run(Constants.TicksPerSecond);
        }
    }
}
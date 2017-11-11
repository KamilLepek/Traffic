using Traffic.Graphics;
using Traffic.Physics;
using Traffic.Utilities;

namespace Traffic
{
    class Traffic
    {

        static void Main(string[] args)
        {
            int verticalLines = 5;
            int horizontalLines = 5;
            int desiredNumberOfVehicles = 1000;

            var simulationController = new SimulationController(horizontalLines, verticalLines, desiredNumberOfVehicles);
            simulationController.InitSimulation();

            var graphicsController = new GraphicsController(simulationController.World, simulationController.PerformSimulationTick);
            graphicsController.Run(Constants.TicksPerSecond);
        }
    }
}

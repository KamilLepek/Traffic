using Traffic.Graphics;
using Traffic.Physics;
using Traffic.Utilities;

namespace Traffic
{
    class Traffic
    {

        static void Main(string[] args)
        {
            int verticalLines = 4;
            int horizontalLines = 3;
            int desiredNumberOfVehicles = 1;

            var simulationController = new SimulationController(horizontalLines, verticalLines, desiredNumberOfVehicles);
            simulationController.InitSimulation();

            var graphicsController = new GraphicsController(simulationController.World, simulationController.PerformSimulationTick);
            graphicsController.Run(Constants.TicksPerSecond);
        }
    }
}

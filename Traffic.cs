using Traffic.Graphics;
using Traffic.Physics;

namespace Traffic
{
    class Traffic
    {

        static void Main(string[] args)
        {
            int verticalLines = 4;
            int horizontalLines = 3;
            int desiredNumberOfVehicles = 2 * (verticalLines + horizontalLines);

            var simulationController = new SimulationController(horizontalLines, verticalLines, desiredNumberOfVehicles);
            simulationController.InitSimulation();

            var graphicsController = new GraphicsController(simulationController.World, simulationController.PerformSimulationTick);
            graphicsController.Run(60.0); // 60 updatów świata na sekundę, tyle fpsów ile wyrobi
        }
    }
}

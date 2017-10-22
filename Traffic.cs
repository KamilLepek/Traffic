using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.Vehicles;
using Traffic.Utilities;
using Traffic.World;
using Traffic.Graphics;
using Traffic.Physics;

namespace Traffic
{
    class Traffic
    {

        static void Main(string[] args)
        {
            int desiredNumberOfVehicles = 90;
            int verticalLines = 2;
            int horizontalLines = 2;

            var controller = new SimulationController(horizontalLines, verticalLines, desiredNumberOfVehicles);

            var temporaryRysowanie = new ConsolePreview(controller.World); // will be changed dramatically in the future
            
            controller.HandleSimulation();
        }
    }
}

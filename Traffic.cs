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
            ConsoleLogger.DeleteLogs();
            int desiredNumberOfVehicles = 500;
            var world = new Map(2, 2, desiredNumberOfVehicles);
            var hehe = new ConsolePreview(world);
            int amountOfSpawnPoints = world.SpawnPoints.Count;
            var spawner = new VehicleGenerator(world);
            var controller = new Controller(world, spawner);
            
            if(desiredNumberOfVehicles > amountOfSpawnPoints)
                spawner.GenerateRandomVehicles(world.Vehicles, amountOfSpawnPoints);
            else
                spawner.GenerateRandomVehicles(world.Vehicles, desiredNumberOfVehicles);

            controller.HandleEverything();
        }
    }
}

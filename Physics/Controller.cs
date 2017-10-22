using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.World;
using Traffic.World.Vertices;
using Traffic.Vehicles;
using Traffic.Utilities;
using System.Diagnostics;

namespace Traffic.Physics
{
    public class Controller
    {

        public Map World { get; private set; }
        public VehicleGenerator Gen { get; private set; }
        public bool IsSpawningAllowed { get; private set; }


        public Controller(Map map, VehicleGenerator gen)
        {
            this.IsSpawningAllowed = false;
            this.World = map;
            this.Gen = gen;
        }

        /// <summary>
        /// Main method of the solution
        /// </summary>
        public void HandleEverything()
        {
            this.World.sw.Start();
            while (true)
            {
                this.HandleSpawnPoints();
                this.HandleSpawning();
            }
        }

        /// <summary>
        /// Moves vehicles from spawn point to street if necessary
        /// </summary>
        private void HandleSpawnPoints()
        {
            //Póki co tylko przesuwamy pojazdy po zrespieniu ze spawn pointu (który nie ma wymiarów), na drogę, która z nim graniczy (żeby przetestować funkcjonalności spawnera)
            //Później jak już będą jeździć to tutaj trzeba będzie ohandlować czy ten co jest na endpoincie to ma zostać zaraz usunięty cyz on się dopiero co zrespił
            foreach (var vehicle in this.World.Vehicles)
            {
                //if vehicle is on spawn point
                if (vehicle.Place.GetType() == typeof(EndPoint))
                {
                    var spawn = (EndPoint)vehicle.Place;
                    this.MoveToStreetFromSpawn(vehicle);
                    spawn.IsOccupied = false;
                }
            }
        }

        /// <summary>
        /// Spawns new vehicles if it is necessary
        /// </summary>
        private void HandleSpawning()
        {
            // Mechanism to spawn new vehicles once per Constants.TimeSpawnInterval s
            long time = this.World.sw.ElapsedMilliseconds % Constants.TimeSpawnInterval;
            if (time < 1 && this.IsSpawningAllowed)
            {
                this.Gen.VehiclesSpawner(World.Vehicles, World.DesiredAmountOfVehicles, World.NumberOfSpawnPoints);
                this.IsSpawningAllowed = false;
                ConsoleLogger.Log("Time " + this.World.sw.ElapsedMilliseconds + "ms");
            }
            else if (time >= 1 && !this.IsSpawningAllowed)
                this.IsSpawningAllowed = true;
        }

        /// <summary>
        /// Moves vehicle from spawn point to street
        /// </summary>
        private void MoveToStreetFromSpawn(Vehicle vehicle)
        {
            ConsoleLogger.Log("Moved vehicle from r:" + vehicle.Place.RowNumber + " c:" + vehicle.Place.ColumnNumber +
                " to r:" + ((EndPoint)vehicle.Place).Street.RowNumber + " c:" + ((EndPoint)vehicle.Place).Street.ColumnNumber);
            vehicle.Place = ((EndPoint)vehicle.Place).Street;
        }
    }
}

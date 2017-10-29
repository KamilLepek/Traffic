using Traffic.World;
using Traffic.World.Vertices;
using Traffic.Vehicles;
using Traffic.Utilities;

namespace Traffic.Physics
{
    public class SimulationController
    {

        public Map World { get; private set; }
        public VehicleGenerator Gen { get; private set; }
        public bool IsSpawningAllowed { get; private set; }

        public SimulationController(int horiz, int vert, int vehicles)
        {
            ConsoleLogger.DeleteLogs();
            this.World = new Map(horiz, vert, vehicles);
            this.Gen = new VehicleGenerator(this.World);
        }

        /// <summary>
        /// Initializes simulation
        /// </summary>
        public void InitSimulation()
        {
            this.World.sw.Start();
            this.Gen.VehiclesSpawner(this.World.Vehicles, this.World.DesiredAmountOfVehicles, this.World.SpawnPoints.Count);
        }

        /// <summary>
        /// Single simulation tick
        /// </summary>
        public void PerformSimulationTick()
        {
            this.HandleSpawnPoints();
            this.HandleSpawning();
            this.MoveVehicles();
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
            if (time < 10 && this.IsSpawningAllowed)
            {
                //ConsoleLogger.Log("Time " + this.World.sw.ElapsedMilliseconds + "ms");
                this.Gen.VehiclesSpawner(this.World.Vehicles, this.World.DesiredAmountOfVehicles, this.World.SpawnPoints.Count);
                this.IsSpawningAllowed = false;
            }
            else if (time >= 10 && !this.IsSpawningAllowed)
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

        /// <summary>
        /// Handles vehicles movement
        /// </summary>
        private void MoveVehicles()
        {

        }
    }
}

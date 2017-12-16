using Traffic.World;
using Traffic.World.Vertices;
using Traffic.Vehicles;
using Traffic.Utilities;

namespace Traffic.Physics
{
    /// <summary>
    /// Main class controlling the simulation
    /// </summary>
    public class SimulationController
    {

        public readonly Map World;
        private readonly VehicleGenerator vehicleGenerator;
        private readonly PhysicsController physicsController;
        private readonly DecisionController decisionController;
        public bool IsSpawningAllowed { get; private set; }

        public SimulationController(int horiz, int vert, int vehicles)
        {
            ConsoleLogger.DeleteLogs();
            this.World = new Map(horiz, vert, vehicles);
            this.vehicleGenerator = new VehicleGenerator(this.World);
            this.physicsController = new PhysicsController(this.World);
            this.decisionController = new DecisionController(this.World);
        }

        /// <summary>
        /// Initializes simulation
        /// </summary>
        public void InitSimulation()
        {
            this.World.sw.Start();
            this.vehicleGenerator.VehiclesSpawner(this.World.Vehicles, this.World.DesiredAmountOfVehicles, this.World.SpawnPoints.Count);
        }

        /// <summary>
        /// Single simulation tick
        /// </summary>
        public void PerformSimulationTick()
        {
            this.HandleSpawnPoints();
            this.HandleSpawning();
            this.decisionController.HandleDecisions();
            this.physicsController.HandlePhysics();
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
            if (time < 100 && this.IsSpawningAllowed)
            {
                //ConsoleLogger.Log("Time " + this.World.sw.ElapsedMilliseconds + "ms");
                this.vehicleGenerator.VehiclesSpawner(this.World.Vehicles, this.World.DesiredAmountOfVehicles, this.World.SpawnPoints.Count);
                this.IsSpawningAllowed = false;
            }
            else if (time >= 100 && !this.IsSpawningAllowed)
                this.IsSpawningAllowed = true;
        }

        /// <summary>
        /// Moves vehicle from spawn point to street and add them to vehicles list
        /// </summary>
        private void MoveToStreetFromSpawn(Vehicle vehicle)
        {
            vehicle.Place = ((EndPoint)vehicle.Place).Street;
            vehicle.Place.Vehicles.Add(vehicle);
        }
    }
}

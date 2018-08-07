using Traffic.World;
using Traffic.World.Vertices;
using Traffic.Vehicles;
using Traffic.Utilities;

namespace Traffic.Physics
{
    /// <summary>
    ///     Main class controlling the simulation
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
        ///     Initializes simulation
        /// </summary>
        public void InitSimulation()
        {
            this.World.sw.Start();
            this.vehicleGenerator.VehiclesSpawner(this.World.Vehicles, this.World.DesiredAmountOfVehicles, this.World.SpawnPoints.Count);
        }

        /// <summary>
        ///     Single simulation tick
        /// </summary>
        public void PerformSimulationTick()
        {
            this.HandleSpawnPoints();
            this.HandleSpawning();
            this.decisionController.HandleDecisions();
            this.physicsController.HandlePhysics();
        }

        /// <summary>
        ///     Moves vehicles from spawn point to street if necessary
        /// </summary>
        private void HandleSpawnPoints()
        {
            //TODO: won't work for very large amount of vehicles, either make some constraint on amount of vehicles per streets or handle this properly
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
        ///     Spawns new vehicles if it is necessary
        /// </summary>
        private void HandleSpawning()
        {
            // Mechanism to spawn new vehicles once per Constants.TimeSpawnInterval s
            long time = this.World.sw.ElapsedMilliseconds % Constants.TimeSpawnInterval;
            if (time < 100 && this.IsSpawningAllowed)
            {
                this.vehicleGenerator.VehiclesSpawner(this.World.Vehicles, this.World.DesiredAmountOfVehicles, this.World.SpawnPoints.Count);
                this.IsSpawningAllowed = false;
            }
            else if (time >= 100 && !this.IsSpawningAllowed)
                this.IsSpawningAllowed = true;
        }

        /// <summary>
        ///     Moves vehicle from spawn point to street and add them to vehicles list
        /// </summary>
        private void MoveToStreetFromSpawn(Vehicle vehicle)
        {
            vehicle.Place = ((EndPoint)vehicle.Place).Street;
            vehicle.Place.Vehicles.Add(vehicle);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traffic.Vehicles;
using Traffic.Utilities;
using Traffic.World;
using Traffic.World.Edges;
using Traffic.World.Vertices;
using Traffic.Exceptions;
using System.Threading;

namespace Traffic.Utilities
{
    public class VehicleGenerator
    {

        public Map Map { get; private set; }

        public VehicleGenerator(Map m)
        {
            this.Map = m;
        }

        /// <summary>
        /// Adds randomly generated vehicles to list
        /// </summary>
        /// <param name="Vehicles">List that we add to</param>
        /// <param name="amount">amount of cars to generate</param>
        public void GenerateRandomVehicles(List<Vehicle> vehicles, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                EndPoint spawnPoint = this.GenerateRandomSpawn();
                EndPoint finishPoint = this.GenerateRandomFinish(spawnPoint);
                List<Decision> initialRoute = this.GenerateInitialRoute(spawnPoint, finishPoint);

                //w domyśle tutaj można jeszcze losować typ obiektu jaki będziemy dodawać do listy, np Car/Bicycle/BattlElephant
                vehicles.Add(new Car(RandomGenerator.Velocity(), RandomGenerator.ReactionTime(),
                        RandomGenerator.DistanceHeld(), RandomGenerator.RegistrationNumber(), 
                        spawnPoint, initialRoute));
            }
        }

        /// <summary>
        /// Method to respawn vehicles in order to achieve desired amount of vehicles at any time
        /// </summary>
        public void VehiclesSpawner(List<Vehicle> vehicles, int desiredAmountOfVehicles, int spawnPointsAmount)
        {
            if (vehicles.Count == desiredAmountOfVehicles)
                return;
            int missing = desiredAmountOfVehicles - vehicles.Count;
            if (missing > spawnPointsAmount)
                this.GenerateRandomVehicles(vehicles, spawnPointsAmount);
            else
                this.GenerateRandomVehicles(vehicles, missing);
        }

        /// <summary>
        /// Returns random EndPoint that is not occupied
        /// </summary>
        private EndPoint GenerateRandomSpawn()
        {
            List<EndPoint> notOccupied = this.Map.SpawnPoints.Where(item => item.IsOccupied == false).ToList();
            if (notOccupied.Count == 0)
                throw new NoUnoccupiedSpawnException("There are no unoccupied spawn points");
            var startingPoint = notOccupied[RandomGenerator.Int(notOccupied.Count)];

            ConsoleLogger.Log("Respawn r:" + startingPoint.RowNumber + " c:" + startingPoint.ColumnNumber);

            startingPoint.IsOccupied = true;
            return startingPoint;
        }

        /// <summary>
        /// Returns random finish point diffrent than spawn point
        /// </summary>
        private EndPoint GenerateRandomFinish(EndPoint spawnPoint)
        {
            int randomIndex = RandomGenerator.Int(this.Map.SpawnPoints.Count);
            if (this.Map.SpawnPoints[randomIndex] == spawnPoint)
            {
                if (randomIndex != 0)
                    randomIndex--;
                else
                    randomIndex++;
            }
            return this.Map.SpawnPoints[randomIndex];
        }

        /// <summary>
        /// Generates initial route for spawn point to finish point
        /// </summary>
        /// <param name="spawnPoint"></param>
        /// <param name="finishPoint"></param>
        /// <returns>List of decisions</returns>
        private List<Decision> GenerateInitialRoute(EndPoint spawnPoint, EndPoint finishPoint)
        {
            // TODO, use Constants.Decision
            return new List<Decision>();
        }
    }
}

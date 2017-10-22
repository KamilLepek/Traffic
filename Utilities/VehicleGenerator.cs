using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                //w domyśle tutaj można jeszcze losować typ obiektu jaki będziemy dodawać do listy, np Car/Bicycle/BattlElephant
                vehicles.Add(new Car(RandomGenerator.Velocity() , RandomGenerator.ReactionTime(), 
                        RandomGenerator.DistanceHeld(), RandomGenerator.RegistrationNumber(), this.GenerateRandomSpawn() ));
            }
        }

        /// <summary>
        /// Respawns vehicles asynchrounusly
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
    }
}

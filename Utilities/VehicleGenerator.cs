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

namespace Traffic.Utilities
{
    public class VehicleGenerator
    {

        public Map Map { get; private set; }

        public VehicleGenerator(Map m)
        {
            this.Map = m;
        }

        public List<Car> GenerateRandomCars(int amount)
        {
            var list = new List<Car>();
            for (int i = 0; i < amount; i++)
            {
                list.Add(new Car(RandomGenerator.Velocity() , RandomGenerator.ReactionTime(), 
                        RandomGenerator.DistanceHeld(), RandomGenerator.RegistrationNumber(), this.GenerateRandomSpawnPoint() ));
            }
            return list;
        }

        /// <summary>
        /// W domysle chciałem tu wygenerować losowy spawn point, ale już nie w układzie tym naszym szachowym row/column, tylko normalnie współrzędne
        /// </summary>
        /// <returns>PÓKI CO ZWRACA NA STAŁE 0</returns>
        private Point GenerateRandomSpawnPoint()
        {
            return new Point(0, 0);
            var startingPoint = this.Map.SpawnPoints[RandomGenerator.Int(this.Map.SpawnPoints.Count)];
            switch(startingPoint.Orientation)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    throw new Exception("Co sie stao");
            }
        }
    }
}

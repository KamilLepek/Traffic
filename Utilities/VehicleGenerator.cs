using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.Vehicles;

namespace Traffic.Utilities
{
    public class VehicleGenerator
    {

        private Random gen;

        public VehicleGenerator()
        {
            this.gen = new Random();
        }

        private string generateRandomRegistrationPlate()
        {
            int voivodeship = this.gen.Next(1, 17);
            string registration = string.Empty;
            switch(voivodeship)
            {
                case 1:
                    registration += 'G';
                    break;
                case 2:
                    registration += 'Z';
                    break;
                case 3:
                    registration += 'N';
                    break;
                case 4:
                    registration += 'B';
                    break;
                case 5:
                    registration += 'C';
                    break;
                case 6:
                    registration += 'P';
                    break;
                case 7:
                    registration += 'F';
                    break;
                case 8:
                    registration += 'D';
                    break;
                case 9:
                    registration += 'O';
                    break;
                case 10:
                    registration += 'S';
                    break;
                case 11:
                    registration += 'E';
                    break;
                case 12:
                    registration += 'T';
                    break;
                case 13:
                    registration += 'K';
                    break;
                case 14:
                    registration += 'R';
                    break;
                case 15:
                    registration += 'L';
                    break;
                case 16:
                    registration += 'W';
                    break;
                default:
                    return "Error";
            }
            registration += (char)('A' + this.gen.Next(0, 26));
            registration += (char)('A' + this.gen.Next(0, 26));
            registration += ' ';
            for (int i = 0; i < 3; i++)
            {
                if (this.gen.Next(10) % 2 == 1)
                    registration += (char)(48 + this.gen.Next(0, 10));
                else
                    registration += (char)('A' + this.gen.Next(0, 26));
            }
            return registration;
        }

        public List<Car> generateRandomCars(int amount)
        {
            List<Car> list = new List<Car>();
            for (int i = 0; i < amount; i++)
            {
                list.Add(new Car((float)this.gen.NextDouble() * 200, (float)this.gen.NextDouble() * 500, (float)this.gen.NextDouble() * 50, this.generateRandomRegistrationPlate()));
            }
            return list;
        }
    }
}

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
            int verticalLines = 10;
            int horizontalLines = 7;
            int desiredNumberOfVehicles = 2 * (verticalLines + horizontalLines); //to jest liczba spawnow, dla wiekszej ilosci rysowanie sie wywali(tzn bardziej nie ma sensu),
            //bo nie ma poruszania ani kolizji i będą się stackować na tych ulicach,
            //a jeśli damy powyższą wartość lub mniejszą, to powinno dać się cacy narysować auta po zrespieniu, bo każdy dostanie inny spawn :)

            var controller = new SimulationController(horizontalLines, verticalLines, desiredNumberOfVehicles);
            controller.HandleSimulation();
        }
    }
}

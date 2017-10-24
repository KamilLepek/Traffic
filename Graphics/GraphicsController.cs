using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.World;

namespace Traffic.Graphics
{
    /// <summary>
    /// Class handling graphics of the application
    /// </summary>
    public class GraphicsController
    {

        public Map Map { get; private set; }

        public GraphicsController(Map m)
        {
            this.Map = m;
        }

        // póki co poniższa metoda jest swoistą zaślepką tego temporary printowania, ale docelowo wszystko będziemy tu pisać a klasę ConsolePreview usuniemy
        public void PrintMap()
        {
            ConsolePreview.PrintMap(this.Map);
        }
    }
}

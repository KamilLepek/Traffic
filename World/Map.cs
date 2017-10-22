using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traffic.World.Edges;
using Traffic.World.Vertices;
using Traffic.Utilities;
using Traffic.Vehicles;
using System.Diagnostics;

namespace Traffic.World
{
    public class Map
    {
        public List<Vehicle> Vehicles { get; set; }
        public List<EndPoint> SpawnPoints { get; private set; }
        public List<Street> Streets { get; private set; }
        public List<Intersection> Intersections { get; private set; }

        public double MapWidth { get; private set; }
        public double MapHeight { get; private set; }

        public int DesiredAmountOfVehicles { get; private set; }

        public Stopwatch sw { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="a">rows</param>
        /// <param name="b">collumns</param>
        public Map(int a, int b, int des)
        {
            this.sw = new Stopwatch();
            this.DesiredAmountOfVehicles = des;
            this.Streets = new List<Street>();
            this.SpawnPoints = new List<EndPoint>();
            this.Intersections = new List<Intersection>();
            this.Vehicles = new List<Vehicle>();

            this.MapHeight = (a + 1) * Constants.StreetLength + a * Constants.IntersectionSize;
            this.MapWidth = (b + 1) * Constants.StreetLength + b * Constants.IntersectionSize;
            //liczba ulic będzie a*(b+1)+b*(a+1)
            //skrzyżowań a*b
            //endpointów 2*(a+b)

            //pionowe ulice
            for (int i = 0; i < a + 1; i++) 
            {
                for (int j = 0; j < b; j++)
                {
                    this.Streets.Add(new Street(2 * (i + 1) - 1, 2 * (j + 1), true));
                }
            }

            //poziome ulice
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b + 1; j++)
                {
                    this.Streets.Add(new Street(2 * (i + 1), 2 * (j + 1) - 1, false));
                }
            }

            //dodajemy endopinty
            foreach(var street in Streets)
            {
                bool IsTopOrBottomEnd = (street.ColumnNumber % 2 == 0 && (street.RowNumber == 1 || street.RowNumber == 2 * (a + 1) - 1));
                bool IsLeftOrRightEnd = (street.RowNumber % 2 == 0 && (street.ColumnNumber == 1 || street.ColumnNumber == 2 * (b + 1) - 1));

                if (IsTopOrBottomEnd || IsLeftOrRightEnd) 
                {
                    EndPoint vert = new EndPoint(street);
                    street.Edges.Add(vert);
                    this.SpawnPoints.Add(vert);
                }
            }

            //dodajemy skrzyżowania
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    var intersection = new Intersection(2 * (i + 1), 2 * (j + 1));
                    this.Intersections.Add(intersection);
                    foreach(var street in Streets)
                    {
                        if (Math.Abs(street.ColumnNumber - intersection.ColumnNumber) + Math.Abs(street.RowNumber - intersection.RowNumber) == 1)
                        {
                            intersection.IntersectingStreets.Add(street);
                            street.Edges.Add(intersection);
                        }
                    }
                }
            }

        }
    }
}

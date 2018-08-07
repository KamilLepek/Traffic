using System;
using System.Collections.Generic;
using System.Linq;
using Traffic.Vehicles;
using Traffic.World;
using Traffic.World.Vertices;
using Traffic.Exceptions;

namespace Traffic.Utilities
{
    /// <summary>
    ///     Class which handles vehicles generation
    /// </summary>
    public class VehicleGenerator
    {

        public Map Map { get; }

        public VehicleGenerator(Map m)
        {
            this.Map = m;
        }

        /// <summary>
        ///     Adds randomly generated vehicles to list
        /// </summary>
        /// <param name="vehicles"> List that we add to </param>
        /// <param name="amount"> amount of cars to generate </param>
        public void GenerateRandomVehicles(List<Vehicle> vehicles, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                EndPoint spawnPoint = this.GenerateRandomSpawn();
                EndPoint finishPoint = this.GenerateRandomFinish(spawnPoint);
                List<Decision> initialRoute = this.GenerateInitialRoute(spawnPoint, finishPoint);

                //TODO: if vehicles different than cars will be implemented then we will randomize it here as well
                vehicles.Add(new Car(RandomGenerator.Velocity(), RandomGenerator.ReactionTime(),
                        RandomGenerator.DistanceHeld(), RandomGenerator.RegistrationNumber(),
                        spawnPoint, initialRoute, finishPoint, RandomGenerator.Int(Constants.NumberOfTextures)));
            }
        }

        /// <summary>
        ///     Method to respawn vehicles in order to achieve desired amount of vehicles at any time
        /// </summary>
        public void VehiclesSpawner(List<Vehicle> vehicles, int desiredAmountOfVehicles, int spawnPointsAmount)
        {
            if (vehicles.Count == desiredAmountOfVehicles)
                return;
            int missing = desiredAmountOfVehicles - vehicles.Count;
            this.GenerateRandomVehicles(vehicles, missing > spawnPointsAmount ? spawnPointsAmount : missing);
        }

        /// <summary>
        ///     Returns random EndPoint that is not occupied
        /// </summary>
        private EndPoint GenerateRandomSpawn()
        {
            List<EndPoint> notOccupied = this.Map.SpawnPoints.Where(item => item.IsOccupied == false).ToList();
            if (notOccupied.Count == 0)
                throw new NoUnoccupiedSpawnException("There are no unoccupied spawn points");
            var startingPoint = notOccupied[RandomGenerator.Int(notOccupied.Count)];
            startingPoint.IsOccupied = true;
            return startingPoint;
        }

        /// <summary>
        ///     Returns random finish point diffrent than spawn point
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
        ///     Generates initial route for spawn point to finish point
        /// </summary>
        /// <returns> List of decisions </returns>
        private List<Decision> GenerateInitialRoute(EndPoint spawnPoint, EndPoint finishPoint)
        {
            var list = new List<Decision>();

            // Case in which we won't need to double turn
            if (spawnPoint.Orient != finishPoint.Orient)
            {
                int columnDistance = Math.Abs(spawnPoint.ColumnNumber - finishPoint.ColumnNumber);
                int rowDistance = Math.Abs(spawnPoint.RowNumber - finishPoint.RowNumber);
                if (columnDistance == 0 || rowDistance == 0) // Case in which we just need to drive forward :)
                    this.AddForwardDecisions(columnDistance, rowDistance, list);
                else if (this.AreOpposite(spawnPoint, finishPoint))// Case in which we go forward turn, go forward again and turn
                    this.AddForwardTurnForwardTurn(spawnPoint, finishPoint, list);
                else if (this.ForwardRightXorLeft(spawnPoint, finishPoint) == Orientation.Right) // Case in which we go forward right and forward
                    this.AddForwardRightXorLeftForward(spawnPoint.Orient, columnDistance, rowDistance, list, Decision.Right);
                else if (this.ForwardRightXorLeft(spawnPoint, finishPoint) == Orientation.Left) // Case in which we go forward left and forward
                    this.AddForwardRightXorLeftForward(spawnPoint.Orient, columnDistance, rowDistance, list, Decision.Left);
            }
            // Here we will need to double turn as we start and finnish at the same side of the map
            else
                this.DoubleTurn(spawnPoint, finishPoint, list);
            if (!list.Any())
                throw new Exception("Decision list is empty");
            return list;
        }

        /// <summary>
        ///     Determines whether 2 EndPoints have opposite orientation
        /// </summary>
        /// <returns> True if both endpoints have opposite orientation, flase otherwise </returns>
        private bool AreOpposite(EndPoint spawnPoint, EndPoint finishPoint)
        {
            if (spawnPoint.Orient == Orientation.Top && finishPoint.Orient == Orientation.Bottom ||
                spawnPoint.Orient == Orientation.Bottom && finishPoint.Orient == Orientation.Top ||
                spawnPoint.Orient == Orientation.Left && finishPoint.Orient == Orientation.Right ||
                spawnPoint.Orient == Orientation.Right && finishPoint.Orient == Orientation.Left)
                return true;
            return false;
        }

        /// <summary>
        ///     Add accurate amount of forward decision decided by the distance between points on map
        /// </summary>
        private void AddForwardDecisions(int columnDistance, int rowDistance, List<Decision> list)
        {
            int nonZeroDimension = columnDistance == 0 ? rowDistance : columnDistance;
            for (int i = 0; i < (nonZeroDimension - 2) / 2; i++)
                list.Add(Decision.Forward);
        }

        /// <summary>
        ///     For scenarios where we go forward then turn and go forward.
        ///     Determines wheter we need to turn right or left
        /// </summary>
        /// <returns> Right or Left decision </returns>
        private Orientation ForwardRightXorLeft(EndPoint spawnPoint, EndPoint finishPoint)
        {
            switch (spawnPoint.Orient)
            {
                case Orientation.Top:
                    return finishPoint.Orient == Orientation.Left ? Orientation.Right : Orientation.Left;
                case Orientation.Bottom:
                    return finishPoint.Orient == Orientation.Right ? Orientation.Right : Orientation.Left;
                case Orientation.Left:
                    return finishPoint.Orient == Orientation.Bottom ? Orientation.Right : Orientation.Left;
                case Orientation.Right:
                    return finishPoint.Orient == Orientation.Top ? Orientation.Right : Orientation.Left;
            }
            throw new Exception("Failed to determine orientation");
        }

        /// <summary>
        ///     Adds list of decisions in case we need to move forward, then turn and move forward again
        /// </summary>
        /// <param name="or"> orientation of spawn point </param>
        /// <param name="columnDistance"> distance in columns between spawn and finish point </param>
        /// <param name="rowDistance"> distance in rows between spawn and finish point </param>
        /// <param name="list"> list to add to </param>
        /// <param name="rightOrLeft"> determines if the turn is right or left </param>
        private void AddForwardRightXorLeftForward(Orientation or, int columnDistance, int rowDistance, List<Decision> list, Decision rightOrLeft)
        {
            switch (or)
            {
                case Orientation.Top:
                case Orientation.Bottom:
                    for (int i = 0; i < (rowDistance - 2) / 2; i++)
                        list.Add(Decision.Forward);
                    list.Add(rightOrLeft);
                    for (int i = 0; i < (columnDistance - 2) / 2; i++)
                        list.Add(Decision.Forward);
                    break;
                case Orientation.Left:
                case Orientation.Right:
                    for (int i = 0; i < (columnDistance - 2) / 2; i++)
                        list.Add(Decision.Forward);
                    list.Add(rightOrLeft);
                    for (int i = 0; i < (rowDistance - 2) / 2; i++)
                        list.Add(Decision.Forward);
                    break;
            }
        }

        /// <summary>
        ///     Adds list of decisions for case when we go forward turn and go forward again and turn at the end 
        /// </summary>
        private void AddForwardTurnForwardTurn(EndPoint spawnPoint, EndPoint finishPoint, List<Decision> list)
        {
            int columnDistance = Math.Abs(spawnPoint.ColumnNumber - finishPoint.ColumnNumber);
            int rowDistance = Math.Abs(spawnPoint.RowNumber - finishPoint.RowNumber);
            switch (spawnPoint.Orient)
            {
                case Orientation.Bottom:
                    for (int i = 0; i < (rowDistance - 4) / 2; i++)
                        list.Add(Decision.Forward);
                    if (finishPoint.ColumnNumber - spawnPoint.ColumnNumber > 0)
                    {
                        list.Add(Decision.Right);
                        this.AddForwardRightXorLeftForward(Orientation.Left, columnDistance, 2, list, Decision.Left);
                    }
                    else
                    {
                        list.Add(Decision.Left);
                        this.AddForwardRightXorLeftForward(Orientation.Right, columnDistance, 2, list, Decision.Right);
                    }
                    break;
                case Orientation.Left:
                    for (int i = 0; i < (columnDistance - 4) / 2; i++)
                        list.Add(Decision.Forward);
                    if (finishPoint.RowNumber > spawnPoint.RowNumber)
                    {
                        list.Add(Decision.Right);
                        this.AddForwardRightXorLeftForward(Orientation.Top, 2, rowDistance, list, Decision.Left);
                    }
                    else
                    {
                        list.Add(Decision.Left);
                        this.AddForwardRightXorLeftForward(Orientation.Bottom, 2, rowDistance, list, Decision.Right);
                    }
                    break;
                case Orientation.Right:
                    for (int i = 0; i < (columnDistance - 4) / 2; i++)
                        list.Add(Decision.Forward);
                    if (finishPoint.RowNumber > spawnPoint.RowNumber)
                    {
                        list.Add(Decision.Left);
                        this.AddForwardRightXorLeftForward(Orientation.Top, 2, rowDistance, list, Decision.Right);
                    }
                    else
                    {
                        list.Add(Decision.Right);
                        this.AddForwardRightXorLeftForward(Orientation.Bottom, 2, rowDistance, list, Decision.Left);
                    }
                    break;
                case Orientation.Top:
                    for (int i = 0; i < (rowDistance - 4) / 2; i++)
                        list.Add(Decision.Forward);
                    if (finishPoint.ColumnNumber - spawnPoint.ColumnNumber > 0)
                    {
                        list.Add(Decision.Left);
                        this.AddForwardRightXorLeftForward(Orientation.Left, columnDistance, 2, list, Decision.Right);
                    }
                    else
                    {
                        list.Add(Decision.Right);
                        this.AddForwardRightXorLeftForward(Orientation.Right, columnDistance, 2, list, Decision.Left);
                    }
                    break;
            }
        }

        /// <summary>
        ///     Adds list of decisions for case when we start at the same side of the map as we finnish
        /// </summary>
        private void DoubleTurn(EndPoint spawnPoint, EndPoint finishPoint, List<Decision> list)
        {
            int columnDistance = Math.Abs(spawnPoint.ColumnNumber - finishPoint.ColumnNumber);
            int rowDistance = Math.Abs(spawnPoint.RowNumber - finishPoint.RowNumber);
            switch (spawnPoint.Orient)
            {
                case Orientation.Bottom:
                    if (finishPoint.ColumnNumber - spawnPoint.ColumnNumber > 0)
                    {
                        list.Add(Decision.Right);
                        this.AddForwardRightXorLeftForward(Orientation.Left, columnDistance, 2, list, Decision.Right);
                    }
                    else
                    {
                        list.Add(Decision.Left);
                        this.AddForwardRightXorLeftForward(Orientation.Right, columnDistance, 2, list, Decision.Left);
                    }
                    break;
                case Orientation.Left:
                    if (finishPoint.RowNumber - spawnPoint.RowNumber > 0)
                    {
                        list.Add(Decision.Right);
                        this.AddForwardRightXorLeftForward(Orientation.Top, 2, rowDistance, list, Decision.Right);
                    }
                    else
                    {
                        list.Add(Decision.Left);
                        this.AddForwardRightXorLeftForward(Orientation.Bottom, 2, rowDistance, list, Decision.Left);
                    }
                    break;
                case Orientation.Right:
                    if (finishPoint.RowNumber - spawnPoint.RowNumber > 0)
                    {
                        list.Add(Decision.Left);
                        this.AddForwardRightXorLeftForward(Orientation.Top, 2, rowDistance, list, Decision.Left);
                    }
                    else
                    {
                        list.Add(Decision.Right);
                        this.AddForwardRightXorLeftForward(Orientation.Bottom, 2, rowDistance, list, Decision.Right);
                    }
                    break;
                case Orientation.Top:
                    if (finishPoint.ColumnNumber - spawnPoint.ColumnNumber > 0)
                    {
                        list.Add(Decision.Left);
                        this.AddForwardRightXorLeftForward(Orientation.Left, columnDistance, 2, list, Decision.Left);
                    }
                    else
                    {
                        list.Add(Decision.Right);
                        this.AddForwardRightXorLeftForward(Orientation.Left, columnDistance, 2, list, Decision.Right);
                    }
                    break;
            }
        }
    }
}

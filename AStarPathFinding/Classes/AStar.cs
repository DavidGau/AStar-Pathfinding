using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarPathFinding.Classes
{
    /// <summary>
    /// This class is the heart of the Algorithm.
    /// 
    /// It's been created based on the following video:
    /// https://www.youtube.com/watch?v=-L-WgKMFuhE
    /// </summary>
    public class AStar
    {

        private MovementCost _movementCost = new MovementCost();

        private enum EnumPositionValues
        {
            Nothing = 0,
            Wall = 1
        }

        private uint[][] _arr2DMap { get; set; }
        private (uint,uint) _tplStartingPosition { get; set; }
        private (uint,uint) _tplEndingPosition { get; set; }


        /// <summary>
        /// This constructor takes a 2D map and its position to initialize the algorithm.
        /// </summary>
        /// <param name="arr2DMap">
        /// The array that corresponds to the map. All the values contained in that map must be either:
        /// - 0 (Contains nothing at this position)
        /// - 1 (Contains a wall at this position)
        /// </param>
        /// <param name="tplStartingNode">
        /// A tuple that contains the row, col of the starting node in the given 2D map
        /// </param>
        /// <param name="tplEndingNode">
        /// A tuple that contains the row, col of the ending node in the given 2D map
        /// </param>
        public AStar(uint[][] arr2DMap, (uint, uint) tplStartingNode, (uint, uint) tplEndingNode)
        {

            //Check if the map is valid
            if(!this.CheckIfMapIsValid(arr2DMap))
            {
                throw new InvalidDataException("The given map is invalid. Make sure it contains at least one row, one column and always has the same number of columns.");
            }

            //Check if the starting node is invalid
            if(!this.CheckPositionInMap(arr2DMap, tplStartingNode))
            {
                throw new IndexOutOfRangeException("The starting position is not contained within the map.");
            }

            //Check if the ending node is invalid
            if(!this.CheckPositionInMap(arr2DMap, tplEndingNode))
            {
                throw new IndexOutOfRangeException("The ending position is not contained within the map.");
            }

            this._arr2DMap = arr2DMap;
            this._tplStartingPosition = tplStartingNode;
            this._tplEndingPosition = tplEndingNode;
        }


        /// <summary>
        /// Takes a map and checks whether its valid or not.
        /// </summary>
        /// <param name="arrMap">
        /// The map to check
        /// </param>
        /// <returns>
        /// A boolean indicating whether the position is a valid one or not.
        /// </returns>
        private bool CheckIfMapIsValid(uint[][] arrMap)
        {
            //If the map doesn't contain at least one row
            if(arrMap.Length <= 0)
            {
                return false;
            }

            int numberOfColsFirstRow = arrMap[0].Length;

            //If the map does not contain at least one column
            if(numberOfColsFirstRow <= 0)
            {
                return false;
            }

            //If the rows don't always contain the same number of columns
            if(arrMap.Any(x => x.Length != numberOfColsFirstRow))
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// Takes a map and a position and checks whether the position is contained 
        /// in the map or not.
        /// </summary>
        /// <param name="arrMap">
        /// The map
        /// </param>
        /// <param name="tplPosition">
        /// The position to check
        /// </param>
        /// <returns>
        /// A boolean indicating whether the position is a valid one or not.
        /// </returns>
        private bool CheckPositionInMap(uint[][] arrMap, (uint,uint) tplPosition)
        {
            //If the position's row is greater than the actual number of rows the map has
            if (tplPosition.Item1 > arrMap.Length)
            {
                return false;
            }

            //If the position's column is greater than the actual number of columns the map has
            if(tplPosition.Item2 > arrMap[0].Length)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// This structure is used to contain the costs of the two possible
    /// types of movements that the algorithm can make.
    /// </summary>
    public struct MovementCost
    {
        public int directMovement = 10;
        public int diagonalMovement = 14;

        public MovementCost()
        {

        }
    }
}

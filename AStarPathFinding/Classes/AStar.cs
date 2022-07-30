using System;
using System.Collections.Generic;
using System.Drawing;
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
        private List<Node> _lstVisitedNodes { get; set; }
        private List<Node> _lstPotentialNodes { get; set; }

        private (int, int) _tplMapDimensions { get; set; }
        private Node? _pathFound { get; set; }


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
            this._tplMapDimensions = (arr2DMap.Length, arr2DMap[0].Length);
            this._tplStartingPosition = tplStartingNode;
            this._tplEndingPosition = tplEndingNode;
            this._lstVisitedNodes = new List<Node>();
            this._lstPotentialNodes = new List<Node>();
            this._pathFound = null;
        }


        /// <summary>
        /// Finds the shortest path possible from the starting node
        /// to do ending node.
        /// 
        /// Note: Takes into account that there IS a solution.
        /// </summary>
        /// <returns>
        /// Returns the last node, from which we can recreate the entire
        /// path using the "previousNode" property.
        /// </returns>
        public Node FindPath()
        {
            //Create the starting node
            Node startingNode = new Node(0, 0, 0, this._tplStartingPosition.Item1, this._tplStartingPosition.Item2, null);

            //Initialize the list of potential nodes
            this._lstPotentialNodes = this.GetSurroundingNodes(startingNode);

            //Initialize the current Node the algorithm is on
            Node currentNode = startingNode;

            bool hasSolution = false;

            //While the solution has not been found
            while(!hasSolution)
            {
                //Find the next node to move on
                Node nextNode = this.GetNextNode(currentNode);

                //If the nextNode corresponds to the end node
                if(nextNode.Row == this._tplEndingPosition.Item1 && nextNode.Col == this._tplEndingPosition.Item2)
                {
                    hasSolution = true;
                }

                //Set the current node to the next node
                currentNode = nextNode;
            }

            //Set the found path property
            this._pathFound = currentNode;

            //Return the solution
            return currentNode;
        }


        /// <summary>
        /// This method takes the current node and 
        /// returns the next node we should select
        /// </summary>
        /// <param name="currentNode">
        /// The current node we're on
        /// </param>
        /// <returns>
        /// The next node to move to
        /// </returns>
        private Node GetNextNode(Node currentNode)
        {
            Node nextNode;

            //Find the smallest F-COST of all the potential nodes
            int smallestFCost = this._lstPotentialNodes.Select(x => x.FCost).Min();

            //Find all the nodes that have this F-Cost
            List<Node> lstNodesSmallestFCost = this._lstPotentialNodes.Where(x => x.FCost == smallestFCost).ToList();

            //If there is more than a single node with the same F-Cost
            if(lstNodesSmallestFCost.Count > 1)
            {
                //Find the smallest H-Cost
                int smallestHCost = lstNodesSmallestFCost.Select(x => x.HCost).Min();

                //Find the first node that has this H-Cost
                //Takes the first without any other consideration because, there are no "better choice" (these nodes would have all the same costs)
                nextNode = lstNodesSmallestFCost.First(x => x.HCost == smallestHCost);
            }
            else
            {
                nextNode = lstNodesSmallestFCost[0];
            }

            //Adds the node to the list of visited nodes
            this._lstVisitedNodes.Add(nextNode);

            //Check for surrounding nodes and adds them to the potential nodes array
            this._lstPotentialNodes.AddRange(this.GetSurroundingNodes(nextNode));
            this._lstPotentialNodes = this._lstPotentialNodes.Where(x => x.Row != nextNode.Row || x.Col != nextNode.Col).ToList();

            currentNode.NextNode = nextNode;
            return currentNode.NextNode;
        }

        /// <summary>
        /// Takes a node and returns the list of all the nodes
        /// that surrounds it. (Logically, there won't be more than 8 nodes)
        /// </summary>
        /// <param name="currentNode">
        /// The node we want to know the surrounding of
        /// </param>
        /// <returns>
        /// The list of nodes that surround the current node
        /// </returns>
        private List<Node> GetSurroundingNodes(Node currentNode)
        {
            List<Node> lstSurroundingNodes = new List<Node>();

            //Checks up,middle,down
            for(int i = -1;i < 2;i++)
            {
                //Checks left,middle,right
                for(int j = -1;j < 2;j++)
                {
                    //If we're on the current node...
                    if(i == 0 && j == 0)
                    {
                        continue;
                    }

                    uint rowToCheck = (uint)(currentNode.Row + i);
                    uint colToCheck = (uint)(currentNode.Col + j);

                    //If we're out of bound
                    if(rowToCheck < 0 || colToCheck < 0 || rowToCheck >= this._tplMapDimensions.Item1 || colToCheck >= this._tplMapDimensions.Item2)
                    {
                        continue;
                    }

                    //If we're on an obstacle
                    if (this._arr2DMap[rowToCheck][colToCheck] == 1)
                    {
                        continue;
                    }

                    //If we're looking at a node that we already visited
                    bool hasAlreadyBeenVisited = this._lstVisitedNodes.Any(x => x.Row == rowToCheck && x.Col == colToCheck); //TODO: Vérifier s'il est logique de faire cette vérification

                    if(hasAlreadyBeenVisited)
                    {
                        continue;
                    }

                    //Add the node to the list of surrounding nodes
                    lstSurroundingNodes.Add(this.CreateNewNode(rowToCheck, colToCheck, currentNode));
                }
            }

            return lstSurroundingNodes;
        }

        /// <summary>
        /// Takes the row,col of the new Node as well
        /// as its previousNode and creates the new Node.
        /// </summary>
        /// <param name="row">
        /// The row of the new Node
        /// </param>
        /// <param name="col">
        /// The col of the new Node
        /// </param>
        /// <param name="previousNode">
        /// The previous Node (the one that lead to this one)
        /// </param>
        /// <returns>
        /// The new node
        /// </returns>
        private Node CreateNewNode(uint row, uint col, Node previousNode)
        {
            int gCost = this.GetGCost(row, col, previousNode);
            int hCost = this.GetHCost(row, col);
            int fCost = gCost + hCost;

            return new Node(hCost, gCost, fCost, row, col, previousNode);
        }

        /// <summary>
        /// Takes the previous node as well as the position of the current
        /// node we want to calculate the G-Cost for.
        /// 
        /// The reason why we need the previous G-Cost is because the cost is relative
        /// to the path it took from the start. A same node has multiple costs depending on the path
        /// we took to reach it.
        /// 
        /// The G-Cost is the distance between the node and the starting node, relative to the path
        /// we took to get to that node.
        /// </summary>
        /// <param name="rowToGo">The row of the new node we want to calculate the G-Cost for</param>
        /// <param name="colToGo">The column of the new node we want to calculate the G-Cost for</param>
        /// <param name="previousNode">The node we took to get here</param>
        /// <returns>
        /// An int corresponding to the G-Cost of the new node
        /// </returns>
        private int GetGCost(uint rowToGo, uint colToGo, Node previousNode)
        {
            int totalGCost = previousNode.GCost;

            //If we move diagonally to get to that node
            if(previousNode.Row != rowToGo && previousNode.Col != colToGo)
            {
                totalGCost += this._movementCost.diagonalMovement;
            }
            else //If we move directly to get to that node
            {
                totalGCost += this._movementCost.directMovement;
            }

            return totalGCost;
        }


        /// <summary>
        /// Takes a new node position and calculates the distance
        /// between this node and the ending node.
        /// 
        /// The H-COst is the distance between the node and the ending node. It does not take into
        /// account any obstacles, its really just the most direct distance between the current node and the ending node.
        /// </summary>
        /// <param name="rowToGo">The node's row</param>
        /// <param name="colToGo">The node's col</param>
        /// <returns>
        /// An int corresponding to the H-Cost of the new node
        /// </returns>
        private int GetHCost(uint rowToGo, uint colToGo)
        {
            //Calculates the difference in rows and cols between the current node and the ending node
            int rowDistanceBetweenCurrentAndEnding = Math.Abs((int)(rowToGo - this._tplEndingPosition.Item1));
            int colDistanceBetweenCurrentAndEnding = Math.Abs((int)(colToGo - this._tplEndingPosition.Item2));

            //Calculates the number of diagonal steps we can make
            int nbInDiagonal = Math.Min(rowDistanceBetweenCurrentAndEnding, colDistanceBetweenCurrentAndEnding);
            rowDistanceBetweenCurrentAndEnding -= nbInDiagonal;
            colDistanceBetweenCurrentAndEnding -= nbInDiagonal;

            //Takes the first one that is not 0 (one of them surely is 0 after the previous step)
            int nbInDirect = Math.Max(rowDistanceBetweenCurrentAndEnding, colDistanceBetweenCurrentAndEnding);

            //Calculate the final cost
            int finalCost = (nbInDiagonal * this._movementCost.diagonalMovement) + (nbInDirect * this._movementCost.directMovement);

            return finalCost;

        }

        /// <summary>
        /// Exports the map as a Bitmap
        /// </summary>
        /// <param name="scale">
        /// This optional parameter allows us to scale the map to a certain level. Usefull when the maps are small and we want
        /// to make them readable to the human eye.
        /// Must be an integer greater or equal to one
        /// </param>
        /// <param name="showPathFound">
        /// Boolean that indicates whether we want to show the path that has been found by the algorithm.
        /// </param>
        /// <returns>
        /// The Bitmap that corresponds to the map
        /// </returns>
        public Bitmap ExportMapAsBitmap(int scale = 1, bool showPathFound=false)
        {
            //Checks if the scaling is valid
            if(scale < 1)
            {
                throw new InvalidDataException("The scale must be greater or equal to one.");
            }

            //If the user wants to show the path found but there is none
            if(showPathFound && this._pathFound == null)
            {
                throw new InvalidOperationException("The path has not yet been solved. Please call the FindPath method.");
            }

            Bitmap bmp = new Bitmap(this._tplMapDimensions.Item2 * scale, this._tplMapDimensions.Item1 * scale);

            //Get all the path positions
            List<(uint, uint)>? lstPathPosition = new List<(uint, uint)>();
            Node currentIndexNode = this._pathFound;
            while(currentIndexNode != null)
            {
                lstPathPosition.Add((currentIndexNode.Row, currentIndexNode.Col));
                currentIndexNode = currentIndexNode.PreviousNode;
            }

            //For each row
            for (int i = 0;i < this._arr2DMap.Length;i++)
            {
                //For each col
                for (int j = 0; j < this._arr2DMap[i].Length;j++)
                {
                    Color color;

                    //If it's the starting position
                    if (i == this._tplStartingPosition.Item1 && j == this._tplStartingPosition.Item2)
                    {
                        color = Color.Red;
                    }
                    else if (i == this._tplEndingPosition.Item1 && j == this._tplEndingPosition.Item2) //If it's the ending position

                    {
                        color = Color.Green;
                    }
                    else if (this._arr2DMap[i][j] == (int)EnumPositionValues.Nothing)      //If it's nothing
                    {
                        //if it's part of the path found
                        if(showPathFound && lstPathPosition.Where(x => x.Item1 == i && x.Item2 == j).Count() > 0)
                        {
                            color = Color.Blue;
                        }
                        else //If it's not part of the path
                        {
                            color = Color.White;
                        }

                    }
                    else
                    {
                        color = Color.Black;
                    }

                    //Adds the color to the right pixels
                    int pixelRowWithScaling = i * scale;
                    int pixelColWithScaling = j * scale;

                    for(int k = 0;k < scale;k++)
                    {
                        for(int l = 0;l < scale;l++)
                        {
                            bmp.SetPixel(pixelColWithScaling + l,pixelRowWithScaling + k, color);
                        }
                    }
                }
            }

            return bmp;
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

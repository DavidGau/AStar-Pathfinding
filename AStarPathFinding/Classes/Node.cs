using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AStarPathFinding.Classes
{
    /// <summary>
    /// This class is used to represent a Node. (Essentially, it's a potential movement the algorithm could choose to do.)
    /// The choice will depend on the different cost values that the node contains (H-Cost, G-Cost, F-Cost).
    /// </summary>
    public class Node
    {
        public int HCost  { get; set; }
        public int GCost { get; set; }
        public int FCost { get; set; }
        public uint Row { get; set; }
        public uint Col { get; set; }
        public Node PreviousNode { get; set; }
        public Node? NextNode { get; set; }

        /// <summary>
        /// Node's constructor
        /// </summary>
        /// <param name="hCost">
        /// Distance from the ending node
        /// </param>
        /// <param name="gCost">
        /// Distance from the starting node
        /// </param>
        /// <param name="fCost">
        /// hCost + gCost
        /// </param>
        /// <param name="row">
        /// Row of the node in the map
        /// </param>
        /// <param name="col">
        /// Col of the node in the map
        /// </param>
        /// <param name="previousNode">
        /// The node that leaded to this one
        /// </param>
        public Node(int hCost, int gCost, int fCost, uint row, uint col, Node? previousNode)
        {
            this.HCost = hCost;
            this.GCost = gCost;
            this.FCost = fCost;
            this.Row = row;
            this.Col = col;
            this.PreviousNode = previousNode;
            this.NextNode = null;
        }
    }
}

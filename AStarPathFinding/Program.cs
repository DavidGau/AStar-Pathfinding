using AStarPathFinding.Classes;

uint[][] arrMap = new uint[][]
{
    new uint[]{1,1,1,1,1,1,1,1},
    new uint[]{1,0,0,0,0,1,1,1},
    new uint[]{1,1,0,1,0,1,1,1},
    new uint[]{1,1,0,1,0,1,1,1},
    new uint[]{1,1,1,1,0,0,0,1},
    new uint[]{1,1,1,1,1,0,0,1}
};

(uint, uint) tplStartingNode = (1, 1);
(uint, uint) tplEndingNode = (5, 7);

AStar AStar = new AStar(arrMap, tplStartingNode, tplEndingNode);
Console.WriteLine("=== End of the program ===");
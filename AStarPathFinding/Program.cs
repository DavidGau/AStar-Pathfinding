using AStarPathFinding.Classes;
using System.Drawing;

uint[][] arrMap = new uint[][]
{
    new uint[]{1,1,1,1,1,1,1,1},
    new uint[]{1,0,0,0,0,1,1,1},
    new uint[]{1,1,0,1,0,1,1,1},
    new uint[]{1,1,0,1,0,1,1,1},
    new uint[]{1,1,1,1,0,0,0,1},
    new uint[]{1,1,1,1,1,0,0,1},
    new uint[]{1,0,0,0,0,0,0,1},
    new uint[]{1,0,1,0,1,0,0,1},
    new uint[]{1,0,1,0,1,1,1,1},
    new uint[]{0,0,0,0,1,1,1,1},
    new uint[]{0,1,1,0,1,1,1,1},
    new uint[]{0,1,1,1,1,1,1,1},
    new uint[]{0,0,0,0,0,0,0,1},
};

(uint, uint) tplStartingNode = (1, 1);
(uint, uint) tplEndingNode = (12, 6);

AStar AStar = new AStar(arrMap, tplStartingNode, tplEndingNode);
Bitmap bmp = AStar.ExportMapAsBitmap(100, showPathFound:false);
bmp.Save("beforeSolved.png");
Node path = AStar.FindPath();
Bitmap bmp1 = AStar.ExportMapAsBitmap(100,showPathFound: true, borderRatio:0.02);
bmp1.Save("afterSolved.png");

Console.WriteLine("=== End of the program ===");
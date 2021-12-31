using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World // Holds World block data
{
    public const int SurfaceY = 12;
    public const int WorldWidth = 1024; // 12 // 1024
    public const int WorldHeight = 32; // 8
    public static Block[,] BlockData = new Block[WorldWidth, WorldHeight];
}

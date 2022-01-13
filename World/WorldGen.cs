using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour // Shapes the World
{
    public bool IsFlat;
    public bool IsRandom;
    public bool IsStoneBlock;
    public bool GenerateSmallTrees;
    public bool GenerateProceduralTrees;
    private void Awake()
    {
        Clear();
        if (IsFlat) GenerateFlat(1, 2);
        if (IsRandom) GenerateRandom();
        if (IsStoneBlock) GenerateStoneBlock();
        InstantiateWorld();
        if (GenerateSmallTrees) TreeGen.CreateTrees(3, 6);
        if (GenerateProceduralTrees) TreeGen.CreateProceduralTrees(5, 8, 0, 3, 2, 5, 5);
        LightingEngine.UpdateWorldLighting();
    }
    private void Clear()
    {
        for (int x = 0; x <= World.BlockData.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= World.BlockData.GetUpperBound(1); y++)
            {
                World.BlockData[x, y] = Blocks.Available[Blocks.BlockID.Air]();
                World.BlockData[x, y].Position = new Point(x, y);
            }
        }
    }
    private void GenerateFlat(int grassHeight, int dirtHeight)
    {
        for (int x = 0; x <= World.BlockData.GetUpperBound(0); x++)
        {
            for (int grassY = World.SurfaceY; grassY > World.SurfaceY - grassHeight; grassY--)
            {
                World.BlockData[x, grassY] = Blocks.Available[Blocks.BlockID.Grass]();
                World.BlockData[x, grassY].Position = new Point(x, grassY);
            }
            for (int dirtY = World.SurfaceY - grassHeight; dirtY > World.SurfaceY - grassHeight - dirtHeight; dirtY--)
            {
                World.BlockData[x, dirtY] = Blocks.Available[Blocks.BlockID.Dirt]();
                World.BlockData[x, dirtY].Position = new Point(x, dirtY);
            }
            for (int stoneY = World.SurfaceY - grassHeight - dirtHeight; stoneY >= 0; stoneY--)
            {
                World.BlockData[x, stoneY] = Blocks.Available[Blocks.BlockID.Stone]();
                World.BlockData[x, stoneY].Position = new Point(x, stoneY);
            }
        }
    }
    private void GenerateRandom()
    {
        for (int x = 0; x <= World.BlockData.GetUpperBound(0); x++)
        {
            int surfaceY = Random.Range(World.SurfaceY, World.SurfaceY + 3);
            int grassHeight = 1;
            int dirtHeight = Random.Range(2, 5);
            for (int grassY = surfaceY; grassY > surfaceY - grassHeight; grassY--)
            {
                World.BlockData[x, grassY] = Blocks.Available[Blocks.BlockID.Grass]();
                World.BlockData[x, grassY].Position = new Point(x, grassY);
            }
            for (int dirtY = surfaceY - grassHeight; dirtY > surfaceY - grassHeight - dirtHeight; dirtY--)
            {
                World.BlockData[x, dirtY] = Blocks.Available[Blocks.BlockID.Dirt]();
                World.BlockData[x, dirtY].Position = new Point(x, dirtY);
            }
            for (int stoneY = surfaceY - grassHeight - dirtHeight; stoneY >= 0; stoneY--)
            {
                World.BlockData[x, stoneY] = Blocks.Available[Blocks.BlockID.Stone]();
                World.BlockData[x, stoneY].Position = new Point(x, stoneY);
            }
        }
    }
    private void GenerateStoneBlock()
    {
        for (int x = 0; x <= World.BlockData.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= World.BlockData.GetUpperBound(1); y++)
            {
                World.BlockData[x, y] = Blocks.Available[Blocks.BlockID.Stone]();
                World.BlockData[x, y].Position = new Point(x, y);
            }
        }
    }
    private void InstantiateWorld()
    {
        foreach (Block b in World.BlockData)
        {
            if (b.ID != Blocks.BlockID.Air)
            {
                BlockManager.InstantiateBlock(b);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGen : MonoBehaviour // Generates trees
{
    public static void CreateProceduralTrees(int trunkLengthMin, int trunkLengthMax, int subTrunkLengthMin, int subTrunkLengthMax, int spaceToTheGround, int leavesWidth, int leavesHeight)
    {
        ValidateProceduralTreeArguments(ref leavesWidth, ref leavesHeight, ref trunkLengthMin, ref spaceToTheGround);
        for (int posX = 0; posX < World.BlockData.GetUpperBound(0); posX++)
        {
            if (Random.Range(0, 12) == 1) // 8.3%
            {
                CreateProceduralTree(posX, trunkLengthMin, trunkLengthMax, subTrunkLengthMin, subTrunkLengthMax, spaceToTheGround, leavesWidth, leavesHeight);
                posX += subTrunkLengthMax + Mathf.FloorToInt(leavesWidth / 2f);
            }
        }

    }
    private static void ValidateProceduralTreeArguments(ref int leavesWidth, ref int leavesHeight, ref int trunkLengthMin, ref int spaceToTheGround)
    {
        if (leavesWidth % 2 == 0) leavesWidth += 1; // Leaves's width and height must be uneven
        if (leavesHeight % 2 == 0) leavesHeight += 1;
        if (trunkLengthMin < spaceToTheGround + Mathf.FloorToInt(leavesHeight / 2f) + 2) // Trunk must be higher than spaceToTheGround
            trunkLengthMin = spaceToTheGround + Mathf.FloorToInt(leavesHeight / 2f) + 2;
    }
    public static void CreateProceduralTree(int x, int trunkLengthMin, int trunkLengthMax, int subTrunkLengthMin, int subTrunkLengthMax, int spaceToTheGround, int leavesWidth, int leavesHeight)
    {
        ValidateProceduralTreeArguments(ref leavesWidth, ref leavesHeight, ref trunkLengthMin, ref spaceToTheGround); // ValidateArguments

        List<OakLog> trunk = new List<OakLog>(); // Create trunk container

        Block startingBlock = WorldManager.FindSurfaceBlock(x); // Block to build tree from

        for (int logY = 0; logY < Random.Range(trunkLengthMin, trunkLengthMax + 1); logY++) // Create trunk
        {
            OakLog log = (OakLog)Blocks.Available[Blocks.BlockID.OakLog]();
            log.Position = new Point(startingBlock.Position.X, startingBlock.Position.Y + logY);
            trunk.Add(log);
        }

        int subTrunksPosMin = spaceToTheGround + Mathf.FloorToInt(leavesHeight / 2f); // Defines minimum trunk pos
        Block trunksOrigin = trunk[Random.Range(subTrunksPosMin, trunk.Count - 1)]; // Where subtrunks grow from

        for (int logX = 0; logX < Random.Range(subTrunkLengthMin, subTrunkLengthMax + 1); logX++) // Create right subtrunks
        {
            OakLog log = (OakLog)Blocks.Available[Blocks.BlockID.OakLog]();
            log.Position = new Point(startingBlock.Position.X + 1 + logX, trunksOrigin.Position.Y);
            trunk.Add(log);
        }

        for (int logX = 0; logX < Random.Range(subTrunkLengthMin, subTrunkLengthMax + 1); logX++) // Create left subtrunks
        {
            OakLog log = (OakLog)Blocks.Available[Blocks.BlockID.OakLog]();
            log.Position = new Point(startingBlock.Position.X - 1 - logX, trunksOrigin.Position.Y);
            trunk.Add(log);
        }
        foreach (OakLog b in trunk) // Place trunk in the world
        {
            if (b.Position.X >= 0 && b.Position.Y < World.WorldHeight && b.Position.X < World.WorldWidth && b.Position.Y >= 0) // Check out of bounds
            {
                WorldManager.Place(b);
            }
        }
        foreach (OakLog b in trunk) // Create leaves
        {
            if (b.Position.Y > startingBlock.Position.Y + spaceToTheGround + 1) // Check if log is too low
            {
                List<OakLeaf> leaves = new List<OakLeaf>(); // Create leaves container
                for (int w = 0; w < leavesWidth; w++) // Leaves block
                {
                    for (int h = 0; h < leavesHeight; h++)
                    {
                        if (!(w == 0 && h == 0) && !(w == 0 && h == leavesHeight - 1) 
                            && !(w == leavesWidth - 1 && h == 0) && !(w == leavesWidth - 1 && h == leavesHeight - 1)) // Cut corners
                        {
                            OakLeaf leaf = (OakLeaf)Blocks.Available[Blocks.BlockID.OakLeaf]();
                            leaf.Position = new Point(b.Position.X - Mathf.FloorToInt(leavesWidth / 2f) + w, b.Position.Y - Mathf.FloorToInt(leavesHeight / 2f) + h);
                            leaves.Add(leaf);
                        }
                    }
                }
                foreach (OakLeaf leaf in leaves) // Place leaves in world
                {
                    if (leaf.Position.X >= 0 && leaf.Position.Y < World.WorldHeight && leaf.Position.X < World.WorldWidth && leaf.Position.Y >= 0) // Check out of bounds
                    {
                        if (World.BlockData[leaf.Position.X, leaf.Position.Y].ID != Blocks.Available[Blocks.BlockID.OakLog]().ID)
                        {
                            WorldManager.Place(leaf);
                        }
                    }
                }
            }
        }
    }
    public static void CreateTrees(int minHeight, int maxHeight) // Creates small trees
    {
        for (int x = 0; x < World.BlockData.GetUpperBound(0); x++)
        {
            if (Random.Range(0, 5) == 0) // 20%
            {
                CreateSmallTree(x, minHeight, maxHeight);
                x += 2;
            }
        }
    }
    public static void CreateSmallTree(int x, int minHeight, int maxHeight)
    {
        Point position = WorldManager.FindSurfaceBlock(x).Position;
        int height = Random.Range(minHeight, maxHeight + 1);
        for (int logY = 0; logY < height; logY++) // Vertical logs
        {
            WorldManager.Place(Blocks.BlockID.OakLog, new Point(position.X, position.Y + logY));
        }
        for (int leafX = 0; leafX < 3; leafX++) // 3 Horizontal leaves
        {
            WorldManager.Place(Blocks.BlockID.OakLeaf, new Point(position.X - 1 + leafX, position.Y + height));
        }
        WorldManager.Place(Blocks.BlockID.OakLeaf, new Point(position.X, position.Y + height + 1)); // 1 Leaf on top
    }
}

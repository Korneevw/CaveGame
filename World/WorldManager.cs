using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager // Operates World
{
    public static void Place(Blocks.BlockID block, Point position) 
    {
        try
        {
            if (World.BlockData[position.X, position.Y].ID == Blocks.Available[Blocks.BlockID.Air]().ID)
            {
                Object.Destroy(World.BlockData[position.X, position.Y].GameObject);
                World.BlockData[position.X, position.Y] = Blocks.Available[block]();
                World.BlockData[position.X, position.Y].Position = position;
                InstantiateBlock(World.BlockData[position.X, position.Y]);
            }
        }
        catch (System.IndexOutOfRangeException) { Debug.Log("Can't build on edges of the world or world height."); } // заменить на сообщение в чате
    }
    public static void Place(Block b)
    {
        try
        {
            if (World.BlockData[b.Position.X, b.Position.Y].ID == Blocks.Available[Blocks.BlockID.Air]().ID)
            {
                Object.Destroy(World.BlockData[b.Position.X, b.Position.Y].GameObject);
                World.BlockData[b.Position.X, b.Position.Y] = b;
                InstantiateBlock(b);
            }
        }
        catch (System.IndexOutOfRangeException) { Debug.Log("Can't build on edges of the world or world height."); } // заменить на сообщение в чате
    }
    public static void Destroy(Point position)
     {
        try
        {
            Object.Destroy(World.BlockData[position.X, position.Y].GameObject);
            World.BlockData[position.X, position.Y] = Blocks.Available[Blocks.BlockID.Air]();
            World.BlockData[position.X, position.Y].Position = position;
        }
        catch (System.IndexOutOfRangeException) { Debug.Log("Can't destroy on edges of the world or world height."); } // заменить на сообщение в чате
    }
    public static bool AreNeighboursSolid(Point position)
    {
        bool leftIsSolid = false;
        bool rightIsSolid = false;
        bool topIsSolid = false;
        bool bottomIsSolid = false;
        try { leftIsSolid = World.BlockData[position.X - 1, position.Y].IsSolid == true; } catch { }
        try { rightIsSolid = World.BlockData[position.X + 1, position.Y].IsSolid == true; } catch { }
        try { topIsSolid = World.BlockData[position.X, position.Y - 1].IsSolid == true; } catch { }
        try { bottomIsSolid = World.BlockData[position.X, position.Y + 1].IsSolid == true; } catch { }
        return leftIsSolid || rightIsSolid || topIsSolid || bottomIsSolid;
    }
    public static void InstantiateBlock(Block block)
    {
        GameObject gameObject = new GameObject();
        gameObject.transform.position = new Vector2(block.Position.X, block.Position.Y);
        gameObject.name = block.Name;
        gameObject.AddComponent<SpriteRenderer>().sprite = block.Texture;
        if (block.IsSolid) gameObject.AddComponent<BoxCollider2D>().size = new Vector2(1, 1);
        gameObject.layer = 3;
        block.GameObject = gameObject;
    }
    public static Block FindSurfaceBlock(int x)
    {
        for (int y = 0; y <= World.BlockData.GetUpperBound(1); y++)
        {
            try
            {
                if (World.BlockData[x, y].ID == Blocks.Available[Blocks.BlockID.Air]().ID)
                {
                    return World.BlockData[x, y];
                }
            }
            catch (System.IndexOutOfRangeException)
            {
                return null;
            }
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingEngine
{
    public static void UpdateBlockLighting(Point position, float brightness)
    {
        try
        {
            Color c = World.BlockData[position.X, position.Y].GameObject.GetComponent<SpriteRenderer>().color;
            float H;
            float S;
            float V;
            Color.RGBToHSV(c, out H, out S, out V);
            V = brightness;
            World.BlockData[position.X, position.Y].GameObject.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(H, S, V);
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("s");
        }
    }
    public static void UpdateWorldLighting()
    {
        float lightLevel = 1f;
        for (int x = 0; x < World.WorldWidth; x++)
        {
            BlockManager.FindSurfaceBlock(x).LightLevel = lightLevel;
            for (int y = BlockManager.FindSurfaceBlock(x).Position.Y; y >= 0; y--)
            {
                if (World.BlockData[x, y].ID != Blocks.BlockID.Air)
                {
                    World.BlockData[x, y].LightLevel = lightLevel;
                    if (lightLevel != 0f)
                    {
                        lightLevel -= 0.1f;
                    }
                }
            }
            lightLevel = 1f;
        }
    }
}

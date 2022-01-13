using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dirt : Block
{
    public Dirt()
    {
        ID = Blocks.BlockID.Dirt;
        Texture = Resources.Load<Sprite>("Sprites/dirtBlock");
        Name = "Dirt";
        IsSolid = true;
        Drop = Items.Available[Items.ItemID.DirtBlock]();
    }
}

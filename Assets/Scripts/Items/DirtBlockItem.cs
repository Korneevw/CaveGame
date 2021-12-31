using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtBlockItem : BlockItem
{
    public DirtBlockItem()
    {
        ID = Items.ItemID.DirtBlock;
        Texture = Resources.Load<Sprite>("Sprites/dirtBlock");
        BlockID = Blocks.BlockID.Dirt;
    }
}

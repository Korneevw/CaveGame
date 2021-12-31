using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakLeafBlockItem : BlockItem
{
    public OakLeafBlockItem()
    {
        ID = Items.ItemID.OakLeafBlock;
        Texture = Resources.Load<Sprite>("Sprites/oakLeafBlock");
        BlockID = Blocks.BlockID.OakLeaf;
    }
}

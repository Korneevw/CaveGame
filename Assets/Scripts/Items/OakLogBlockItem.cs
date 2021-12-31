using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakLogBlockItem : BlockItem
{
    public OakLogBlockItem()
    {
        ID = Items.ItemID.OakLogBlock;
        Texture = Resources.Load<Sprite>("Sprites/oakLogBlock");
        BlockID = Blocks.BlockID.OakLog;
    }
}

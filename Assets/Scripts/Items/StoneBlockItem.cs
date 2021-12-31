using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBlockItem : BlockItem
{
    public StoneBlockItem()
    {
        ID = Items.ItemID.StoneBlock;
        Texture = Resources.Load<Sprite>("Sprites/stoneBlock");
        BlockID = Blocks.BlockID.Stone;
    }
}

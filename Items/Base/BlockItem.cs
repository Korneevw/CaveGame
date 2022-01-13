using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockItem : Item
{
    protected Blocks.BlockID BlockID { get; set; }
    public sealed override void Use(Point position)
    {
        if (BlockManager.PlaceBlock(BlockID, position) == true) Count--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockItem : Item
{
    protected Blocks.BlockID BlockID;
    public sealed override void Use(Point position)
    {
        WorldManager.Place(BlockID, position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Items
{
    public enum ItemID
    {
        StoneBlock,
        DirtBlock,
        GrassBlock,
        OakLogBlock,
        OakLeafBlock
    }
    public static Dictionary<ItemID, Func<Item>> Available = new Dictionary<ItemID, Func<Item>>()
    {
        { ItemID.StoneBlock, () => new StoneBlockItem() },
        { ItemID.DirtBlock, () => new DirtBlockItem() },
        { ItemID.GrassBlock, () => new GrassBlockItem() },
        { ItemID.OakLogBlock, () => new OakLogBlockItem() },
        { ItemID.OakLeafBlock, () => new OakLeafBlockItem() },
    };
}

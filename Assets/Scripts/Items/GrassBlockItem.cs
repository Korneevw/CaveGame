using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBlockItem : BlockItem
{
    public GrassBlockItem()
    {
        ID = Items.ItemID.GrassBlock;
        Texture = Resources.Load<Sprite>("Sprites/grassBlock");
        BlockID = Blocks.BlockID.Grass;
    }
}

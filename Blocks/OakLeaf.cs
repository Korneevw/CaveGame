using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakLeaf : Block
{
    public OakLeaf()
    {
        ID = Blocks.BlockID.OakLeaf;
        Texture = Resources.Load<Sprite>("Sprites/oakLeafBlock");
        Name = "Oak Leaf";
        IsSolid = true;
    }
}

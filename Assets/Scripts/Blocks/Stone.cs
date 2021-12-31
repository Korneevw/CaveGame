using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Stone : Block
{
    public Stone()
    {
        ID = Blocks.BlockID.Stone;
        Texture = Resources.Load<Sprite>("Sprites/stoneBlock");
        Name = "Stone";
        IsSolid = true;
    }
}

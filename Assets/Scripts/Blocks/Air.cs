using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Air : Block
{
    public Air()
    {
        ID = Blocks.BlockID.Air;
        Texture = Resources.Load<Sprite>("Sprites/air");
        Name = "Air";
        IsSolid = false;
    }
}

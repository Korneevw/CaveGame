using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakLog : Block
{
    public OakLog()
    {
        ID = Blocks.BlockID.OakLog;
        Texture = Resources.Load<Sprite>("Sprites/oakLogBlock");
        Name = "Oak Log";
        IsSolid = true;
        Drop = Items.Available[Items.ItemID.OakLogBlock]();
    }
}

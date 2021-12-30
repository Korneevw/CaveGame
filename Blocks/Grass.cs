using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Block
{
    public Grass()
    {
        ID = Blocks.BlockID.Grass;
        Texture = Resources.Load<Sprite>("Sprites/grassBlock");
        Name = "Grass";
        IsSolid = true;
    }
}

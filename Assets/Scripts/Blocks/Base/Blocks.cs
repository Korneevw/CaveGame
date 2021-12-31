using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Blocks // Gives access to blocks
{
    public enum BlockID
    {
        Air,
        Stone,
        Dirt,
        Grass,
        OakLog,
        OakLeaf
    }
    public static Dictionary<BlockID, Func<Block>> Available = new Dictionary<BlockID, Func<Block>>()
    {
        { BlockID.Air, () => new Air() },
        { BlockID.Stone, () => new Stone() },
        { BlockID.Dirt, () => new Dirt() },
        { BlockID.Grass, () => new Grass() },
        { BlockID.OakLog, () => new OakLog() },
        { BlockID.OakLeaf, () => new OakLeaf() }
    };
}

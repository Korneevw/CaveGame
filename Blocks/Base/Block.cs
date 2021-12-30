using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Block
{
    public Blocks.BlockID ID { get; protected set; }
    public Sprite Texture { get; protected set; } = Resources.Load<Sprite>("Sprites/unknown");
    public string Name { get; protected set; } = "Unknown Block";
    public bool IsSolid { get; protected set; } = false;
    public GameObject GameObject;
    public Point Position;
}
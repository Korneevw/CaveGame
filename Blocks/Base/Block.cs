using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Block
{
    public Blocks.BlockID ID { get; protected set; }
    public Sprite Texture { get; protected set; } = Resources.Load<Sprite>("Sprites/unknown");
    public string Name { get; protected set; } = "Unknown Block";
    public bool IsSolid { get; protected set; } = false;
    public GameObject GameObject { get; set; }
    public Point Position { get; set; }
    private Item _drop;
    public Item Drop
    {
        get { return _drop; }
        set 
        { 
            value.Count = 1; 
            _drop = value;
        }
    }
    private float lightLevel = 1f;
    public float LightLevel
    {
        get
        {
            return lightLevel;
        }
        set
        {
            lightLevel = value;
            LightingEngine.UpdateBlockLighting(Position, lightLevel);
        }
    }
}
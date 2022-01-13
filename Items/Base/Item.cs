using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public const int StackSize = 64;
    public int Count { get; set; } = 0;
    public Items.ItemID ID { get; protected set; }
    public Sprite Texture { get; protected set; }
    public GameObject GameObjectInWorld { get; set; }
    public abstract void Use(Point position);
}

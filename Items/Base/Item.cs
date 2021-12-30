using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    // Implement later
    //public int Count = 1;
    //public const int StackSize = 64;
    public Items.ItemID ID { get; protected set; }
    public Sprite Texture { get; protected set; }
    public GameObject GameObject;
    public abstract void Use(Point position);
}

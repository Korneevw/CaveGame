using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int SlotCount { get; set; }
    public bool IsVisible { get; set; } = true;
    public InventorySlotB[] Slots { get; private set; }
    private Sprite _slotSprite;
    private void Start()
    {
        _slotSprite = Resources.Load<Sprite>("Sprites/invSlot");
        CreateInventory();
        SlotCount = Width * Height;
        Hide();
    }
    private void CreateInventory()
    {
        gameObject.name = "Inventory";
        Slots = new InventorySlotB[Width * Height];
        int index = 0;
        for (int yOffset = 0; yOffset < Height; yOffset++)
        {
            for (int xOffset = 0; xOffset < Width; xOffset++, index++)
            {
                GameObject slotGameObject = new GameObject();
                slotGameObject.name = $"Slot{index}";
                SpriteRenderer spriteRenderer = slotGameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = _slotSprite;
                slotGameObject.transform.parent = gameObject.transform;
                slotGameObject.transform.localPosition = new Vector3(spriteRenderer.sprite.bounds.size.x * xOffset, -(spriteRenderer.sprite.bounds.size.y * yOffset));
                InventorySlotB slot = slotGameObject.AddComponent<InventorySlotB>();
                Slots[index] = slot;
                slot.Index = index;
            }
        }
    }
    public void UseItem(int slot, Point position)
    {
        if (Slots[slot].Item != null)
        {
            Slots[slot].Item.Use(position);
            if (Slots[slot].Item.Count <= 0)
            {
                Slots[slot].Item = null;
            }
        }
    }
    public void AddItem(Items.ItemID ID, int count)
    {
        if (GetItemSlot(ID) != null)
        {
            InventorySlotB slot = GetItemSlot(ID);
            if (slot.Item.Count + count <= Item.StackSize)
            {
                slot.Item.Count += count;
            }
            else
            {
                count = count - (Item.StackSize - slot.Item.Count);
                slot.Item.Count += Item.StackSize - slot.Item.Count;
                AddItem(ID, count);
            }
        }
        else if (GetAvailableSlot() != null)
        {
            InventorySlotB slot = GetAvailableSlot();
            slot.Item = Items.Available[ID]();
            AddItem(ID, count);
        }
    }
    public void AddItem(Item item)
    {
        if (item != null)
        {
            if (GetItemSlot(item.ID) != null)
            {
                InventorySlotB slot = GetItemSlot(item.ID);
                if (slot.Item.Count + item.Count <= Item.StackSize)
                {
                    slot.Item.Count += item.Count;
                }
                else
                {
                    item.Count = item.Count - (Item.StackSize - slot.Item.Count);
                    slot.Item.Count += Item.StackSize - slot.Item.Count;
                    AddItem(item.ID, item.Count);
                }
            }
            else if (GetAvailableSlot() != null)
            {
                InventorySlotB slot = GetAvailableSlot();
                slot.Item = Items.Available[item.ID]();
                AddItem(item.ID, item.Count);
            }
        }
    }
    public void ShiftItems() // Temporary
    {
        for (int i = 1; i < Slots.Length; i++)
        {
            if (Slots[i - 1].Item == null)
            {
                Slots[i - 1].Item = Slots[i].Item;
                Slots[i].Item = null;
            }
        }
    }
    private InventorySlotB GetItemSlot(Items.ItemID ID)
    {
        foreach (InventorySlotB slot in Slots)
        {
            if (slot.Item != null)
            {
                if (slot.Item.ID == ID && slot.Item.Count <= 64)
                {
                    return slot;
                }
            }
        }
        return null;
    }
    private InventorySlotB GetAvailableSlot()
    {
        foreach (InventorySlotB slot in Slots)
        {
            if (slot.Item == null)
            {
                return slot;
            }
        }
        return null;
    }
    public void Display()
    {
        gameObject.SetActive(true);
        IsVisible = true;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        IsVisible = false;
    }
}

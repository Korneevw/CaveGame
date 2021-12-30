using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Sprite _inventoryTexture;
    private GameObject _gameObject;
    private const float Pixel = 1f / 16f;
    private const int SlotsHorizontal = 5;
    private const int SlotsVertical = 3;
    private InventorySlot[,] _slots;
    public bool IsVisible { get; private set; }
    public Inventory(Vector2 position)
    {
        _inventoryTexture = Resources.Load<Sprite>("Sprites/inventory3x5");
        CreateInventory(position);
        Hide();
    }
    private void CreateInventory(Vector2 position) // Creates GameObject for the inventory
    {
        _gameObject = new GameObject(); // Creates the GameObject
        SpriteRenderer spriteRenderer = _gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = _inventoryTexture;
        spriteRenderer.sortingOrder = 1; // Inventory is on top of everything
        _gameObject.transform.position = new Vector2(position.x, position.y);
        _slots = new InventorySlot[SlotsHorizontal, SlotsVertical]; // Initializes slots
        for (int slotX = 0; slotX < SlotsHorizontal; slotX++) 
        {
            for (int slotY = 0; slotY < SlotsVertical; slotY++)
            {
                _slots[slotX, slotY] = new InventorySlot();
                _slots[slotX, slotY].Position = new Point(slotX, slotY);
            }
        }
    } 
    public Vector2 GetPosition()
    {
        return _gameObject.transform.position;
    }
    public void SetPosition(Vector2 position)
    {
        _gameObject.transform.position = position;
        UpdateItemPositions();
    }
    private void UpdateItemPositions()
    {
        float x = _gameObject.transform.position.x + Pixel * 3f; // 3 Pixels right from the left bound
        float y = _gameObject.transform.position.y - Pixel * 3f;
        foreach (InventorySlot slot in _slots)
        {
            if (slot.Item != null)
            {
                float itemX = x + (1f + Pixel * 5f) * slot.Position.X;
                float itemY = y - (1f + Pixel * 5f) * slot.Position.Y;
                slot.Item.GameObject.transform.position = new Vector3(itemX, itemY);
            }
        }
    }
    private void CreateItem(InventorySlot slot) // Creates GameObject for the item
    {
        float x = _gameObject.transform.position.x + Pixel * 3f; // 3 Pixels right from the left bound
        float y = _gameObject.transform.position.y - Pixel * 3f; // 3 Pixel down from the upper bound
        if (_slots[slot.Position.X, slot.Position.Y].Item != null) // If item exists
        {
            if (_slots[slot.Position.X, slot.Position.Y].Item.GameObject == null) // And it's GameObject doesn't exist
            {
                x = x + (1f + Pixel * 5f) * slot.Position.X; // Calculates item position X
                y = y - (1f + Pixel * 5f) * slot.Position.Y; // Calculates item position Y
                GameObject gameObject = new GameObject();
                gameObject.transform.position = new Vector2(x, y); // Snapped position relative to inventory position
                SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = _slots[slot.Position.X, slot.Position.Y].Item.Texture;
                spriteRenderer.sortingOrder = _gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1; // Item is on top of inventory
                _slots[slot.Position.X, slot.Position.Y].Item.GameObject = gameObject; // Save the GameObject
            }
        }
    }
    public InventorySlot GetSlot(Point slot)
    {
        if (slot.X > SlotsHorizontal || slot.X < 0 || slot.Y > SlotsVertical || slot.Y < 0) // Checks invalid position
        {
            throw new System.IndexOutOfRangeException("Invalid slot position.");
        }
        return _slots[slot.X, slot.Y];
    } 
    public Item GetItem(Point slot) // Gets an item from the slot
    {
        if (slot.X > SlotsHorizontal || slot.X < 0 || slot.Y > SlotsVertical || slot.Y < 0) // Checks invalid position
        {
            throw new System.IndexOutOfRangeException("Invalid slot position.");
        }
        return _slots[slot.X, slot.Y].Item;
    } 
    public void SetItem(InventorySlot slot, Item item) // Sets item in a slot
    {
        if (slot.Item == null)
        {
            slot.Item = item;
            CreateItem(slot);
        }
    } 
    public void RemoveItem(InventorySlot slot) // Removes an item from the slot
    {
        if (slot.Item != null)
        {
            Object.Destroy(slot.Item.GameObject);
            slot.Item = null;
        }
    }
    public void AddItem(Item item) // Adds item in the first available slot
    {
        InventorySlot slot = FindAvailableSlot();
        if (slot != null)
        {
            slot.Item = item;
            CreateItem(slot);
        }
    } 
    public InventorySlot FindAvailableSlot()
    {
        for (int slotY = 0; slotY < SlotsVertical; slotY++)
        {
            for (int slotX = 0; slotX < SlotsHorizontal; slotX++)
            {
                if (_slots[slotX, slotY].Item == null)
                {
                    return _slots[slotX, slotY];
                }
            }
        }
        return null;
    }
    public void Display()
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.Item != null)
            {
                if (slot.Item.GameObject != null)
                {
                    slot.Item.GameObject.SetActive(true);
                }
            }
        }
        _gameObject.SetActive(true);
        IsVisible = true;
    }
    public void Hide()
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.Item != null)
            {
                if (slot.Item.GameObject != null)
                {
                    slot.Item.GameObject.SetActive(false);
                }
            }
        }
        _gameObject.SetActive(false);
        IsVisible = false;
    }
}

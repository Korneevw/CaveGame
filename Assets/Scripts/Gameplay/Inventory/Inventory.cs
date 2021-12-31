using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private const float Pixel = 1f / 16f;
    private GameObject _inventory;
    public int SlotCount;
    private InventorySlot[] _slots;
    public bool IsVisible { get; private set; } = true;
    public Inventory(Vector2 position, int width, int height)
    {
        CreateInventory(position, width, height);
        SlotCount = width * height;
    }
    public Inventory(Vector2 position, int width, int height, int slotCount)
    {
        CreateInventory(position, width, height, slotCount);
        SlotCount = slotCount;
    }
    private void CreateInventory(Vector2 position, int width, int height, int slotCount)
    {
        if (width * height < slotCount) throw new System.ArgumentException("width and height must be equal or bigger than slotCount");
        _inventory = new GameObject();
        _inventory.name = "Inventory";
        _inventory.transform.position = position;
        _inventory.AddComponent<SpriteRenderer>().sortingOrder = 1; // Inventory on top of everything

        _slots = new InventorySlot[slotCount];
        Sprite slotTexture = Resources.Load<Sprite>("Sprites/invSlot");
        float slotSizeX = slotTexture.bounds.size.x;
        float slotSizeY = slotTexture.bounds.size.y;
        float posX = 0;
        float posY = 0;
        int slotIndex = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++, slotIndex++)
            {
                if (slotIndex < slotCount)
                {
                    GameObject slotGameObject = new GameObject(); // Create slot GameObject
                    SpriteRenderer spriteRenderer = slotGameObject.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = slotTexture;
                    spriteRenderer.sortingOrder = _inventory.GetComponent<SpriteRenderer>().sortingOrder + 1; // Slots on top of the inventory
                    slotGameObject.transform.parent = _inventory.transform; // Parent is the inventory GameObject
                    slotGameObject.transform.localPosition = new Vector2(posX, -posY); // Snapped position
                    posX += slotSizeX; // Proper snapping

                    InventorySlot slot = new InventorySlot();
                    slot.Index = slotIndex;
                    slot.DrawingPosition = new Vector2(_inventory.transform.position.x + slotGameObject.transform.localPosition.x + Pixel * 3f,
                                                       _inventory.transform.position.y + slotGameObject.transform.localPosition.y - Pixel * 3f); // Proper drawing position
                    slot.GameObject = slotGameObject;
                    _slots[slotIndex] = slot;
                }
            }
            posY += slotSizeY;
            posX = 0;
        }
    }
    private void CreateInventory(Vector2 position, int width, int height)
    {
        _inventory = new GameObject();
        _inventory.name = "Inventory";
        _inventory.transform.position = position;
        _inventory.AddComponent<SpriteRenderer>().sortingOrder = 1; // Inventory on top of everything

        _slots = new InventorySlot[width * height];
        Sprite slotTexture = Resources.Load<Sprite>("Sprites/invSlot");
        float slotSizeX = slotTexture.bounds.size.x;
        float slotSizeY = slotTexture.bounds.size.y;
        float posX = 0;
        float posY = 0;
        int slotIndex = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++, slotIndex++)
            {
                GameObject slotGameObject = new GameObject(); // Create slot GameObject
                SpriteRenderer spriteRenderer = slotGameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = slotTexture;
                spriteRenderer.sortingOrder = _inventory.GetComponent<SpriteRenderer>().sortingOrder + 1; // Slots on top of the inventory
                slotGameObject.transform.parent = _inventory.transform; // Parent is the inventory GameObject
                slotGameObject.transform.localPosition = new Vector2(posX, -posY); // Snapped position
                posX += slotSizeX; // Proper snapping

                InventorySlot slot = new InventorySlot();
                slot.Index = slotIndex;
                slot.DrawingPosition = new Vector2(_inventory.transform.position.x + slotGameObject.transform.localPosition.x + Pixel * 3f,
                                                   _inventory.transform.position.y + slotGameObject.transform.localPosition.y - Pixel * 3f); // Proper drawing position
                slot.GameObject = slotGameObject;
                _slots[slotIndex] = slot;
            }
            posY += slotSizeY;
            posX = 0;
        }
    }
    public void Destroy()
    {
        Object.Destroy(_inventory);
        foreach (InventorySlot slot in _slots)
        {
            Object.Destroy(slot.GameObject);
            if (slot.Item != null)
            {
                if (slot.Item.GameObject != null)
                {
                    Object.Destroy(slot.Item.GameObject);
                }
            }
        }
    }
    public Vector2 GetPosition()
    {
        return _inventory.transform.position;
    }
    public void SetPosition(Vector2 position)
    {
        _inventory.transform.position = position;
        UpdateItemPositions();
    }
    private void UpdateItemPositions()
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.Item != null)
            {
                slot.DrawingPosition = new Vector2(_inventory.transform.position.x + slot.GameObject.transform.localPosition.x + Pixel * 3f,
                                                   _inventory.transform.position.y + slot.GameObject.transform.localPosition.y - Pixel * 3f);
                slot.Item.GameObject.transform.position = slot.DrawingPosition;
            }
        }
    }
    private void CreateItem(int slot) // Creates GameObject for the item
    {
        if (_slots[slot].Item != null) // If item exists
        {
            if (_slots[slot].Item.GameObject == null) // And it's GameObject doesn't exist
            {
                GameObject gameObject = new GameObject();
                gameObject.transform.position = _slots[slot].DrawingPosition; // Snapped position
                SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = _slots[slot].Item.Texture;
                spriteRenderer.sortingOrder = _slots[slot].GameObject.GetComponent<SpriteRenderer>().sortingOrder + 1; // Item is on top of slot
                if (IsVisible == false) gameObject.SetActive(false);
                _slots[slot].Item.GameObject = gameObject; // Save the GameObject
            }
        }
    }
    public Item GetItem(int slot)
    {
        if (slot < 0 || slot > _slots.Length) throw new System.ArgumentException("Invalid slot index.");
        return _slots[slot].Item;
    } 
    public void SetItem(int slot, Item item) // Sets item in a slot
    {
        if (slot < 0 || slot > _slots.Length) throw new System.ArgumentException("Invalid slot index.");
        RemoveItem(slot);
        if (_slots[slot].Item == null)
        {
            _slots[slot].Item = item;
            CreateItem(slot);
        }
    } 
    public void RemoveItem(int slot) // Removes an item from the slot
    {
        if (_slots[slot].Item != null)
        {
            Object.Destroy(_slots[slot].Item.GameObject);
            _slots[slot].Item = null;
        }
    }
    public void AddItem(Item item) // Adds item in the first available slot
    {
        int slot = FindAvailableSlot();
        if (_slots[slot] != null)
        {
            _slots[slot].Item = item;
            CreateItem(slot);
        }
    } 
    public int FindAvailableSlot()
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.Item == null)
            {
                return slot.Index;
            }
        }
        return -1;
    }
    public void Display()
    {
        foreach (InventorySlot slot in _slots)
        {
            if (slot.Item != null)
            {
                if (slot.Item.GameObject != null)
                {
                    slot.GameObject.SetActive(true);
                    slot.Item.GameObject.SetActive(true);
                }
            }
        }
        _inventory.SetActive(true);
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
                    slot.GameObject.SetActive(false);
                    slot.Item.GameObject.SetActive(false);
                }
            }
        }
        _inventory.SetActive(false);
        IsVisible = false;
    }
}

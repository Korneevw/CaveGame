using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InventorySlotB : MonoBehaviour
{
    public Vector2 DrawingPosition
    {
        get
        {
            return new Vector3(Measurements.Pixel * 3f, -(Measurements.Pixel * 3f));
        }
    }
    [HideInInspector] public int Index { get; set; }
    private Item _item;
    private GameObject _displayedItem;
    public Item Item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
            if (value != null)
            {
                CreateItem();
            }
            else
            {
                DestroyItem();
            }
        }
    }
    private void DestroyItem()
    {
        if (_displayedItem != null)
        {
            Object.Destroy(_displayedItem);
            _displayedItem = null;
        }
    }
    private void CreateItem()
    {
        if (_displayedItem == null)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = $"{Item.ID}";
            gameObject.transform.parent = this.gameObject.transform;
            gameObject.transform.localPosition = DrawingPosition;
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Item.Texture;
            spriteRenderer.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
            _displayedItem = gameObject;
        }
    }
}

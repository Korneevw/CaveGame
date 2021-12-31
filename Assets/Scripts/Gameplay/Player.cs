using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider2D;
    private Inventory _inventory;
    private Vector3 _inventoryPosition;

    public bool CustomPosition;
    public int PositionX;
    public int PositionY;

    public int InventoryWidth;
    public int InventoryHeight;
    public int InventorySlotCount;

    public float DefaultMovementSpeed;
    public float MovementSpeed;
    public float JumpForce;
    public LayerMask GroundLayer;

    public bool Creative;
    public bool Inventory;
    public bool RectangularInventory;
    private void Start()
    {
        transform.position = new Vector2(World.WorldWidth / 2, WorldManager.FindSurfaceBlock(World.WorldWidth / 2).Position.Y + 3);
        MovementSpeed = DefaultMovementSpeed;
        if (CustomPosition == true) transform.position = new Vector2(PositionX, PositionY);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        if (Creative == true) _rigidBody.gravityScale = 0;
        _boxCollider2D = GetComponent<BoxCollider2D>();

        if (Inventory == true)
        {
            _inventoryPosition = new Vector3(1, 4);
            // Decide default inventory
            if (RectangularInventory == true) _inventory = new Inventory(transform.position + _inventoryPosition, InventoryWidth, InventoryHeight);
            else _inventory = new Inventory(transform.position + _inventoryPosition, InventoryWidth, InventoryHeight, InventorySlotCount);
            _inventory.Hide();
        }
    }
    private void Update()
    {
        HandleBlockInteractions();
        HandleCameraZoom();
        UpdateCameraPosition();
        if (Inventory == true && _inventory != null)
        {
            HandleInventoryVisibility();
            UpdateInventoryPosition();
            HandleInventoryAddition();
            HandleInventoryRemoval();
        }
        if (Creative == false) 
        {
            HandleJumping();
            _rigidBody.gravityScale = 3;
            MovementSpeed = DefaultMovementSpeed;
        }
        if (Creative == true)
        {
            HandleExtra();
            HandleMovementSpeed();
            _rigidBody.gravityScale = 0;
        }
    }
    private void FixedUpdate()
    {
        HandleMovement();
        if (Creative == true) HandleFlying();
    }
    private void HandleExtra()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2)) // Teleportation
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
        if (Input.GetKeyDown(KeyCode.F)) // Inventory Creation
        {
            _inventory.Destroy();
            if (RectangularInventory == true) _inventory = new Inventory(transform.position + _inventoryPosition, InventoryWidth, InventoryHeight);
            else _inventory = new Inventory(transform.position + _inventoryPosition, InventoryWidth, InventoryHeight, InventorySlotCount);
        }
    }
    private void HandleInventoryAddition() // Implement selector!
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _inventory.SetItem(0, Items.Available[Items.ItemID.StoneBlock]());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _inventory.SetItem(0, Items.Available[Items.ItemID.DirtBlock]());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _inventory.SetItem(0, Items.Available[Items.ItemID.GrassBlock]());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _inventory.SetItem(0, Items.Available[Items.ItemID.OakLogBlock]());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _inventory.SetItem(0, Items.Available[Items.ItemID.OakLeafBlock]());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            for (int i = 0; i < _inventory.SlotCount; i++)
            {
                _inventory.SetItem(i, Items.Available[Items.ItemID.OakLeafBlock]());
            }
        }
    }
    private void HandleInventoryRemoval()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _inventory.RemoveItem(0);
        }
    }
    private void HandleBlockInteractions()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Point p = new Point(Mathf.FloorToInt(mousePos.x), Mathf.CeilToInt(mousePos.y));
            WorldManager.Destroy(p);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Point p = new Point(Mathf.FloorToInt(mousePos.x), Mathf.CeilToInt(mousePos.y));
            if (WorldManager.AreNeighboursSolid(p))
            {
                if (_inventory.GetItem(0) != null)
                {
                    _inventory.GetItem(0).Use(p);
                }
            }
        }
    }
    private void HandleMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(horizontalMovement * Time.deltaTime * MovementSpeed, 0, 0);
    }
    private void HandleFlying()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        transform.position += new Vector3(0, verticalMovement * Time.deltaTime * MovementSpeed, 0);
    }
    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckGrounded() == true)
            {
                _rigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            }
        }
    }
    private void HandleMovementSpeed()
    {
        float wheelMovement = Input.mouseScrollDelta.y;
        if (MovementSpeed + wheelMovement / 2f >= 0 && MovementSpeed + wheelMovement / 2f <= 50)
        {
            MovementSpeed += wheelMovement / 2f;
        }
    }
    private bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_boxCollider2D.bounds.center, _boxCollider2D.bounds.size - new Vector3(0.1f, 0, 0), 0f, Vector2.down, 0.1f, GroundLayer);
        return hit.collider != null;
    }
    private void HandleCameraZoom()
    {
        float step = 0.5f;
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            if (Camera.main.orthographicSize >= 0 && Camera.main.orthographicSize - 0.5f >= 0)
            {
                Camera.main.orthographicSize -= step;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            if (Camera.main.orthographicSize <= 50 && Camera.main.orthographicSize + 0.5f <= 50)
            {
                Camera.main.orthographicSize += step;
            }
        }
    }
    private void HandleInventoryVisibility()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_inventory.IsVisible == false)
            {
                _inventory.Display();
            }
            else if (_inventory.IsVisible == true)
            {
                _inventory.Hide();
            }
        }
    }
    private void UpdateCameraPosition()
    {
        Camera.main.transform.position = new Vector3(_spriteRenderer.bounds.center.x, _spriteRenderer.bounds.center.y, -10);
    }
    private void UpdateInventoryPosition()
    {
        _inventory.SetPosition(transform.position + _inventoryPosition);
    }
}

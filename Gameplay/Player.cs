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
    private const float Pixel = 1 / 16f;

    public bool CustomPosition;
    public int PositionX;
    public int PositionY;
    public float MovementSpeed;
    public LayerMask GroundLayer;
    public float JumpForce;
    public bool Creative;
    private void Start()
    {
        transform.position = new Vector2(World.WorldWidth / 2, WorldManager.FindSurfaceBlock(World.WorldWidth / 2).Position.Y + 3);
        if (CustomPosition == true) transform.position = new Vector2(PositionX, PositionY);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        if (Creative == true) _rigidBody.gravityScale = 0;
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _inventoryPosition = new Vector3(1, 4);
        _inventory = new Inventory(transform.position + _inventoryPosition);
        _inventory.Hide();
    }
    private void Update()
    {
        HandleBlockInteractions();
        HandleCameraZoom();
        UpdateCameraPosition();
        HandleInventoryVisibility();
        UpdateInventoryPosition();
        HandleInventoryAddition();
        HandleInventoryRemoval();
        if (Creative == false) 
        {
            HandleJumping();
            _rigidBody.gravityScale = 3;
            MovementSpeed = 3.5f;
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
    }
    private void HandleInventoryAddition() // Implement selector!
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _inventory.SetItem(_inventory.GetSlot(new Point(0, 0)),Items.Available[Items.ItemID.StoneBlock]());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _inventory.SetItem(_inventory.GetSlot(new Point(0, 0)), Items.Available[Items.ItemID.DirtBlock]());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _inventory.SetItem(_inventory.GetSlot(new Point(0, 0)), Items.Available[Items.ItemID.GrassBlock]());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _inventory.SetItem(_inventory.GetSlot(new Point(0, 0)), Items.Available[Items.ItemID.OakLogBlock]());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _inventory.SetItem(_inventory.GetSlot(new Point(0, 0)), Items.Available[Items.ItemID.OakLeafBlock]());
        }
    }
    private void HandleInventoryRemoval()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _inventory.RemoveItem(_inventory.GetSlot(new Point(0, 0)));
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
                if (_inventory.GetItem(new Point(0, 0)) != null)
                {
                    _inventory.GetItem(new Point(0, 0)).Use(p);
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
        MovementSpeed += wheelMovement / 2f;
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
        if (_inventory.IsVisible == true)
        {
            _inventory.SetPosition(transform.position + _inventoryPosition);
        }
    }
}

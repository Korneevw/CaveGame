using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider2D;
    private GameObject _inventoryGameObject;
    public static Camera ViewCamera;
    private Vector3 _inventoryPosition;
    public Inventory Inventory { get; private set; }

    public bool CustomPosition;
    public int PositionX;
    public int PositionY;

    public float DefaultMovementSpeed;
    public float MovementSpeed;
    public float JumpForce;
    public LayerMask GroundLayer;

    public bool Creative;
    public bool IsInventory;
    private void Awake()
    {
        ViewCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    private void Start()
    {
        ViewCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        MovementSpeed = DefaultMovementSpeed;
        if (CustomPosition == false) transform.position = new Vector2(World.WorldWidth / 2, BlockManager.FindSurfaceBlock(World.WorldWidth / 2).Position.Y + 3);
        else if (CustomPosition == true) transform.position = new Vector2(PositionX, PositionY);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
        if (Creative == true) _rigidBody.gravityScale = 0;
        _boxCollider2D = GetComponent<BoxCollider2D>();

        if (IsInventory == true)
        {
            _inventoryPosition = new Vector3(1, 4);
            _inventoryGameObject = new GameObject();
            Inventory = _inventoryGameObject.AddComponent<Inventory>();
            Inventory.Width = 3;
            Inventory.Height = 3;
        }
    }
    private void Update()
    {
        HandleBlockInteractions();
        HandleCameraZoom();
        UpdateCameraPosition();
        if (IsInventory == true && Inventory != null)
        {
            HandleInventoryVisibility();
            UpdateInventoryPosition();
            HandleInventoryAddition();
        }
        if (Creative == false) 
        {
            HandleJumping();
            if (MovementSpeed != DefaultMovementSpeed)
            {
                MovementSpeed = DefaultMovementSpeed;
            }
        }
        if (Creative == true)
        {
            HandleExtra();
            HandleMovementSpeed();
        }
    }
    private void FixedUpdate()
    {
        HandleMovement();
        if (Creative == true) HandleFlying();
    }
    private void HandleExtra()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
    }
    private void HandleInventoryAddition()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.AddItem(Items.ItemID.StoneBlock, 1);
        }
    }
    private void HandleBlockInteractions()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePos = ViewCamera.ScreenToWorldPoint(Input.mousePosition);
            Point p = new Point(Mathf.FloorToInt(mousePos.x), Mathf.CeilToInt(mousePos.y));
            Inventory.AddItem(BlockManager.DestroyBlock(p).Drop);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Vector2 mousePos = ViewCamera.ScreenToWorldPoint(Input.mousePosition);
            Point p = new Point(Mathf.FloorToInt(mousePos.x), Mathf.CeilToInt(mousePos.y));
            if (BlockManager.AreNeighboursSolid(p))
            {
                Inventory.UseItem(0, p);
                Inventory.ShiftItems(); // Temporary
            }
        }
    }
    private void HandleMovement()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(horizontalMovement * Time.deltaTime * MovementSpeed, 0, 0);
    }
    private void HandleFlying() // Creative
    {
        if (_rigidBody.gravityScale != 0)
        {
            _rigidBody.gravityScale = 0;
        }
        float verticalMovement = Input.GetAxis("Vertical");
        transform.position += new Vector3(0, verticalMovement * Time.deltaTime * MovementSpeed, 0);
    }
    private void HandleJumping()
    {
        if (_rigidBody.gravityScale != 3)
        {
            _rigidBody.gravityScale = 3;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckGrounded() == true)
            {
                _rigidBody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            }
        }
    }
    private void HandleMovementSpeed() // Creative
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
            if (ViewCamera.orthographicSize > 0 && Camera.main.orthographicSize - 0.5f > 0)
            {
                ViewCamera.orthographicSize -= step;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            if (ViewCamera.orthographicSize <= 50 && Camera.main.orthographicSize + 0.5f <= 50)
            {
                ViewCamera.orthographicSize += step;
            }
        }
    }
    private void HandleInventoryVisibility()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Inventory.IsVisible == false)
            {
                Inventory.Display();
            }
            else if (Inventory.IsVisible == true)
            {
                Inventory.Hide();
            }
        }
    }
    private void UpdateCameraPosition()
    {
        Vector2 mousePos = ViewCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        float movementModifier = 0.1f;
        ViewCamera.transform.position = new Vector3(transform.position.x + distance.x * movementModifier, transform.position.y + distance.y * movementModifier, -10);
    }
    private void UpdateInventoryPosition()
    {
        _inventoryGameObject.transform.position = (transform.position + _inventoryPosition);
    }
}

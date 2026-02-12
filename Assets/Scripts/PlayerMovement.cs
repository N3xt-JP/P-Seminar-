using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{ 
    public Rigidbody2D rb;
    private new Camera camera;
    private new Collider2D collider;

    public float moveSpeed = 5f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float JumpForce => 2f * maxJumpHeight / (maxJumpTime / 2f);
    public float Gravity => -2f * maxJumpHeight / Mathf.Pow(maxJumpTime / 2f, 2);

    public bool Grounded { get; private set; }
    public bool Jumping { get; private set; }
    public bool running => Mathf.Abs(rb.linearVelocity.x) > 0.1f && Grounded;
    public bool sliding => (rb.linearVelocity.x > 0f && transform.eulerAngles.y == 180f || rb.linearVelocity.x < 0f && transform.eulerAngles.y == 0f) && Grounded;

    
    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    float horizontalMovement;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        camera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        collider = GetComponent<Collider2D>();
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
    }

    private void OnEnable()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        collider.enabled = true;
        horizontalMovement = 0f;
        Jumping = false;
        touchPressAction.performed += TouchPressed;
        touchPositionAction.performed += TouchPosition;
    }

    private void OnDisable()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        collider.enabled = false;
        horizontalMovement = 0f;
        Jumping = false;
        touchPressAction.performed -= TouchPressed;
        touchPositionAction.performed -= TouchPosition;
    }
    private void Update()
    {   
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        // Check if grounded
        Grounded = rb.Raycast(Vector2.down);    
        if (Grounded)
        {
            GroundedMovement();
        }
        ApplyGravity();
    }

    // Input System Callbacks
    public void Move (InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;     
        //fix horizontal movement speed building up when moving against a wall
        if ((horizontalMovement > 0 && rb.Raycast(Vector2.right)) || (horizontalMovement < 0 && rb.Raycast(Vector2.left)))
        {
            horizontalMovement = 0f;
        }
        if (horizontalMovement > 0f)
        {transform.eulerAngles = new Vector3(0f, 0f, 0f);}
        else if (horizontalMovement < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        
    }
    
    public void Jump (InputAction.CallbackContext context)
    {
        //Jump only when grounded
        if (context.performed && Grounded)
        {
            Jumping = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
        }else if (context.canceled)
        // hold to jump higher
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }
      
        private void GroundedMovement()
    {   
        Jumping = rb.linearVelocity.y > 0f;
        rb.linearVelocity =  Mathf.Max(rb.linearVelocity.y, 0f) * Vector2.up + rb.linearVelocity.x * Vector2.right;
        //cancel downward velocity when grounded
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
    }    
    private void ApplyGravity()
    {   bool falling = rb.linearVelocity.y <= 0 || !Jumping;
        float multiplier = falling ? 2f : 1f;
        // Gravity function
        rb.linearVelocity += new Vector2(0, Gravity * multiplier * Time.deltaTime);
        //maximum fall speed
        rb.linearVelocity = Mathf.Max(rb.linearVelocity.y, Gravity / 2f) * Vector2.up + rb.linearVelocity.x * Vector2.right;
    }
    // Keep player within camera bounds
    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        Vector2 leftEdge = camera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);
        rb.position = position;
    }
    //fix HeadSticking on ceilings
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if(transform.DotTest(collision.transform, Vector2.down))
            {
                rb.linearVelocityY = JumpForce / 2f;
                Jumping = true;
            }

        }
        if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, 0f));
            }
            
        }
    }

    public void TouchPressed(InputAction.CallbackContext context)
    {
        //Jump only when grounded
        if (context.performed && Grounded)
        {
            Jumping = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
        }else if (context.canceled)
        // hold to jump higher
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    public void TouchPosition(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;     
        //fix horizontal movement speed building up when moving against a wall
        if ((horizontalMovement > 0 && rb.Raycast(Vector2.right)) || (horizontalMovement < 0 && rb.Raycast(Vector2.left)))
        {
            horizontalMovement = 0f;
        }
    }

}

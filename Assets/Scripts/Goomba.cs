using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;
    public float framesPerSecond = 2f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float JumpForce => 2f * maxJumpHeight / (maxJumpTime / 2f);
    private bool Jumping;
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.up;
    private Vector2 velocity;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<EntityMovement>().enabled = true;
        velocity.y = Physics2D.gravity.y * Time.deltaTime;
        Jumping = velocity.y>0f;
    }

    private void OnEnable()
    {
        if(!Jumping){
            InvokeRepeating(nameof(Jump), framesPerSecond, framesPerSecond);
        }
        
    }   
    
    private void OnDisable()
    {
        CancelInvoke(nameof(Jump));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player player))
        {

            if (player.Starpower)
             {
                Flatten();
             }else if (collision.transform.DotTest(transform, Vector2.down))
            {
                Flatten();
            } else {
                player.Hit();
            }
             
        }
    }
    
    

    private void Flatten()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimatedSprites>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        Destroy(gameObject, 0.5f);
    }

    private void Jump()
    {
        Jumping = true;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpForce);
    }
}

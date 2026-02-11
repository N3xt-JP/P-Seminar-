using UnityEngine;

public class PlayerSpriteRenderer:MonoBehaviour
{
    public SpriteRenderer spriteRenderer {get; private set;}
    private PlayerMovement movement;

    public Sprite idle;
    public AnimatedSprites running;
    public Sprite jumping;
    public Sprite sliding;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerMovement>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }
    private void LateUpdate()
    {
        running.enabled = movement.running;
        if (movement.Jumping){
            spriteRenderer.sprite = jumping;
        }else if (movement.sliding){
            spriteRenderer.sprite = sliding;
        }else if (!movement.running){
            spriteRenderer.sprite = idle;
        }
        
    }


}

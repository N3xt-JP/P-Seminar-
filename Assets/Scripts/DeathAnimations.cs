using System.Collections;
using System.Numerics;
using UnityEngine;

public class DeathAnimations : MonoBehaviour
{
   
    public SpriteRenderer spriteRenderer;
    public Sprite deadSprite;

    private void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        UpdateSprite();
        DisablePhysics();  
        StartCoroutine(Animate());
    }

    private void UpdateSprite()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sortingOrder = 10;

        spriteRenderer.sprite = deadSprite;

        if(deadSprite != null)
        {
            spriteRenderer.sprite = deadSprite;
        }
    }

    private void DisablePhysics()
    {
        Collider2D[] colliders = GetComponents<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        EntityMovement entityMovement = GetComponent<EntityMovement>();
        AnimatedSprites Animated = GetComponent<AnimatedSprites>();

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            Animated.enabled = false;
        }
        
        if (entityMovement != null)
        {
            entityMovement.enabled = false;
            Animated.enabled = false;
        }
    }

    private IEnumerator Animate()
    {
        float elapsed = 0f;
        float duration = 3f;

        float jumpVelocity = 10f;
        float gravity = -36f;

        UnityEngine.Vector3 velocity = UnityEngine.Vector3.up * jumpVelocity;

        while (elapsed  < duration)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y = gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }


    }
}

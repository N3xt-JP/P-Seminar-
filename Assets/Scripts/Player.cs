using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //normal PlayerState gets Rendered;
    public PlayerSpriteRenderer playerRenderer;
    

    private DeathAnimations deathAnimation;
    public bool player => playerRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool Starpower { get; private set; }

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimations>();
    }
    public void Hit()
    {
        if (!dead && !Starpower)
        {
            if (player)
            {
             Death();
            }
        }
    }
    
    
    private void Death()
    {
        playerRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

    public void StarMan(float duration = 10f)
    {
        StartCoroutine(StarpowerAnimation(duration));
    }

    private IEnumerator StarpowerAnimation(float duration)
    {
        Starpower = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0)
            {
                playerRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }   

            yield return null;
        }
        

        
        playerRenderer.spriteRenderer.color = Color.white;
        Starpower = false;
    }
}

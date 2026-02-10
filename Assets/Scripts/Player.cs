using UnityEngine;

public class Player : MonoBehaviour
{
    //normal PlayerState gets Rendered;
    public PlayerSpriteRenderer playerRenderer;

    private DeathAnimations deathAnimation;
    public bool player => playerRenderer.enabled;
    public bool dead => deathAnimation.enabled;

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimations>();
    }
    public void Hit()
    {
        if (player)
        {
            Death();
        }
    }
    
    private void GetRidOfBuff()
    {
        //if enhanced by buff e.g FireFlower gets rid 
        //TODO because no flower yet
    }
    private void Death()
    {
        playerRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

}

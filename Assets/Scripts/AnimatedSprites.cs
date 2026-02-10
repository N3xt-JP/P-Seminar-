using UnityEngine;
using UnityEngine.Timeline;

public class AnimatedSprites:MonoBehaviour
{
    public Sprite[] sprites;
    public float framesPerSecond = 1/6f;

    private SpriteRenderer spriteRenderer;
    private int Frame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(Animate), framesPerSecond, framesPerSecond);
    }   
    
    private void OnDisable()
    {
        CancelInvoke(nameof(Animate));
    }

    private void Animate()
    {
        Frame++;
        if (Frame >= sprites.Length)
        {
            Frame = 0;
        }
        if(Frame >= 0 && Frame < sprites.Length)
        {
        spriteRenderer.sprite = sprites[Frame];
        }
    }

}

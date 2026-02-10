using System.Collections;
using UnityEngine;

public class BlockHit:MonoBehaviour
{

    public int  MaxHits = -1;
    public Sprite emptyBlock;
    private bool animating = false;
    public GameObject item;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!animating &&  MaxHits !=0 && collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.transform.DotTest(transform, Vector2.up))
            {
                Hit();
            }
        }
    }
    private void Hit()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;

        MaxHits--;
        if (MaxHits == 0)
        {
            spriteRenderer.sprite = emptyBlock;
        }

        if (item != null)
        {
           Instantiate(item, transform.position, Quaternion.identity);
        }

        StartCoroutine(Animate());

    }

    private IEnumerator Animate()
    {
        animating = true;
        
        Vector3 restingPosition = transform.localPosition; 
        Vector3 animatingPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatingPosition);
        yield return Move(animatingPosition, restingPosition);

        
        animating = false;

    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        float duration = 0.1f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            
            transform.localPosition =  Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = to;
    }
}
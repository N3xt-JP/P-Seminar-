using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class Flagpole : MonoBehaviour
{   
    public Transform flag;
    public Transform poleBottom;
    public Transform entrance;
    public float speed = 6f;
    public int nextWorld = 1;
    public int nextStage = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelComplete(other.transform));
        }
    }

    private IEnumerator LevelComplete(Transform Player)
    {
        Player.GetComponent<PlayerMovement>().enabled = false;

        yield return MoveTo(Player, poleBottom.position);
        yield return MoveTo(Player, Player.position + Vector3.right);
        yield return MoveTo(Player, Player.position + Vector3.right + Vector3.down);
        yield return MoveTo(Player, entrance.position);

        Player.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);   
        GameManager.Instance.LoadLevel(nextWorld, nextStage);
    }
    
    private IEnumerator MoveTo(Transform subject, Vector3 destination)
    {
        while (Vector3.Distance(subject.position, destination) > 0.1f)
        {
            subject.position = Vector3.MoveTowards(subject.position, destination, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = destination;
    }
}

using UnityEngine;
using DG.Tweening;

public class Rats : MonoBehaviour
{
    private Sequence sequence;
    
    private void Start()
    {
        Move();
    }

    private void Move()
    {
        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOMoveX(transform.position.x - 25f, 2f))
            .OnComplete(() => Destroy(gameObject));
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Hit Rats");
            collision.gameObject.transform.DOJump(collision.gameObject.transform.position+Vector3.left*2f, 1f, 1, 0.35f);
        }
    }
}

using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class Rock : MonoBehaviour
{
    private Sequence sequence;
    private Vector3 targetPosition;
    
    private void Start()
    {
        targetPosition = transform.position + Vector3.down * 13f;
        targetPosition += Vector3.left * 5f;
        Move();
    }

    private void Move()
    {
        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOJump(targetPosition, 1f, 1, 0.5f))
            .Join(transform.DORotate(new Vector3(0, 0, 180), 0.5f, RotateMode.FastBeyond360))
            .Append(transform.DOJump(targetPosition + Vector3.left * 5f, 5f, 1, 0.5f))
            .Join(transform.DORotate(new Vector3(0, 0, 270), 0.5f, RotateMode.FastBeyond360))
            .OnComplete(() => Destroy(gameObject));
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Hit Rock");
            collision.gameObject.transform.DOJump(collision.gameObject.transform.position+Vector3.left*2f, 1f, 1, 0.35f);

            //if(sequence.IsPlaying())
            //    sequence.Kill();
            //Destroy(gameObject);
        }
    }
}
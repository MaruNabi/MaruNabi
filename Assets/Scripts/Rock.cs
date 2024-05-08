using System;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class Rock : MonoBehaviour
{
    private Sequence sequence;
    
    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.left * 5f;
        Move();
    }

    private void Move()
    {
        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(5f)
            .OnComplete(() => Destroy(gameObject));
    }
    
    private void OnDestroy()
    {
        sequence.Kill();
    }
}
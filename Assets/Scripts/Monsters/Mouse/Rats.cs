using System;
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
            .Join(transform.DOMoveY(transform.position.y - 2.61f, 2f))
            .OnComplete(() => Destroy(gameObject));
    }
    
    private void OnDestroy()
    {
        sequence.Kill();
    }
}

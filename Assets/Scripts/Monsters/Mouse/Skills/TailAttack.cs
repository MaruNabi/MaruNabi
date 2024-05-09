using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TailAttack : MonoBehaviour
{
    private Sequence sequence;
    private Vector3 startPos;
    
    private void Start()
    {
        startPos = transform.position;
        Move();
    }

    private void Move()
    {
        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOPunchPosition(Vector3.left*8f + Vector3.up*5f + Vector3.down, 1.5f, 1))
            .Join(transform.DOMove(startPos, 1.5f))
            .OnComplete(() => Destroy(gameObject));
    }
}

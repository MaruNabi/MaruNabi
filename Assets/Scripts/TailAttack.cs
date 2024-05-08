using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TailAttack : MonoBehaviour
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
            .Append(transform.DOPunchPosition(Vector3.left*8f + Vector3.down, 1.5f, 1))
            .Join(transform.DOPunchScale(Vector3.left*8f, 1.5f, 1))
            .OnComplete(() => Destroy(gameObject));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Hit Tail");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TigerSideAtk : MonoBehaviour
{
    Sequence sequence;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Move(bool _isRight)
    {
        float x = _isRight ? 20f : -20f;
        
        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOMoveX(transform.position.x - x/4, 0.35f))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                animator.enabled = true;
                animator.SetTrigger("Go");
            })
            .Append(transform.DOMoveX(transform.position.x - x, 0.35f))
            .Append(transform.DOMoveX(transform.position.x + x/2, 0.35f))
            .OnComplete(()=> Destroy(gameObject));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = Utils.GetOrAddComponent<Player>(collision.gameObject);
            if (player.IsInvincibleTime == false && player.GetIsDeadBool() == false)
            {
                player.PlayerHit(transform.position, false);
            }
        }
    }
}
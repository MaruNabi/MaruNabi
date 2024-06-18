using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class TigerSideAtk2 : MonoBehaviour, IDelete
{
    Sequence sequence;
    Animator animator;
    SpriteRenderer spriteRenderer;

    private void Init()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void Move(bool _isRight)
    {
        Init();
        spriteRenderer.flipX = !_isRight;
   
        float firstX = _isRight ? -3.2f : 3.2f;
        float x = _isRight ? -12f : 12f;
        
        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOMoveX(transform.position.x + firstX, 0.35f))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                animator.enabled = true;
                animator.SetTrigger("Go");
            })
            .Append(transform.DOMoveX(transform.position.x + x, 0.35f))
            .Append(transform.DOMoveX(transform.position.x, 0.35f))
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

    public void Delete()
    {
        try
        {
            Debug.Log("Delete");

            DOTween.Kill(this);
            sequence = DOTween.Sequence();
            sequence
                .Append(spriteRenderer.DOFade(0, 0.5f))
                .OnComplete(() => Destroy(gameObject));
        }
        catch (Exception e)
        {
        }

    }
}
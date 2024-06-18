using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TigerSlapAtk : MonoBehaviour, IDelete
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
        
        spriteRenderer.flipX = _isRight;
        
        sequence = DOTween.Sequence();
        sequence
            .OnStart(() =>
            {
                animator.enabled = true;
                spriteRenderer.sortingOrder = -11;
            })
            .AppendInterval(0.5f)
            .Append(transform.DOMoveY(transform.position.y - 5f, 0.5f))
            .JoinCallback(() =>
            {
                spriteRenderer.sortingOrder = 20;
            })
            .AppendInterval(0.25f)
            .Append(transform.DOMoveY(transform.position.y, 0.5f))
            .Join(spriteRenderer.DOFade(0,0.5f))
            .OnComplete(()=> Destroy(gameObject));
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         var player = Utils.GetOrAddComponent<Player>(collision.gameObject);
    //         if (player.IsInvincibleTime == false && player.GetIsDeadBool() == false)
    //         {
    //             player.PlayerHit(transform.position, false);
    //         }
    //     }
    // }

    public void Delete()
    {
        Debug.Log("Delete");
        if(spriteRenderer == null) 
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0, 0.5f))
            .OnComplete(() => Destroy(gameObject));
    }
    
    public void CanHit(bool _canHit)
    {
        if (_canHit)
        {
            tag = "NoDeleteEnemyBullet";
            gameObject.layer = 7;
        }
        else
        {
            tag = "Untagged";
            gameObject.layer = 0;
        }
    }
}
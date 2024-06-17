using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TigerDigAtk : MonoBehaviour
{
    Sequence sequence;
    Animator animator;
    Vector3 startPos;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    public void Move(bool _isRight, int _height = 1)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        float firstX = _isRight ? -5f : 5f;
        firstX = transform.position.x + firstX / _height;
        float firstY = transform.position.y - 3 * _height;
        startPos = new Vector3(firstX, firstY, startPos.z);

        float x = _isRight ? -14f : 14f;

        float floatHeight = _height;
        
        if(_height == 2)
            floatHeight = 1.5f;
        
        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOMoveX(firstX, 0.35f))
            .Join(transform.DOMoveY(firstY, 0.35f))
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                animator.enabled = true;
                animator.SetTrigger("Go");
            })
            .Append(transform.DOMoveX(firstX + x / floatHeight, 0.35f))
            .Join(transform.DOMoveY(firstY - 8.5f / floatHeight, 0.35f))
            .Append(transform.DOMove(startPos, 0.35f))
            .Append(spriteRenderer.DOFade(0, 0.35f))
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
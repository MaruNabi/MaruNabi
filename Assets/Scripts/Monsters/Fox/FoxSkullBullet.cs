using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class FoxSkullBullet : MonoBehaviour, IMonsterBullet
{
    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer spriteRenderer;
    private bool isDestroy;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0;
        col.isTrigger = true;
    }
    
    public void Throw()
    {
        rb.gravityScale = 2;
        col.isTrigger = false;
        var random = Random.Range(0, 2);
        var randomPower = 5;
        
        if(random == 0)
            rb.velocity = Vector2.left * randomPower;
        else
            rb.velocity = Vector2.right * randomPower;
    }

    public void DestroyBullet()
    {
        if(this == null)
            return;
        
        gameObject.layer = 0;
        tag = "Untagged";
        isDestroy = true;
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0,0.25f))
            .OnComplete(() => Destroy(gameObject));
    }
}

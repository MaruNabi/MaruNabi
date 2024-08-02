using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FoxBullet : MonoBehaviour
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
        rb.gravityScale = 1;
        col.isTrigger = false;
        var random = Random.Range(0, 2);
        var randomPower = Random.Range(4, 7);
        
        if(random == 0)
            rb.velocity = Vector2.left * randomPower;
        else
            rb.velocity = Vector2.right * randomPower;
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall"))
        {
            if(!isDestroy)
                Destroy(gameObject);
        }
    }
    
    public void DestroyBullet()
    {
        if(this == null)
            return;
        
        isDestroy = true;
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0,0.5f))
            .OnComplete(() => Destroy(gameObject));
    }
}

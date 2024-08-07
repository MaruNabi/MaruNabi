using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSkullBullet : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        rb.gravityScale = 0;
        col.isTrigger = true;
    }
    
    public void Throw()
    {
        rb.gravityScale = 1;
        col.isTrigger = false;
        var random = Random.Range(0, 2);
        var randomPower = 4;
        
        if(random == 0)
            rb.velocity = Vector2.left * randomPower;
        else
            rb.velocity = Vector2.right * randomPower;
    }
}

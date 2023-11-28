using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float distance;
    [SerializeField]
    protected LayerMask isLayer;
    protected Rigidbody2D bulletRigidbody;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected void SetBullet()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        Invoke("DestroyBullet", 2);
    }

    protected void DestroyBullet()
    {
        Destroy(gameObject);
    }

    protected virtual void AttackInstantiate()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);

        Debug.DrawRay(transform.position, transform.right * distance, Color.red);

        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy")
            {
                Debug.Log("Enemy Hit!");

                PlayerNabi.ultimateGauge += 0.3f;
            }
            DestroyBullet();
        }
    }
}

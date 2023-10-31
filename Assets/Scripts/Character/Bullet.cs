using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D bulletRigidbody;
    public float distance;
    public LayerMask isLayer;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        Invoke("DestroyBullet", 2);
    }

    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, transform.right, distance, isLayer);

        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy")
            {
                Debug.Log("Enemy Hit!");
            }
            Debug.Log(ray.collider.tag);
            DestroyBullet();
        }

        Debug.Log(transform.rotation.y);

        if (transform.rotation.y == 0)
        {
            bulletRigidbody.velocity = new Vector2(-speed, 0);
        }

        else
        {
            bulletRigidbody.velocity = new Vector2(speed, 0);
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}

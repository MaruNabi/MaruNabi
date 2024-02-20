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
    public Vector2 lockedBulletVector;
    private float attackPower = 300f;

    void Start()
    {
        
    }

    protected void SetBullet()
    {
        lockedBulletVector = BulletVectorManager.bulletVector;

        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletRigidbody.gravityScale = 0;
        bulletRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
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

                if ((PlayerNabi.ultimateGauge += attackPower) > 1500f)
                {
                    PlayerNabi.ultimateGauge = 1500f;
                }
            }
            DestroyBullet();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float rayDistance;
    [SerializeField]
    protected LayerMask isLayer;
    protected Rigidbody2D bulletRigidbody;
    public Vector2 lockedBulletVector;
    private float attackPower = 300f;

    private RaycastHit2D ray;

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
        this.ray = Physics2D.Raycast(transform.position, transform.right, rayDistance, isLayer);

        Debug.DrawRay(transform.position, transform.right * rayDistance, Color.red);
    }

    protected void NormalBulletMovement()
    {
        if (lockedBulletVector.magnitude == 0)
        {
            if (transform.rotation.y == 0)
            {
                bulletRigidbody.velocity = new Vector2(-speed, 0);
            }

            else
            {
                bulletRigidbody.velocity = new Vector2(speed, 0);
            }
        }

        else
        {
            bulletRigidbody.velocity = lockedBulletVector * speed;
        }
    }

    protected void ColliderCheck(bool isNormalAtk, bool isPenetrate)
    {
        if (this.ray.collider != null)
        {
            if (this.ray.collider.tag == "Enemy" && isNormalAtk)
            {
                if ((PlayerNabi.ultimateGauge += attackPower) > 1500.0f)
                {
                    PlayerNabi.ultimateGauge = 1500.0f;
                }
            }

            if (!isPenetrate)
            {
                DestroyBullet();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float rayDistance;
    [SerializeField]
    protected LayerMask isLayer;
    protected Rigidbody2D bulletRigidbody;
    protected Vector2 lockedBulletVector;
    protected float attackPower = 300f;

    protected string currentHit;
    private bool isHitOnce = true;
    private float angle;
    private bool isOneInit = true;

    [SerializeField]
    private Transform rayStartPosition;

    protected RaycastHit2D ray;

    private IEnumerator bulletDestroyCoroutine;

    BulletVectorManager bulletVec = new BulletVectorManager();

    protected void SetBullet(float bulletHoldingTime = 2.0f)
    {
        if (bulletDestroyCoroutine != null)
            StopCoroutine(bulletDestroyCoroutine);

        if (Input.GetKey(KeyCode.L))
            lockedBulletVector = bulletVec.GetDirectionalInputNabi();

        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletRigidbody.gravityScale = 0.0f;
        bulletRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        bulletDestroyCoroutine = BulletDestroy(bulletHoldingTime);
        StartCoroutine(bulletDestroyCoroutine);
    }

    async UniTaskVoid UniWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        Debug.Log("Disappear");
        DestroyBullet();
    }

    private IEnumerator BulletDestroy(float bulletHoldingTime = 2.0f)
    {
        isOneInit = true;
        yield return new WaitForSeconds(bulletHoldingTime);
        lockedBulletVector = new Vector2(0.0f, 0.0f);
        Managers.Pool.Push(ComponentUtil.GetOrAddComponent<Poolable>(this.gameObject));
    }

    protected void DestroyBullet()
    {
        lockedBulletVector = new Vector2(0.0f, 0.0f);
        Managers.Pool.Push(ComponentUtil.GetOrAddComponent<Poolable>(this.gameObject));
    }

    protected virtual void AttackInstantiate()
    {
        this.ray = Physics2D.Raycast(rayStartPosition.position, transform.right, rayDistance, isLayer);

        Debug.DrawRay(rayStartPosition.position, transform.right * rayDistance, Color.red);
    }

    protected void NormalBulletMovement()
    {
        if (lockedBulletVector.magnitude == 0)
        {
            if (isOneInit)
            {
                isOneInit = false;
                if (transform.rotation.y == 0)
                {
                    angle = 180;
                    bulletRigidbody.velocity = new Vector2(-speed, 0);
                }

                else
                {
                    angle = 0;
                    bulletRigidbody.velocity = new Vector2(speed, 0);
                }
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }

        else
        {
            bulletRigidbody.velocity = lockedBulletVector * speed;
            angle = Mathf.Atan2(lockedBulletVector.y, lockedBulletVector.x) * Mathf.Rad2Deg;
            angle %= 360f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    protected void ColliderCheck(bool isNormalAtk, bool isPenetrate)
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy" && isNormalAtk)
            {
                if ((PlayerNabi.ultimateGauge += attackPower) > 1500.0f)
                {
                    PlayerNabi.ultimateGauge = 1500.0f;
                }
            }

            else if (ray.collider.tag == "Enemy" && !isNormalAtk && isHitOnce)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                Debug.Log("Hit");
            }

            else if (ray.collider.tag == "Enemy" && ray.collider.name != currentHit)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                Debug.Log("Hit");
            }

            if (!isPenetrate)
            {
                DestroyBullet();
            }
        }

        else if (ray.collider == null && !isNormalAtk)
        {
            isHitOnce = true;
        }
    }
}

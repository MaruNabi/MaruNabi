using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Bullet : MonoBehaviour
{
    public static float totalDamage = 0;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float rayDistance;
    [SerializeField]
    protected LayerMask isLayer;
    protected Rigidbody2D bulletRigidbody;
    protected Vector2 lockedBulletVector;
    protected float attackPower;

    protected string currentHit;
    protected bool isEffectOnce = true;
    private bool isHitOnce = true;
    private float angle;
    private bool isOneInit = true;
    private bool isEnemy = false;
    private KeyCode lockKey;

    [SerializeField]
    private Transform rayStartPosition;

    protected RaycastHit2D ray;

    private IEnumerator bulletDestroyCoroutine;

    protected GameObject shootEffect;

    BulletVectorManager bulletVec = new BulletVectorManager();

    protected void SetBullet(float bulletHoldingTime = 2.0f)
    {
        if (bulletDestroyCoroutine != null)
            StopCoroutine(bulletDestroyCoroutine);

        if (KeyData.isNabiPad && !KeyData.isBothPad)
            lockKey = KeyCode.Joystick1Button4;
        else if (KeyData.isBothPad)
            lockKey = KeyCode.Joystick2Button4;
        else if (!KeyData.isNabiPad)
            lockKey = KeyCode.L;

        if (Input.GetKey(lockKey))
            lockedBulletVector = bulletVec.GetDirectionalInputNabi();

        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletRigidbody.gravityScale = 0.0f;
        bulletRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        bulletDestroyCoroutine = BulletDestroy(bulletHoldingTime);
        isEffectOnce = true;
        isEnemy = false;
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
        angle = 0.0f;
        bulletRigidbody.velocity = Vector2.zero;
        Managers.Pool.Push(Utils.GetOrAddComponent<Poolable>(this.gameObject));
    }

    protected void DestroyBullet()
    {
        isOneInit = true;
        lockedBulletVector = new Vector2(0.0f, 0.0f);
        Managers.Pool.Push(Utils.GetOrAddComponent<Poolable>(this.gameObject));
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

    protected void ColliderCheck(bool _isNormalAtk)
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy" || ray.collider.tag == "NoBumpEnemy")
                isEnemy = true;
            else
                isEnemy = false;

            if (isEnemy && _isNormalAtk)
            {
                PlayerNabi.ultimateGauge += attackPower;
                
                if (PlayerNabi.ultimateGauge >= 5000.0f)
                {
                    PlayerNabi.ultimateGauge = 5000.0f;
                }

                totalDamage += attackPower;
                ray.collider.GetComponent<Entity>().OnDamage(attackPower);
                StartCoroutine(bulletDestroyCoroutine);
            }
            else if (isEnemy && !_isNormalAtk && isHitOnce)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                totalDamage += attackPower;
                ray.collider.GetComponent<Entity>().OnDamage(attackPower);
            }
            else if (isEnemy && ray.collider.name != currentHit && !_isNormalAtk && isHitOnce)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                totalDamage += attackPower;
                ray.collider.GetComponent<Entity>().OnDamage(attackPower);
            }
        }

        else if (ray.collider == null && !_isNormalAtk)
        {
            isHitOnce = true;
        }
    }

    protected void PlayShootEffect()
    {
        if (isEffectOnce)
        {
            isEffectOnce = false;
            Instantiate(shootEffect, transform.position, transform.rotation);
        }
    }
}

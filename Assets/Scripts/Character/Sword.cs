using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sword : MonoBehaviour
{
    public static float totalDamage = 0;
    [SerializeField] protected float rayDistance;
    [SerializeField] protected LayerMask isLayer;
    protected Rigidbody2D swordRigidbody;
    protected SpriteRenderer swordSpriteRenderer;
    protected Vector2 lockedSwordVector;
    public float attackPower;

    [SerializeField] private Transform rayStartPosition;

    protected RaycastHit2D ray;
    protected GameObject swordReturnPosition;

    private IEnumerator bulletDestroyCoroutine;

    protected Vector2 swordPosition;
    protected Vector2 targetVec;
    protected Vector2 swordDistance = new Vector2(0, 0);

    protected bool isActive = false;
    protected bool isLoop = true;
    protected bool isHitOnce = true;
    protected string currentHit;

    protected bool isEnemy = false;

    BulletVectorManager bulletVec = new BulletVectorManager();

    protected void SetSword(float bulletHoldingTime = 2.0f)
    {
        if (bulletDestroyCoroutine != null)
            StopCoroutine(bulletDestroyCoroutine);

        if (Input.GetKey(KeyCode.LeftControl))
            lockedSwordVector = bulletVec.GetDirectionalInputMaru();

        swordRigidbody = GetComponent<Rigidbody2D>();
        swordSpriteRenderer = GetComponent<SpriteRenderer>();
        swordRigidbody.gravityScale = 0.0f;
        swordRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        swordReturnPosition = GameObject.Find("MaruBulletPosition");
        bulletDestroyCoroutine = BulletDestroy(bulletHoldingTime);
        isEnemy = false;
        StartCoroutine(bulletDestroyCoroutine);
    }

    protected IEnumerator BulletDestroy(float bulletHoldingTime = 2.0f)
    {
        yield return new WaitForSeconds(bulletHoldingTime);
        lockedSwordVector = new Vector2(0.0f, 0.0f);
        Managers.Pool.Push(Utils.GetOrAddComponent<Poolable>(this.gameObject));
    }

    protected virtual void AttackInstantiate()
    {
        this.ray = Physics2D.Raycast(rayStartPosition.position, transform.right, rayDistance, isLayer);

        Debug.DrawRay(rayStartPosition.position, transform.right * rayDistance, Color.red);
    }

    protected void NormalHit()
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy" || ray.collider.tag == "NoBumpEnemy")
                isEnemy = true;
            else
                isEnemy = false;

            if (isEnemy && isHitOnce)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                PlayerMaru.ultimateGauge += attackPower;

                if (PlayerMaru.ultimateGauge >= 2500.0f)
                    PlayerMaru.ultimateGauge = 2500.0f;

                totalDamage += attackPower;
                ray.collider.GetComponent<Entity>().OnDamage(attackPower);
            }

            else if (isEnemy && ray.collider.name != currentHit)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                PlayerMaru.ultimateGauge += attackPower;

                if (PlayerMaru.ultimateGauge >= 2500.0f)
                    PlayerMaru.ultimateGauge = 2500.0f;

                totalDamage += attackPower;
                ray.collider.GetComponent<Entity>().OnDamage(attackPower);
            }
        }

        else if (ray.collider == null)
        {
            isHitOnce = true;
        }
    }

    protected void SkillHit()
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy" || ray.collider.tag == "NoBumpEnemy")
                isEnemy = true;
            else
                isEnemy = false;

            if (isEnemy && isHitOnce)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                totalDamage += attackPower;
                ray.collider.GetComponent<Entity>().OnDamage(attackPower);
            }

            else if (isEnemy && ray.collider.name != currentHit)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                totalDamage += attackPower;
                ray.collider.GetComponent<Entity>().OnDamage(attackPower);
            }
        }

        else if (ray.collider == null)
        {
            isHitOnce = true;
        }
    }

    protected void SwordFade(float durationTime)
    {
        swordSpriteRenderer.DOFade(0, durationTime);
    }

    protected IEnumerator ReturnSword()
    {
        while (true)
        {
            DOTween.To(() => transform.position, x => transform.position = x, swordReturnPosition.transform.position, 0.3f);
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    protected float rayDistance;
    [SerializeField]
    protected LayerMask isLayer;
    protected Rigidbody2D swordRigidbody;
    protected Vector2 lockedSwordVector;
    protected float attackPower;

    [SerializeField]
    private Transform rayStartPosition;

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

    BulletVectorManager bulletVec = new BulletVectorManager();
    protected void SetSword(float bulletHoldingTime = 2.0f)
    {
        if (bulletDestroyCoroutine != null)
            StopCoroutine(bulletDestroyCoroutine);

        if (Input.GetKey(KeyCode.LeftControl))
            lockedSwordVector = bulletVec.GetDirectionalInputMaru();

        swordRigidbody = GetComponent<Rigidbody2D>();
        swordRigidbody.gravityScale = 0.0f;
        swordRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        swordReturnPosition = GameObject.Find("MaruBulletPosition");
        bulletDestroyCoroutine = BulletDestroy(bulletHoldingTime);
        StartCoroutine(bulletDestroyCoroutine);
    }

    private IEnumerator BulletDestroy(float bulletHoldingTime = 2.0f)
    {
        yield return new WaitForSeconds(bulletHoldingTime);
        lockedSwordVector = new Vector2(0.0f, 0.0f);
        Managers.Pool.Push(ComponentUtil.GetOrAddComponent<Poolable>(this.gameObject));
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
            if (ray.collider.tag == "Enemy" && isHitOnce && ray.collider.name != currentHit)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                if ((PlayerMaru.ultimateGauge += attackPower) > 1500.0f)
                {
                    PlayerMaru.ultimateGauge = 1500.0f;
                }
            }
        }

        else if (ray.collider == null)
        {
            isHitOnce = true;
        }
    }
}

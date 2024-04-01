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

    BulletVectorManager bulletVec = new BulletVectorManager();
    protected void SetBullet(float bulletHoldingTime = 2.0f)
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
}

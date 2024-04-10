using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCurseCharm : Bullet
{
    private Transform enemy;
    private float turnSpeed = 5.0f;
    private Vector3 bulletDirection;

    private bool isInitOnce = true;
    protected bool isHitEnemy = false;

    private float bulletAngle;
    private float currentZ;
    private float newZ;

    [SerializeField]
    private GameObject curseArrow;

    private Animator curseCharmAnimator;

    void OnEnable()
    {
        SetBullet();

        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        }

        curseCharmAnimator = GetComponent<Animator>();
        curseArrow.SetActive(false);
    }

    void Update()
    {
        if (transform.rotation == Quaternion.Euler(0, 0, 0) && isInitOnce && enemy != null) //transform.rotation == Quaternion.Euler(0, 0, 0)
        {
            isInitOnce = false;
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }

        else if (transform.rotation != Quaternion.Euler(0, 0, 0) && isInitOnce && enemy != null)
        {
            isInitOnce = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (!isHitEnemy)
        {
            AttackInstantiate();
        }

        else
        {
            bulletRigidbody.velocity = new Vector2(0, 0);
            //transform.rotation = Quaternion.Euler(0, 180, 0);

            if (bulletDirection.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            curseArrow.SetActive(true);
        }
    }

    private void OnDisable()
    {
        isInitOnce = true;
        enemy = null;
        isHitEnemy = false;
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        if (enemy != null)
        {
            bulletDirection = (enemy.position - transform.position).normalized;

            bulletRigidbody.velocity = transform.right * speed;

            bulletAngle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            currentZ = transform.rotation.eulerAngles.z;
            newZ = Mathf.LerpAngle(currentZ, bulletAngle, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newZ);
        }

        else
        {
            NormalBulletMovement();
        }

        CurseCharmHit();
    }

    private void CurseCharmHit()
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy")
            {
                curseCharmAnimator.SetBool("isHit", true);
                isHitEnemy = true;
            }
        }
    }
}

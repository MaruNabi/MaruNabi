using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharm : Bullet
{
    private Transform enemy;
    float turnSpeed = 5.0f;
    Vector3 bulletDirection;

    private bool isInitOnce = true;

    private float bulletAngle;
    private float currentZ;
    private float newZ;

    void OnEnable()
    {
        SetBullet();

        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        }

        shootEffect = Resources.Load<GameObject>("Prefabs/VFX/Player/15Sprites/Dron");
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

        PlayShootEffect();

        AttackInstantiate();
    }

    private void OnDisable()
    {
        isInitOnce = true;
        enemy = null;
        //attackCharmAnimator.StopPlayback();
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

        ColliderCheck(true, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharm : Bullet
{
    private Transform enemy;
    float turnSpeed = 5.0f;
    Vector3 bulletDirection;

    float angle;
    float currentZ;
    float newZ;

    void Start()
    {
        SetBullet();

        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        }

        if (transform.rotation == Quaternion.Euler(0, 0, 0))
        {
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }
    }

    void Update()
    {
        AttackInstantiate();
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        if (enemy != null)
        {
            bulletDirection = (enemy.position - transform.position).normalized;

            bulletRigidbody.velocity = transform.right * speed;

            angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            currentZ = transform.rotation.eulerAngles.z;
            newZ = Mathf.LerpAngle(currentZ, angle, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newZ);
        }

        else
        {
            NormalBulletMovement();
        }

        ColliderCheck(true, false);
    }
}

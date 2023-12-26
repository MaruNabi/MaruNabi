using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharm : Bullet
{
    public Transform enemy;
    float turn = 5.0f;
    Vector3 bulletDirection;

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
        }

        bulletRigidbody.velocity = transform.right * speed;

        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        float currentZ = transform.rotation.eulerAngles.z;
        float newZ = Mathf.LerpAngle(currentZ, angle, turn * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newZ);
    }
}

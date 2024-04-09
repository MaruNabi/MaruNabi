using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharm : Bullet
{
    private Transform enemy;
    float turnSpeed = 5.0f;
    Vector3 bulletDirection;

    private bool isInitOnce = true;

    private float angle;
    private float currentZ;
    private float newZ;

    private Animator attackCharmAnimator;

    void OnEnable()
    {
        SetBullet();

        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        }

        
        //attackCharmAnimator.StartPlayback();
    }

    void Update()
    {
        if (transform.rotation == Quaternion.Euler(0, 0, 0) && isInitOnce && enemy != null) //transform.rotation == Quaternion.Euler(0, 0, 0)
        {
            isInitOnce = false;
            Debug.Log("Rotate Left");
            transform.rotation = Quaternion.Euler(0, 0, -180);
            Debug.Log(transform.rotation);
        }

        else if (transform.rotation != Quaternion.Euler(0, 0, 0) && isInitOnce && enemy != null)
        {
            isInitOnce = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

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

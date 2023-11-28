using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharm : Bullet
{
    private Transform enemy;

    void Start()
    {
        SetBullet();

        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        bulletRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        AttackInstantiate();
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        //enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        /*if (transform.rotation.y == 0)
        {
            bulletRigidbody.velocity = new Vector3(-speed, 0, 0);
        }

        else
        {
            bulletRigidbody.velocity = new Vector3(speed, 0, 0);
        }*/

        Vector3 direction = (enemy.position - this.transform.position).normalized;

        float vx = direction.x * speed;
        float vy = direction.y * speed;

        bulletRigidbody.velocity = new Vector2(vx, vy);

        //transform.position += direction * speed * Time.deltaTime;
    }
}

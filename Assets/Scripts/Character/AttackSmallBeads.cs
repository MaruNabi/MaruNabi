using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSmallBeads : Bullet
{
    void Start()
    {
        SetBullet();
    }

    void Update()
    {
        AttackInstantiate();
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        //나중에 8방향때문에 override 사용
        if (transform.rotation.y == 0)
        {
            bulletRigidbody.velocity = new Vector2(-speed, 0);
        }

        else
        {
            bulletRigidbody.velocity = new Vector2(speed, 0);
        }
    }
}

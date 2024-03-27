using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSmallBeads : Bullet
{
    void OnEnable()
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

        NormalBulletMovement();

        ColliderCheck(true, false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBigBeads : Bullet
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

        NormalBulletMovement();

        ColliderCheck(false, true);
    }
}

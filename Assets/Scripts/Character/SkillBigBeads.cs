using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBigBeads : Bullet
{
    void OnEnable()
    {
        SetBullet(2.5f);

        isPenetrate = true;
    }

    void Update()
    {
        AttackInstantiate();
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        NormalBulletMovement();

        ColliderCheck(false);
    }
}

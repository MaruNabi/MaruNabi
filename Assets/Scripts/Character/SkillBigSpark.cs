using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBigSpark : Bullet
{
    void OnEnable()
    {
        SetBullet();
    }

    // Update is called once per frame
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

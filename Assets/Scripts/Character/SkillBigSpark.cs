using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBigSpark : Bullet
{
    void OnEnable()
    {
        SetBullet();

        shootEffect = Resources.Load<GameObject>("Prefabs/VFX/Player/15Sprites/Tank");
    }

    void Update()
    {
        PlayShootEffect();

        AttackInstantiate();
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        NormalBulletMovement();

        ColliderCheck(false);
    }
}

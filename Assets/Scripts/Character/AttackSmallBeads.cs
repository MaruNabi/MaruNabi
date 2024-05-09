using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSmallBeads : Bullet
{
    void OnEnable()
    {
        SetBullet();

        isPenetrate = false;

        shootEffect = Resources.Load<GameObject>("Prefabs/VFX/Player/15Sprites/Rikochet");
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

        ColliderCheck(true);
    }
}

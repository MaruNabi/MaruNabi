using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSmallBeads : Bullet
{
    private const float BEADS_ATTACK_POWER = 50.0f; 

    void OnEnable()
    {
        if (PlayerNabi.isNabiTraitActivated)
            attackPower = BEADS_ATTACK_POWER * 1.5f;
        else
            attackPower = BEADS_ATTACK_POWER;

        SetBullet();

        Managers.Sound.PlaySFX("Beads");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBigBeads : Bullet
{
    private const float SBEADS_ATTACK_POWER = 500.0f; //450

    void OnEnable()
    {
        if (PlayerNabi.isNabiTraitActivated)
            attackPower = SBEADS_ATTACK_POWER * 2.3f;
        else
            attackPower = SBEADS_ATTACK_POWER;

        SetBullet(2.5f);

        Managers.Sound.PlaySFX("BBeads");
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

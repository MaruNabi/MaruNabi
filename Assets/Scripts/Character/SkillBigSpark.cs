using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBigSpark : Bullet
{
    private const float SSPARK_ATTACK_POWER = 400.0f;

    void OnEnable()
    {
        if (PlayerNabi.isNabiTraitActivated)
            attackPower = SSPARK_ATTACK_POWER * 1.5f;
        else
            attackPower = SSPARK_ATTACK_POWER;

        SetBullet();

        Managers.Sound.PlaySFX("BSpark");
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

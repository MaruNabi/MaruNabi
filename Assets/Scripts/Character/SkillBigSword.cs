using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBigSword : Sword
{
    private const float SSWORD_ATTACK_POWER = 500.0f;

    private GameObject bigSwordPosition;
    private GameObject playerPosition;

    private Vector3 shortDistance = new Vector3(1, 0, 0);
    private Vector3 bigSwordDistance;

    private float swordAngle;

    private void Start()
    {
        bigSwordPosition = GameObject.Find("MaruBigSwordPosition");
        playerPosition = GameObject.Find("Maru_Test");
    }

    private void OnEnable()
    {
        if (PlayerNabi.isNabiTraitActivated)
            attackPower = SSWORD_ATTACK_POWER * 1.5f;
        else
            attackPower = SSWORD_ATTACK_POWER;

        SetSword();
    }

    void Update()
    {
        if (playerPosition.transform.rotation.y == -1)
        {
            swordAngle = 0;
            bigSwordDistance = bigSwordPosition.transform.position + shortDistance;
        }
            
        else
        {
            swordAngle = 180;
            bigSwordDistance = bigSwordPosition.transform.position - shortDistance;
        }

        transform.position = bigSwordDistance;
        transform.rotation = Quaternion.Euler(0, swordAngle, 0);

        AttackInstantiate();
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        SkillHit();
    }
}

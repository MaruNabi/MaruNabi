using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillBigMace : Sword
{
    private Vector3 spawnPosition;
    private Vector3 maceDistance = new Vector3(2, 10, 0);

    private void OnEnable()
    {
        SetSword(1.5f);

        if (transform.rotation.y == 0)
        {
            spawnPosition.x = swordReturnPosition.transform.position.x + maceDistance.x;
        }

        else
        {
            spawnPosition.x = swordReturnPosition.transform.position.x - maceDistance.x;
        }

        spawnPosition.y = swordReturnPosition.transform.position.y + maceDistance.y;
    }

    void Update()
    {
        AttackInstantiate();
    }

    private void OnDisable()
    {
        
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        StartCoroutine(SkillMaceMovement());
    }

    private IEnumerator SkillMaceMovement()
    {
        transform.position = spawnPosition;

        yield return null;
    }
}

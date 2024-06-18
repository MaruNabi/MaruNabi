using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

public class AttackSmallMaze : Sword
{
    private const float MACE_ATTACK_POWER = 400.0f;

    private const float MAX_DISTANCE = 2.0f;

    private Vector3 lookDir;
    private float angle;
    private Quaternion dRot;

    private Vector3 rotateVec;

    private Vector3[] mazeTarget = new Vector3[3];
    private Vector3[] wayPoints;

    private void OnEnable()
    {
        if (PlayerNabi.isNabiTraitActivated)
            attackPower = MACE_ATTACK_POWER * 2.3f;
        else
            attackPower = MACE_ATTACK_POWER;

        SetSword(1f);

        swordDistance = new Vector2(MAX_DISTANCE, MAX_DISTANCE);
        attackPower = 400.0f;
        wayPoints = new Vector3[3];

        Managers.Sound.PlaySFX("Mace");
    }

    void Update()
    {
        AttackInstantiate();
    }

    private void OnDisable()
    {
        isActive = false;
        currentHit = "";

        for (int i = 0; i < wayPoints.Length; i++)
            mazeTarget[i] = Vector2.zero;

        wayPoints = null;
        swordSpriteRenderer.color = new Color(1, 1, 1, 1);
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        StartCoroutine(NormalMazeMovement());

        lookDir = swordReturnPosition.transform.position - transform.position;
        angle = Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg;
        dRot = Quaternion.AngleAxis((angle - 90), Vector3.back);

        transform.rotation = dRot;

        NormalHit();
    }

    private IEnumerator NormalMazeMovement()
    {
        if (!isActive)
        {
            isActive = true;
            swordPosition = transform.position;

            if (transform.rotation.y == 0) //left
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                mazeTarget[0].x = swordPosition.x - swordDistance.x;
                mazeTarget[1].x = swordPosition.x - Mathf.Sqrt(Mathf.Pow(MAX_DISTANCE, 2) * 2);
                rotateVec = Vector3.back;
            }

            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                mazeTarget[0].x = swordPosition.x + swordDistance.x;
                mazeTarget[1].x = swordPosition.x + Mathf.Sqrt(Mathf.Pow(MAX_DISTANCE, 2) * 2);
                rotateVec = Vector3.forward;
            }

            mazeTarget[0].y = swordPosition.y + swordDistance.y;
            mazeTarget[1].y = swordPosition.y;
            mazeTarget[2].x = mazeTarget[0].x;
            mazeTarget[2].y = swordPosition.y - swordDistance.y;

            if (lockedSwordVector.magnitude != 0)
            {
                if (lockedSwordVector.x != 0 && lockedSwordVector.y != 0)
                    lockedSwordVector.x = 0;

                else if (lockedSwordVector.x == 0)
                    lockedSwordVector.x = -1;

                for (int i = 0; i < mazeTarget.Length; i++)
                {
                    Vector3 rot = Quaternion.AngleAxis((1 - lockedSwordVector.x) * 45, rotateVec * lockedSwordVector.y) * (mazeTarget[i] - swordReturnPosition.transform.position);
                    mazeTarget[i] = rot + swordReturnPosition.transform.position;
                }
            }

            transform.DOMove(mazeTarget[0], 0.15f).SetEase(Ease.InOutBounce);
            yield return new WaitForSeconds(0.15f);

            for (int i = 0; i < 3; i++)
                wayPoints.SetValue(mazeTarget[i], i);

            transform.DOPath(wayPoints, 0.3f, PathType.CatmullRom).SetEase(Ease.InCubic);
            yield return new WaitForSeconds(0.3f);

            transform.DOMove(swordReturnPosition.transform.position, 0.3f).SetEase(Ease.InCirc);
            yield return new WaitForSeconds(0.2f);

            SwordFade(0.2f);
            StartCoroutine("ReturnSword");
            yield return new WaitForSeconds(0.1f);
            StopCoroutine("ReturnSword");
        }
    }
}

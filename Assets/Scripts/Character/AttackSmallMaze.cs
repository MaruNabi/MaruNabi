using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

public class AttackSmallMaze : Sword
{
    private const float MAX_DISTANCE = 2.0f;

    private Vector3 firstTarget;
    private Vector3 secondTarget;
    private Vector3 thirdTarget;

    private Vector3[] wayPoints;

    private void OnEnable()
    {
        SetSword(1f);

        swordDistance = new Vector2(MAX_DISTANCE, MAX_DISTANCE);
        attackPower = 100.0f;
        wayPoints = new Vector3[3];
    }

    void Update()
    {
        AttackInstantiate();
    }

    private void OnDisable()
    {
        isActive = false;
        currentHit = "";

        firstTarget = Vector3.zero;
        secondTarget = Vector3.zero;
        thirdTarget = Vector3.zero;
        wayPoints = null;
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        StartCoroutine(NormalMazeMovement());

        NormalHit();
    }

    private IEnumerator NormalMazeMovement()
    {
        if (!isActive)
        {
            isActive = true;
            swordPosition = transform.position;

            if (lockedSwordVector.magnitude == 0)
            {
                if (transform.rotation.y == 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    firstTarget.x = swordPosition.x - swordDistance.x;
                    secondTarget.x = swordPosition.x - Mathf.Sqrt(Mathf.Pow(MAX_DISTANCE, 2) * 2);
                }
                
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    firstTarget.x = swordPosition.x + swordDistance.x;
                    secondTarget.x = swordPosition.x + Mathf.Sqrt(Mathf.Pow(MAX_DISTANCE, 2) * 2);
                }

                firstTarget.y = swordPosition.y + swordDistance.y;
                secondTarget.y = swordPosition.y;
                thirdTarget.x = firstTarget.x;
                thirdTarget.y = swordPosition.y - swordDistance.y;

                transform.DOMove(firstTarget, 0.15f).SetEase(Ease.InOutBounce);
                yield return new WaitForSeconds(0.15f);

                wayPoints.SetValue(firstTarget, 0);
                wayPoints.SetValue(secondTarget, 1);
                wayPoints.SetValue(thirdTarget, 2);

                transform.DOPath(wayPoints, 0.3f, PathType.CatmullRom).SetEase(Ease.InCubic);
                yield return new WaitForSeconds(0.3f);

                transform.DOMove(swordReturnPosition.transform.position, 0.3f).SetEase(Ease.InCirc);
                yield return new WaitForSeconds(0.2f);

                StartCoroutine("ReturnSword");
                yield return new WaitForSeconds(0.1f);
                StopCoroutine("ReturnSword");
            }
        }
    }
}

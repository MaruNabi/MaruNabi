using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

public class AttackSmallSword : Bullet
{
    private Vector3 bulletPosition;
    private Vector3 targetVec;
    private Vector3 intersection = new Vector3(3.0f, 0, 0);

    private bool isActive = false;

    private bool isLoop = true;

    private void OnEnable()
    {
        SetBullet(1f);
    }

    void Update()
    {
        AttackInstantiate();
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        StartCoroutine(NormalSwordMovement());

    }

    private IEnumerator NormalSwordMovement()
    {
        if (!isActive)
        {
            isActive = true;
            if (lockedBulletVector.magnitude == 0)
            {
                bulletPosition = transform.position;

                if (transform.rotation.y == 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    targetVec = bulletPosition - intersection;
                    //transform.DOMove(bulletPosition, 0.4f).SetEase(Ease.InCubic);
                }

                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    targetVec = bulletPosition + intersection;
                }

                transform.DOMove(targetVec, 0.4f).SetEase(Ease.OutCubic);
                yield return new WaitForSeconds(0.4f);
            }
        }
    }
}

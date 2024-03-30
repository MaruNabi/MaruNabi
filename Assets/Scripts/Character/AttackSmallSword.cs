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
    private Vector3 bulletDistance = new Vector3(4.0f, 0, 0);

    private bool isActive = false;

    private bool isLoop = true;

    private GameObject bulletReturnPosition;

    private void OnEnable()
    {
        SetBullet(0.8f);

        bulletReturnPosition = GameObject.Find("MaruBulletPosition");
    }

    void Update()
    {
        AttackInstantiate();
    }

    private void OnDisable()
    {
        isActive = false;
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
                    targetVec = bulletPosition - bulletDistance;
                    //transform.DOMove(bulletPosition, 0.4f).SetEase(Ease.InCubic);
                }

                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    targetVec = bulletPosition + bulletDistance;
                }
            }

            transform.DOMove(targetVec, 0.4f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(0.4f);

            transform.DOMove(bulletReturnPosition.transform.position, 0.2f).SetEase(Ease.InCirc);
            yield return new WaitForSeconds(0.2f);

            StartCoroutine("ReturnSword");
            yield return new WaitForSeconds(0.3f);
            StopCoroutine("ReturnSword");
        }
    }

    private IEnumerator ReturnSword()
    {
        while (true)
        {
            DOTween.To(() => transform.position, x => transform.position = x, bulletReturnPosition.transform.position, 0.3f);
            yield return null;
        }
    }
}

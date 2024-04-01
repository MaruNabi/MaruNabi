using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

public class AttackSmallSword : Sword
{
    private const float MAX_SWORD_DISTANCE = 4.0f;

    private Vector2 swordPosition;
    private Vector2 targetVec;
    private Vector2 swordDistance = new Vector2(MAX_SWORD_DISTANCE, 0);

    private bool isActive = false;
    private bool isLoop = true;
    private bool isHitOnce = true;

    private GameObject swordReturnPosition;

    private float finalAttackPower;
    private string currentHit;

    private void OnEnable()
    {
        SetBullet(0.8f);

        attackPower = 50.0f;
    }

    void Update()
    {
        AttackInstantiate();
    }

    private void OnDisable()
    {
        isActive = false;
        finalAttackPower = 0.0f;
        currentHit = "";
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        StartCoroutine(NormalSwordMovement());

        SwordHit();
    }

    private IEnumerator NormalSwordMovement()
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
                    targetVec = swordPosition - swordDistance;
                    //transform.DOMove(bulletPosition, 0.4f).SetEase(Ease.InCubic);
                }

                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    targetVec = swordPosition + swordDistance;
                }
            }

            else
            {
                targetVec = swordPosition + (lockedSwordVector * MAX_SWORD_DISTANCE);
                float zAngle = Mathf.Atan2(lockedSwordVector.y, lockedSwordVector.x) * Mathf.Rad2Deg;
                zAngle %= 360f;
                float xAngle = 0.0f;
                if (transform.rotation.y == 0)
                {
                    xAngle = 180f;
                    zAngle *= -1.0f;
                }
                transform.rotation = Quaternion.Euler(xAngle, 0, zAngle);
            }

            Debug.Log(targetVec);

            transform.DOMove(targetVec, 0.4f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(0.4f);

            transform.DOMove(swordReturnPosition.transform.position, 0.2f).SetEase(Ease.InCirc);
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
            DOTween.To(() => transform.position, x => transform.position = x, swordReturnPosition.transform.position, 0.3f);
            yield return null;
        }
    }

    private void SwordHit()
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy" && isHitOnce && ray.collider.name != currentHit)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                finalAttackPower = attackPower * Mathf.Round(MAX_SWORD_DISTANCE - Vector3.Distance(transform.position, swordPosition));
                if ((PlayerMaru.ultimateGauge += finalAttackPower) > 1500.0f)
                {
                    PlayerMaru.ultimateGauge = 1500.0f;
                }
            }
        }

        else if (ray.collider == null)
        {
            isHitOnce = true;
        }
    }
}

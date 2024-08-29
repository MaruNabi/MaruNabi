using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;
using MoreMountains.Feedbacks;

public class AttackSmallSword : Sword
{
    private const float SWORD_ATTACK_POWER = 80.0f; //145

    private const float MAX_DISTANCE = 4.0f;

    private float finalAttackPower;
    private bool canHit;

    [SerializeField]
    private MMF_Player motionBlur;

    private void OnEnable()
    {
        if (PlayerNabi.isNabiTraitActivated)
            attackPower = SWORD_ATTACK_POWER * 1.5f;
        else
            attackPower = SWORD_ATTACK_POWER;

        SetSword(1f);

        swordDistance = new Vector2(MAX_DISTANCE, 0);

        canHit = true;
        Managers.Sound.PlaySFX("Sword");
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
        swordSpriteRenderer.color = new Color(1, 1, 1, 1);
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        motionBlur?.PlayFeedbacks();

        StartCoroutine(NormalSwordMovement());

        if (canHit)
        {
            SwordHit();
        }
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
                if (lockedSwordVector.x != 0 && lockedSwordVector.y != 0)
                    targetVec = swordPosition + (lockedSwordVector * Mathf.Sqrt(Mathf.Pow(MAX_DISTANCE, 2) / 2));
                else
                    targetVec = swordPosition + (lockedSwordVector * MAX_DISTANCE);

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

            transform.DOMove(targetVec, 0.4f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(0.4f);

            canHit = false;

            transform.DOMove(swordReturnPosition.transform.position, 0.2f).SetEase(Ease.InCirc);
            yield return new WaitForSeconds(0.2f);

            SwordFade(0.2f);
            StartCoroutine("ReturnSword");
            yield return new WaitForSeconds(0.3f);
            StopCoroutine("ReturnSword");
        }
    }

    private void SwordHit()
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy" || ray.collider.tag == "NoBumpEnemy")
                isEnemy = true;
            else
                isEnemy = false;

            if (isEnemy && isHitOnce)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                DistancePerDamage();
                PlayerMaru.ultimateGauge += finalAttackPower;

                if (PlayerMaru.ultimateGauge >= 3500.0f)
                    PlayerMaru.ultimateGauge = 3500.0f;

                totalDamage += finalAttackPower;
                ray.collider.GetComponent<Entity>().OnDamage(finalAttackPower);
            }
            else if (ray.collider.name != currentHit && ray.collider.name != "")
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                DistancePerDamage();
                PlayerMaru.ultimateGauge += finalAttackPower;

                if (PlayerMaru.ultimateGauge >= 3500.0f)
                    PlayerMaru.ultimateGauge = 3500.0f;

                totalDamage += finalAttackPower;
                ray.collider.GetComponent<Entity>().OnDamage(finalAttackPower);
            }
        }

        else if (ray.collider == null)
        {
            isHitOnce = true;
        }
    }

    private void DistancePerDamage()
    {
        if (MAX_DISTANCE - Vector3.Distance(transform.position, swordPosition) >= 3.1f)
        {
            finalAttackPower = attackPower * 2.5f;
            Managers.Sound.PlaySFX("Sword2.5");
        }
        else if (MAX_DISTANCE - Vector3.Distance(transform.position, swordPosition) < 2.3f)
        {
            finalAttackPower = attackPower * 1f;
        }
        else
        {
            finalAttackPower = attackPower * 1.5f;
            Managers.Sound.PlaySFX("Sword1.5");
        }
    }
}

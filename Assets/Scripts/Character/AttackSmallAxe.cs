using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

public class AttackSmallAxe : Sword
{
    private const float MAX_DISTANCE = 5.0f;

    [SerializeField]
    private Vector3 rotateSpeed = new Vector3(0, 0, -1920.0f);

    private void OnEnable()
    {
        SetSword(1.6f);

        swordDistance = new Vector2(MAX_DISTANCE, 0);

        attackPower = 100.0f;
    }

    void Update()
    {
        AttackInstantiate();

        transform.Rotate(rotateSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        isActive = false;
        currentHit = "";
        swordSpriteRenderer.color = new Color(1, 1, 1, 1);
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        StartCoroutine(NormalAxeMovement());
    }

    private IEnumerator NormalAxeMovement()
    {
        if (!isActive)
        {
            isActive = true;
            swordPosition = transform.position;
            StartCoroutine("SetActiveRay");

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

            transform.DOMove(targetVec, 0.25f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(0.25f);

            yield return new WaitForSeconds(0.75f);

            transform.DOMove(swordReturnPosition.transform.position, 0.2f).SetEase(Ease.InCirc);
            yield return new WaitForSeconds(0.15f);

            SwordFade(0.3f);
            StartCoroutine("ReturnSword");
            yield return new WaitForSeconds(0.1f);
            StopCoroutine("ReturnSword");
            StopCoroutine("SetActiveRay");
        }
    }

    private IEnumerator SetActiveRay()
    {
        while (true)
        {
            isHitOnce = true;
            AxeHit();
            yield return new WaitForSeconds(0.125f);
        }
    }

    private void AxeHit()
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy" && isHitOnce)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                if ((PlayerMaru.ultimateGauge += attackPower) > 1500.0f)
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

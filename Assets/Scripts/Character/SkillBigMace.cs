using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillBigMace : Sword
{
    private Vector3 spawnPosition;
    private Vector3 maceDistance = new Vector3(2, 10, 0);
    private Vector3 blinkEffectPosition;

    private Color originColor = new Color(1, 1, 1, 1);

    private bool isPositionSet = false;

    [SerializeField]
    private GameObject blinkEffect;
    [SerializeField]
    private SpriteRenderer blinkSpriteRenderer;

    private void OnEnable()
    {
        SetSword(2f);
    }

    void Update()
    {
        AttackInstantiate();

        if (isPositionSet)
            BlinkEffectPos();
    }

    private void OnDisable()
    {
        isActive = false;
        isPositionSet = false;
        blinkEffect.SetActive(true);
        currentHit = "";
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        StartCoroutine(SkillMaceMovement());

        BigMaceHit();
    }

    private IEnumerator SkillMaceMovement()
    {
        if (!isActive)
        {
            isActive = true;

            swordSpriteRenderer.color = new Color(1, 1, 1, 0);

            if (transform.rotation.y == 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                spawnPosition.x = swordReturnPosition.transform.position.x - maceDistance.x;
            }

            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                spawnPosition.x = swordReturnPosition.transform.position.x + maceDistance.x;
            }

            spawnPosition.y = swordReturnPosition.transform.position.y + maceDistance.y;
            targetVec.x = spawnPosition.x;
            targetVec.y = swordReturnPosition.transform.position.y - maceDistance.y;
            blinkEffectPosition.x = spawnPosition.x;
            blinkEffectPosition.y = swordReturnPosition.transform.position.y + 4.0f;

            isPositionSet = true;

            transform.position = spawnPosition;
            swordSpriteRenderer.color = originColor;

            StartCoroutine(BlinkEffect(1f, blinkSpriteRenderer));
            yield return new WaitForSeconds(1f);
            blinkEffect.SetActive(false);

            transform.DOMove(targetVec, 1f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ground Hit");
            StartCoroutine(BulletDestroy(0.0f));
        }
    }

    private void BlinkEffectPos()
    {
        blinkEffect.transform.position = blinkEffectPosition;
    }

    private IEnumerator BlinkEffect(float blinkTime, SpriteRenderer spriteRenderer)
    {
        float remainingTime = 0.0f;
        float startTime = Time.time;

        while (remainingTime < blinkTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.3f);
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = new Color(1, 1, 1, 0.6f);
            yield return new WaitForSeconds(0.15f);
            remainingTime = Time.time - startTime;
        }
    }

    private void BigMaceHit()
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy" && isHitOnce)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                Debug.Log("Hit");
            }

            else if (ray.collider.name != currentHit)
            {
                isHitOnce = false;
                currentHit = ray.collider.name;
                Debug.Log("Hit");
            }
        }

        else if (ray.collider == null)
        {
            isHitOnce = true;
        }
    }
}

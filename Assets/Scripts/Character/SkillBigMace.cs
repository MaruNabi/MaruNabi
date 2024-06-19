using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillBigMace : Sword
{
    private const float SMACE_ATTACK_POWER = 1500.0f; //1100

    private Vector3 spawnPosition;
    private Vector3 maceDistance = new Vector3(2, 10, 0);
    private Vector3 blinkEffectPosition;

    private Color originColor = new Color(1, 1, 1, 1);

    private bool isPositionSet = false;

    [SerializeField] private GameObject blinkEffect;
    [SerializeField] private SpriteRenderer blinkSpriteRenderer;

    private GameObject explosionEffect;

    private void OnEnable()
    {
        if (PlayerNabi.isNabiTraitActivated)
            attackPower = SMACE_ATTACK_POWER * 2.3f;
        else
            attackPower = SMACE_ATTACK_POWER;

        SetSword(2f);

        explosionEffect = Resources.Load<GameObject>("Prefabs/VFX/Player/15Sprites/MaceExploEff");
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

        SkillHit();
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

            transform.DOMove(targetVec, 0.75f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.75f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ground Hit");
            Instantiate(explosionEffect, transform.position, transform.rotation);
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
}

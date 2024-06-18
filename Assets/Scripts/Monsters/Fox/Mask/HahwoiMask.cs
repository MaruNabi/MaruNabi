using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class HahwoiMask : Entity
{
    private Fox fox;
    private Vector3[] teleportPoints;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sequence sequence;
    private DetectionArea detectionArea;
    private bool isStart;

    private float time;
    private float disappearTime;
    private float speed = 2f;
    private Transform player;

    private void Update()
    {
        if (isStart)
        {
            if(detectionArea.Players.Count > 0 && player == null)
                player = detectionArea.Players[0].transform;
            
            time += Time.deltaTime;
            
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;
            
            if (time >= disappearTime)
            {
                OnDisappear();
                time = 0;
                isStart = false;
            }
        }
    }
    
    protected override void Init()
    {
        HP = Utils.GetDictValue(Managers.Data.monsterDict, "HAHWOI_MONSTER").LIFE;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SpawnAnimation();
        
        var players = GameObject.FindGameObjectsWithTag("Player");
        player = players[Random.Range(0, players.Length)].transform;
        detectionArea = Utils.FindChild<DetectionArea>(gameObject);
    }
    
    public override void OnDamage(float _damage)
    {
        BeHitEffect();
        HP -= _damage;
    
        if (HP <= 0)
        {
            HP = 0;
            OnDead();
        }
    }

    public void SetVariables(Fox _fox, float _time = 15f)
    {
        fox = _fox;
        disappearTime = _time;
        time = 0;
    }
    
    private void BeHitEffect()
    {
        var hitSequence = DOTween.Sequence();
        hitSequence
            .Append(spriteRenderer.DOFade(0.75f, 0.3f))
            .Append(spriteRenderer.DOFade(1f, 0.3f));
    }

    private void SpawnAnimation()
    {
        animator.enabled = true;
        Managers.Sound.PlaySFX("Mask_Normal");
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(-42f, 0.5f))
            .Join(spriteRenderer.DOFade(1, 0.5f))
            .AppendInterval(0.5f)
            .OnComplete(() =>
            {
                isStart = true;
                tag = "Enemy";
                gameObject.layer = 7;
            });
    }

    private void OnDead()
    {
        sequence.Kill();
        tag = "Untagged";
        gameObject.layer = 0;
        animator.SetTrigger("Die");
        Managers.Sound.PlaySFX("Mask_Death");
        sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0, 1f))
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                // 여우에게 죽음 알리기
                fox.HahwoiDeathCountUp();
                Destroy(gameObject);
            });
    }
    
    private void OnDisappear()
    {
        sequence.Kill();
        tag = "Untagged";
        gameObject.layer = 0;
        animator.SetTrigger("Die");
        sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0, 1f))
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                // 여우에게 사라짐 알리기
                fox.HahwoiDisapCountUp();
                Destroy(gameObject);
            });
    }
}
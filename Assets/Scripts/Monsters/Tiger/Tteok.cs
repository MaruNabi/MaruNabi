using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tteok : Entity, IDelete
{
    [SerializeField] GameObject redBean;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sequence sequence;
    private Sequence hitSequence;
    private Sequence beanSequence;

    private float time;
    private float attackTime;
    private bool isStart;
    private bool isRight;
    private List<GameObject> beans;

    protected override void Init()
    {
        HP = Utils.GetDictValue(Managers.Data.monsterDict, "TTEOK_MONSTER").LIFE;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        time = 0;
        attackTime = 2f;
        isStart = true;
        beans = new List<GameObject>();
    }

    private void Update()
    {
        if (isStart)
        {
            time += Time.deltaTime;

            if (time >= attackTime)
            {
                BeanAttack();
                time = 0;
            }
        }
    }

    public void SetVariables(Tiger _tiger, bool _set)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = _set;

        Move();
    }

    private void BeanAttack()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        isRight = spriteRenderer.flipX;
        float x = isRight ? 32f : -32f;

        animator.SetTrigger("Attack");
        Managers.Sound.PlaySFX("Tteok_Shot");

        var bean = Instantiate(redBean, transform.position, Quaternion.identity);
        beans.Add(bean);

        if (beanSequence != null && beanSequence.IsActive())
            beanSequence.Kill();
        beanSequence = DOTween.Sequence();
        beanSequence
            .Append(bean.transform.DOMoveX(bean.transform.position.x + x, 2f).SetEase(Ease.Linear))
            .OnComplete(() => Destroy(bean));
    }

    private void BeHitEffect()
    {
        if (hitSequence != null && hitSequence.IsActive())
            hitSequence.Kill();

        if (spriteRenderer == null)
            return;

        DOTween.Kill(spriteRenderer);

        hitSequence = DOTween.Sequence();
        hitSequence
            .Append(spriteRenderer.DOFade(0.5f, 0.3f))
            .Append(spriteRenderer.DOFade(1f, 0.3f));
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

    public void Move()
    {
        Managers.Sound.PlaySFX("Tteok_Flying");

        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOMoveY(-50f, 40f));
    }

    public void Delete()
    {
        OnDead();
    }

    public override void OnDead()
    {
        try
        {
            if (spriteRenderer != null)
            {
                // spriteRenderer와 관련된 모든 트윈 중단
                DOTween.Kill(spriteRenderer);
                spriteRenderer.DOFade(0, 0.5f).onComplete = () =>
                {
                    if (gameObject != null && spriteRenderer != null) // 객체가 파괴되었는지 확인
                    {
                        Destroy(gameObject);
                    }
                };
            }
            else
            {
                Destroy(gameObject);
            }
        }
        catch
        {
        }
    }
}

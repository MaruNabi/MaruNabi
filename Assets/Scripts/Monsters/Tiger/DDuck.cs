using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DDuck : Entity, IDelete
{
    [SerializeField] GameObject redBean;

    private Tiger tiger;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sequence sequence;

    private float time;
    private float attackTime;
    private bool isStart;
    private bool isRight;
    
    protected override void Init()
    {
        HP = 500;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        time = 0;
        attackTime = 2f;
        isStart = true;
        Move();
        // ¾Æ·¡
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
    
    public void Flip(bool _set)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        spriteRenderer.flipX = _set;
    }

    private void BeanAttack()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        isRight = spriteRenderer.flipX;
        float x = isRight ? 8f : -8f;
        
        animator.SetTrigger("Attack");
        var bean = Instantiate(redBean, transform.position + Vector3.down, Quaternion.identity);
        sequence = DOTween.Sequence();
        sequence
            .Append(bean.transform.DOMoveX(bean.transform.position.x + x, 1f))
            .OnComplete(()=> Destroy(bean));
    }

    private void BeHitEffect()
    {
        var hitSequence = DOTween.Sequence();
        hitSequence
            .Append(spriteRenderer.DOFade(0.75f, 0.3f))
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
        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOMoveY(-50f, 40f))
            .OnComplete(() => isStart = true);
    }

    public void Delete()
    {
        if(spriteRenderer == null) 
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        sequence.Kill();
        tag = "Untagged";
        gameObject.layer = 0;
        sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0, 0.5f))
            .OnComplete(() => Destroy(gameObject));
    }
    
    public override void OnDead()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        sequence.Kill();
        tag = "Untagged";
        gameObject.layer = 0;
        sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0, 0.5f))
            .OnComplete(() => Destroy(gameObject));
    }
}
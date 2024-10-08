using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class MouseScholar : Entity
{
    public static Action<GameObject> StageClear;
    public static Action<float> OnRoundEnd;
    public static Action PunishProduction;
    
    private MouseScholarStateMachine stateMachine;
    private SpriteRenderer spriteRenderer;
    private ScholarEffects mouseEffects;
    private Sequence sequence;
    private GameObject mouseGO;
    private GameObject scholarGO;
    private Animator animator;

    private const float DAMAGE_VALUE = 200;
    private bool isHit;
    public bool IsHit => isHit;
    
    private bool isIdle;
    private float strawOparcity;

    public bool IsIdle
    {
        get => isIdle;
        set => isIdle = value;
    }

    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "MOUSESCHOLAR_MONSTER");
        stateMachine = Utils.GetOrAddComponent<MouseScholarStateMachine>(gameObject);
        spriteRenderer = GetComponent<SpriteRenderer>();
        mouseEffects = GetComponent<ScholarEffects>();
        entityCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        stateMachine.Initialize("Appearance", this, GetComponent<Animator>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (IsIdle && Dead == false)
            {
                isHit = true;
                BeHitEffect();

                if (HP - DAMAGE_VALUE > 0)
                {
                    OnDamage(DAMAGE_VALUE);
                }
                else
                {
                    HP = 0;
                }
            }
        }
    }
    
    public override void OnDamage(float _damage)
    {
        isHit = true;
        BeHitEffect();
        HP -= _damage;
    
        if (HP <= 0)
        {
            HP = 0;
            StageClearProduction();
        }
    }
    
    public void RoundSetting(float _hp, float _oparcity)
    {
        HP = _hp;
        strawOparcity = _oparcity;
    }

    public void AppearanceEffect()
    {
        GameObject smoke = Instantiate(mouseEffects.smokePrefab);
        smoke.transform.position = transform.position;

        Managers.Sound.PlaySFX("Scholar_Enter");
        
        GameObject straw = Instantiate(mouseEffects.strawParticle);
        straw.GetComponent<StrawOparcity>().SetOparcity(strawOparcity);
        straw.transform.position = transform.position;

        sequence = DOTween.Sequence();
        sequence.OnStart(() => spriteRenderer.color = new Color(1, 1, 1, 0))
            .Append(spriteRenderer.DOFade(1f, 1f));
    }

    public void SmokeEffect()
    {
        GameObject smoke = Instantiate(mouseEffects.smokePrefab);
        smoke.transform.position = transform.position;
        spriteRenderer.DOFade(0, 0.4f);
    }

    private void BeHitEffect()
    {
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0.5f, 0.3f))
            .Append(spriteRenderer.DOFade(1f, 0.3f));
    }

    public GameObject MakeFan(Vector3 fanPosition)
    {
        GameObject fan = Instantiate(mouseEffects.fanPrefab);
        fan.transform.position = fanPosition;
        return fan;
    }

    public void DestroyFan(GameObject fan)
    {
        Destroy(fan);
    }

    public override void OnDead()
    {
        DOTween.Kill(spriteRenderer);
        DOTween.Kill(this);
        base.OnDead();
    }

    public void RoundEnd()
    {
        isHit = false;
        OnRoundEnd?.Invoke(HP);
    }

    public void StageClearProduction()
    {
        tag = "Untagged";
        gameObject.layer = 0;
        
        Dead = true;
        stateMachine.SetState("Leave");
        stateMachine.ChangeAnimation(EScholarAnimationType.Hit);
        Managers.Sound.PlaySFX("Scholar_Angry");
        StageClear?.Invoke(gameObject);
        GameObject smoke = null;


        DOTween.Sequence()
            .AppendInterval(stateMachine.GetAnimPlayTime())
            .AppendCallback(() =>
            {
                Managers.Sound.PlaySFX("Mouse_ComeOut");
                smoke = Instantiate(mouseEffects.smokePrefab);
                smoke.transform.position = transform.position;
                animator.runtimeAnimatorController = mouseEffects.mouseAnimator;
                spriteRenderer.sortingOrder = 1;
            })
            .Join(transform.DOMoveY(transform.position.y-0.25f,0f));
    }

    public void JumpAnimation()
    {
        animator.SetTrigger("Dead");
        Managers.Sound.PlaySFX("Scholar_Exit");
        Managers.Sound.PlaySFX("Hit");

        GameObject smoke = null;
        DOTween.Sequence()
            .Append(gameObject.transform.DOJump(transform.position, 2, 1, 0.5f))
            .AppendInterval(2f)
            .Append(spriteRenderer.DOFade(0, 1f))
            .JoinCallback(() =>
            {
                Managers.Sound.PlaySFX("Scholar_Exit");
                smoke = Instantiate(mouseEffects.smokePrefab);
                smoke.transform.position = transform.position;
            })
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
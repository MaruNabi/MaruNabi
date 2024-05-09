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

    //private TMP_Text hpText;
    private Sequence sequence;
    private GameObject mouseGO;
    private GameObject scholarGO;
    private Animator animator;

    private const float DAMAGE_VALUE = 200;
    private bool isHit;

    public bool IsHit
    {
        get => isHit;
        set => isHit = value;
    }

    private bool isIdle;

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
                    //hpText.text = HP.ToString();
                }
                else
                {
                    HP = 0;
                    //hpText.text = HP.ToString();
                    StageClearProduction();
                }
            }
        }
    }
    
    public void InitHp(float _hp)
    {
        //hpText = Utils.FindChild<TMP_Text>(gameObject, "", true);
        HP = _hp;
        //hpText.text = HP.ToString();
    }

    public void AppearanceEffect()
    {
        GameObject smoke = Instantiate(mouseEffects.smokePrefab);
        smoke.transform.position = transform.position;

        GameObject straw = Instantiate(mouseEffects.strawParticle);
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
        //hpText.DOFade(0, 0.4f);
    }

    private void BeHitEffect()
    {
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0.75f, 0.3f))
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
        DOTween.KillAll(spriteRenderer);
        DOTween.KillAll(this);
        base.OnDead();
    }

    public void RoundEnd()
    {
        isHit = false;
        OnRoundEnd?.Invoke(HP);
    }

    public void StageClearProduction()
    {
        Dead = true;
        stateMachine.SetState("Leave");
        stateMachine.ChangeAnimation(EScholarAnimationType.Hit);
        StageClear?.Invoke(gameObject);
        GameObject smoke = null;

        DOTween.Sequence()
            .AppendInterval(stateMachine.GetAnimPlayTime())
            .AppendCallback(() =>
            {
                smoke = Instantiate(mouseEffects.smokePrefab);
                smoke.transform.position = transform.position;

                animator.runtimeAnimatorController = mouseEffects.mouseAnimator;
            });
    }

    public void JumpAnimation(Vector3 targetPos)
    {
        animator.SetTrigger("Dead");

        GameObject smoke = null;
        DOTween.Sequence()
            .Append(gameObject.transform.DOJump(targetPos, 2, 0, 0.6f))
            .AppendInterval(2f)
            .Append(spriteRenderer.DOFade(0, 1f))
            .JoinCallback(() =>
            {
                smoke = Instantiate(mouseEffects.smokePrefab);
                smoke.transform.position = transform.position;
            })
            .OnComplete(() => Destroy(gameObject));
    }
}
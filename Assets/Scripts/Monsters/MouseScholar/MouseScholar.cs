using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UniRx.Triggers;
using UnityEditor.SceneManagement;

public class MouseScholar : Entity
{
    public static Action<GameObject> StageClear;
    public static Action Punish;

    public static Action OnRoundEnd;

    private MouseScholarStateMachine stateMachine;
    private SpriteRenderer spriteRenderer;
    private ScholarEffects mouseEffects;
    //private TMP_Text hpText;
    private Sequence sequence;
    private GameObject mouseGO;
    private GameObject scholarGO;
    private Animator animator;

    private const float DAMAGE_VALUE = 13000;
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
        //hpText = Utils.FindChild<TMP_Text>(gameObject, "", true);
        stateMachine = Utils.GetOrAddComponent<MouseScholarStateMachine>(gameObject);

        spriteRenderer = GetComponent<SpriteRenderer>();
        mouseEffects = GetComponent<ScholarEffects>();
        entityCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        stateMachine.Initialize("Appearance", this, GetComponent<Animator>());
        maxHP = Data.LIFE;
        HP = maxHP;
        //hpText.text = HP.ToString();
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

    public void AppearanceEffect()
    {
        GameObject smoke = Instantiate(mouseEffects.smokePrefab);
        smoke.transform.position = transform.position;

        GameObject straw = Instantiate(mouseEffects.strawPrefab);
        straw.transform.position = transform.position;

        float timer = 0;
        DOTween.To(() => timer, x => timer = x, 1f, 0.5f)
            .OnComplete(() =>
            {
                Destroy(smoke);
                Destroy(straw);
            });

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
        DOTween.KillAll(spriteRenderer);
        DOTween.KillAll(this);
        base.OnDead();
    }

    public void RoundEnd()
    {
        isHit = false;
        OnRoundEnd?.Invoke();
    }

    public void StageClearProduction()
    {
        Dead = true;
        stateMachine.SetState("Leave");
        stateMachine.ChangeAnimation(EAnimationType.Hit);
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
            .Append(gameObject.transform.DOJump(targetPos, 3, 0, 0.6f))
            .AppendInterval(1f)
            .Append(spriteRenderer.DOFade(0, 1f))
            .JoinCallback(() =>
            {
                smoke = Instantiate(mouseEffects.smokePrefab);
                smoke.transform.position = transform.position;
            });
    }

    public void StageSkip()
    {
        HP = 0;
        //hpText.text = HP.ToString();
    }
}
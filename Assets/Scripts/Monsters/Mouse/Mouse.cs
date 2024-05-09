using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Mouse : Entity
{
    public static Action<bool> MovingBackGround; 
    
    private MouseEffects mouseEffects;
    private MouseStateMachine mouseStateMachine;
    private SpriteRenderer mouseSpriteRenderer;
    private BoxCollider2D headCollider;
    private Animator mouseAnimator;
    private Sequence sequence;
    private TMP_Text hpText;
    private Vector3 startPos;

    private Dictionary<EMousePattern, int> behaviorGacha;
    public Dictionary<EMousePattern, int> BehaviorGacha => behaviorGacha;

    private bool canHit;
    private bool phaseChange;
    private bool rushEvent;
    private bool tailEvent;

    public bool PhaseChange
    {
        get => phaseChange;
        set => phaseChange = value;
    }
    
    private const int DAMAGE_VALUE = 200;

    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "MOUSE_MONSTER");
        mouseStateMachine = Utils.GetOrAddComponent<MouseStateMachine>(gameObject);
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();
        mouseEffects = GetComponent<MouseEffects>();
        hpText = Utils.FindChild<TMP_Text>(gameObject);
        
        behaviorGacha = new Dictionary<EMousePattern, int>();
        sequence = DOTween.Sequence();

        startPos = transform.position;
        maxHP = Data.LIFE;
        HP = maxHP;
        hpText.text = HP.ToString();

        behaviorGacha.Add(EMousePattern.Rush, 10);
        behaviorGacha.Add(EMousePattern.SpawnRats, 10);
        behaviorGacha.Add(EMousePattern.Rock, 10);
        behaviorGacha.Add(EMousePattern.Tail, 70);

        mouseStateMachine.Initialize("Enter", this, GetComponent<Animator>());
    }

    public bool CheckPhaseChangeHp()
    {
        return HP <= maxHP/2;
    }

    public bool CheckDead()
    {
        return HP <= 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 머리에만 맞을 수 있게 콜라이더 영역 설정
        if (collision.CompareTag("Bullet"))
        {
            if (canHit)
            {
                HitEffect();
                OnDamage(DAMAGE_VALUE);
                hpText.text = HP.ToString();
            }
        }
    }

    private void HitEffect()
    {
        if (sequence.IsPlaying())
            return;

        sequence = DOTween.Sequence();
        sequence
            .Append(mouseSpriteRenderer.DOFade(0.25f, 0.25f))
            .Append(mouseSpriteRenderer.DOFade(1f, 0.25f));
    }

    public float Rush()
    {
        if (sequence.IsPlaying())
            return 0;
        
        sequence = DOTween.Sequence();
        sequence
            .OnStart(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.Rush);
            })
            .AppendInterval(5f)
            .OnComplete(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.NoRush);
            });

        return sequence.Duration();
    }

    public void RushClipEvent()
    {
        if (rushEvent == false)
        {
            transform.DOMove(transform.position - new Vector3(14f, 1.5f, 0), 1f).SetEase(Ease.InCubic);
        }
        else
        {
            transform.DOMove(startPos, 1f).SetEase(Ease.InCubic);
        }
        
        mouseSpriteRenderer.flipX = rushEvent;
        AllowAttack(!rushEvent);
        rushEvent = !rushEvent;
    }

    public float SpawnRats()
    {
        if (sequence.IsPlaying())
            return 0;

        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                var spawnPos = transform.position + Vector3.right * 5f + Vector3.down * 1f;
                Instantiate(mouseEffects.rats, spawnPos, Quaternion.Euler(0, 0, 6.6f));
            })
            .AppendInterval(3f);

        return sequence.Duration();
    }

    public float SpawnRock()
    {
        if (sequence.IsPlaying())
            return 0;

        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOShakeRotation(1f))
            .AppendCallback(() =>
            {
                var rockSpawnPoint = transform.position + Vector3.up * 10f;
                Instantiate(mouseEffects.rock, rockSpawnPoint, Quaternion.identity);
            })
            .AppendInterval(3f);

        return sequence.Duration();
    }

    public float TailAttack()
    {
        if (sequence.IsPlaying())
            return 0;

        sequence = DOTween.Sequence();
        sequence
            .OnStart(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.Tail);
            })
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                var tailSpawnPoint = transform.position + Vector3.left * 5f + Vector3.down * 0.5f;
                Instantiate(mouseEffects.tail, tailSpawnPoint, Quaternion.Euler(0, 0, 6.6f));
            })
            .AppendInterval(5f);
        
        // 꼬리 공격
        return sequence.Duration();
    }

    public void TailClipEvent()
    {
        AllowAttack(!rushEvent);
        tailEvent = !tailEvent;
    }

    public EMousePattern TakeOne()
    {
        return RandomizerUtil.From(behaviorGacha).TakeOne();
    }

    public void PhaseChangeSprite()
    {
        if (sequence.IsPlaying())
            sequence.Kill();

        var runSprite = mouseSpriteRenderer.sprite;
        // Animator Phase2로 여기서 변경

        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(2f)
            .Append(transform.DOMove(startPos, 1f))
            .OnComplete(() =>
            {
                mouseSpriteRenderer.flipX = false;
                mouseSpriteRenderer.sprite = runSprite;
            });
    }

    public override void OnDead()
    {
        if (sequence.IsPlaying())
            sequence.Kill();

        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(2f)
            .Append(transform.DOMove(startPos, 1f))
            .AppendCallback(() => { 
                Debug.Log("죽음");
                //스테이지 클리어 여기서 죽음 애니메이션
                })
            .AppendInterval(2f)
            .OnComplete(() => { Destroy(gameObject); });
    }

    public void AllowAttack(bool _canHit)
    {
        if (_canHit)
        {
            canHit = _canHit;
            tag = "Enemy";
            gameObject.layer = 7;
        }
        else
        {
            canHit = _canHit;
            tag = "Untagged";
            gameObject.layer = 0;
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Mouse : Entity
{
    public static Action<bool> MovingBackGround; 
    public static Action<GameObject> StageClear; 
    
    private MouseEffects mouseEffects;
    private MouseStateMachine mouseStateMachine;
    private SpriteRenderer mouseSpriteRenderer;
    private BoxCollider2D headCollider;
    private Animator mouseAnimator;
    private Sequence sequence;
    private Vector3 startPos;

    private Dictionary<EMousePattern, int> behaviorGacha;
    public Dictionary<EMousePattern, int> BehaviorGacha => behaviorGacha;

    private bool canHit;
    private bool phaseChange;
    private bool rushEvent;
    private bool tailEvent;
    public bool isStart;

    public bool PhaseChange
    {
        get => phaseChange;
        set => phaseChange = value;
    }
    
    private const int DAMAGE_VALUE = 400;

    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "MOUSE_MONSTER");
        mouseStateMachine = Utils.GetOrAddComponent<MouseStateMachine>(gameObject);
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();
        mouseEffects = GetComponent<MouseEffects>();
        
        behaviorGacha = new Dictionary<EMousePattern, int>();
        sequence = DOTween.Sequence();

        startPos = transform.position;
        maxHP = Data.LIFE;
        HP = maxHP;

        behaviorGacha.Add(EMousePattern.Rush, 35);
        behaviorGacha.Add(EMousePattern.SpawnRats, 15);
        behaviorGacha.Add(EMousePattern.Rock, 25);
        behaviorGacha.Add(EMousePattern.Tail, 25);

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
        if (collision.CompareTag("Bullet"))
        {
            if (canHit)
            {
                BeHitEffect();
                OnDamage(DAMAGE_VALUE);
            }
        }
    }

    private void BeHitEffect()
    {
        sequence = DOTween.Sequence();
        sequence
            .Append(mouseSpriteRenderer.DOFade(0.75f, 0.3f))
            .Append(mouseSpriteRenderer.DOFade(1f, 0.3f));
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

    public void TurnClipEvent()
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
            .AppendInterval(1.225f)
            .AppendCallback(() =>
            {
                var tailSpawnPoint = transform.position + Vector3.left * 4.5f + Vector3.down * 1.7f;
                Instantiate(mouseEffects.tail, tailSpawnPoint, Quaternion.Euler(0, 0, 6.6f));
            })
            .AppendInterval(0.25f);
        
        // 꼬리 공격
        return sequence.Duration();
    }

    public void TailClipEvent()
    {
        AllowAttack(!tailEvent);
        tailEvent = !tailEvent;
    }

    public EMousePattern TakeOne()
    {
        return RandomizerUtil.From(behaviorGacha).TakeOne();
    }

    public void PhaseChangeSequence()
    {
        transform.DOKill();
        
        if (sequence.IsPlaying())
            sequence.Kill();

        rushEvent = false;
        tailEvent = false;
        AllowAttack(false);
        MovingBackGround?.Invoke(false);
        mouseStateMachine.ChangeAnimation(EMouseAnimationType.Dead);
        mouseSpriteRenderer.color = new Color(1, 0.5f, 0.5f, 1f);
        tag = "Untagged";
        // Animator Phase2 변경
        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.Run);
                if (mouseSpriteRenderer.flipX == false)
                    mouseSpriteRenderer.flipX = true;
            })
            .Append(transform.DOMove(startPos, 1f));
    }
    
    public void SmokeEffect()
    {
        GameObject smoke = Instantiate(mouseEffects.smokePrefab);
        smoke.transform.position = transform.position;
        mouseSpriteRenderer.DOFade(0.5f, 0.4f);
    }

    public override void OnDead()
    {
        transform.DOKill();
        if (sequence.IsPlaying())
            sequence.Kill();

        AllowAttack(false);
        tag= "Untagged";
        StageClear?.Invoke(gameObject);
        
        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOMove(startPos, 1f))
            .AppendInterval(2f)
            .AppendCallback(() => mouseStateMachine.ChangeAnimation(EMouseAnimationType.Dead))
            .AppendInterval(1f)
            .AppendCallback(() => { 
                
                SmokeEffect();
                //스테이지 클리어 여기서 죽음 애니메이션
                })
            .AppendInterval(2f)
            .OnComplete(() =>
            {
                Debug.Log("죽음"); 
                StageClear?.Invoke(gameObject);
                tag= "Untagged";
            });
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
            tag = "NoDeleteEnemyBullet";
            gameObject.layer = 0;
        }
    }
}
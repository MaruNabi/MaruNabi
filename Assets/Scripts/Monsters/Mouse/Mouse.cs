using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class Mouse : Entity
{
    public static Action<bool> MovingBackGround;
    public static Action Phase2;
    public static Action<GameObject> StageClear;

    private MouseEffects mouseEffects;
    private MouseStateMachine mouseStateMachine;
    private SpriteRenderer mouseSpriteRenderer;
    private BoxCollider2D headCollider;
    private Animator mouseAnimator;
    private Sequence sequence;
    private Vector3 startPos;
    private CancellationTokenSource cts;

    private Dictionary<EMousePattern, int> behaviorGacha;
    public Dictionary<EMousePattern, int> BehaviorGacha => behaviorGacha;

    private bool canHit;
    private bool phaseChange;
    private bool rushEvent;
    private bool tailEvent;
    private bool isPhaseChanging;
    private bool stageStart;

    public bool StageStart
    {
        set => stageStart = value;
    }

    [SerializeField] private RuntimeAnimatorController phase2Animator;
    [SerializeField] private GameObject runEffect;

    private float patternPercent;

    public float PatternPercent
    {
        get => patternPercent;
        set => patternPercent = value;
    }

    public bool PhaseChange => phaseChange;

    private const int DAMAGE_VALUE = 400;

    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "MOUSE_MONSTER");
        mouseStateMachine = Utils.GetOrAddComponent<MouseStateMachine>(gameObject);
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();
        mouseEffects = GetComponent<MouseEffects>();
        mouseAnimator = GetComponent<Animator>();

        behaviorGacha = new Dictionary<EMousePattern, int>();
        sequence = DOTween.Sequence();

        startPos = transform.position;
        maxHP = Data.LIFE;
        
        //TODO: 원상복구 필요
        //HP = maxHP;
        HP = 1100;
        
        behaviorGacha.Add(EMousePattern.Rush, 35);
        behaviorGacha.Add(EMousePattern.SpawnRats, 15);
        behaviorGacha.Add(EMousePattern.Rock, 25);
        behaviorGacha.Add(EMousePattern.Tail, 25);

        if (stageStart)
            mouseStateMachine.Initialize("Enter", this, GetComponent<Animator>());
        else
        {
            mouseStateMachine.Initialize("Dead", this, GetComponent<Animator>());
            OnDead();
        }
        
        cts = new CancellationTokenSource();
    }

    public bool CheckPhaseChangeHp()
    {
        return HP <= 1000;
    }

    public bool CheckDead()
    {
        return HP <= 0;
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

    private void BeHitEffect()
    {
        var hitSequence = DOTween.Sequence();
        hitSequence
            .Append(mouseSpriteRenderer.DOFade(0.75f, 0.3f))
            .Append(mouseSpriteRenderer.DOFade(1f, 0.3f));
    }

    public float Rush()
    {
        StopSequence();

        sequence = DOTween.Sequence();
        sequence
            .OnStart(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.Rush);
                Managers.Sound.PlaySFX("Mouse_Stop");
            })
            .AppendInterval(0.25f)
            .AppendCallback(() => BackGroundControl(false))
            .AppendInterval(4.75f)
            .OnComplete(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.NoRush);
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.Run);
                BackGroundControl(true);
            });
        
        return sequence.Duration();
    }

    public float Rush2()
    {
        StopSequence();

        sequence = DOTween.Sequence();
        sequence
            .OnStart(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.Rush);
                Managers.Sound.PlaySFX("Mouse_Stop");
            })
            .AppendInterval(0.25f)
            .AppendCallback(() => BackGroundControl(false))
            .AppendInterval(3.5f)
            .OnComplete(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.NoRush);
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.Run);
                BackGroundControl(true);
            });

        return sequence.Duration();
    }

    public void TurnClipEvent()
    {
        if (isPhaseChanging)
            return;

        if (rushEvent == false)
        {
            transform.DOMove(transform.position - new Vector3(14f, 1.5f, 0), 1f).SetEase(Ease.InCubic);
            Managers.Sound.PlaySFX("Mouse_Rush");
        }
        else
        {
            transform.DOMove(startPos, 1f).SetEase(Ease.InCubic);
        }

        mouseSpriteRenderer.flipX = rushEvent;
        AllowAttack(!rushEvent);
        rushEvent = !rushEvent;
    }

    public void Turn2ClipEvent()
    {
        if (isPhaseChanging)
            return;

        if (rushEvent == false)
        {
            transform.DOMove(transform.position - new Vector3(14f, 1.5f, 0), 1f).SetEase(Ease.InCubic);
            Managers.Sound.PlaySFX("Mouse_Rush");
        }
        else
        {
            transform.DOMove(startPos, 1f).SetEase(Ease.InCubic);
        }

        AllowAttack(!rushEvent);
        rushEvent = !rushEvent;
    }

    public float SpawnRats()
    {
        StopSequence();

        Managers.Sound.PlaySFX("Mouse_Growling");
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
        StopSequence();

        Managers.Sound.PlaySFX("Mouse_Growling");

        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOShakeRotation(1f))
            .AppendCallback(() =>
            {
                var rockSpawnPoint = transform.position + Vector3.up * 10f + Random.Range(-10f, 1) * Vector3.right;
                Instantiate(mouseEffects.rock, rockSpawnPoint, Quaternion.identity);
            })
            .AppendInterval(3f);

        return sequence.Duration();
    }

    public float TailAttack()
    {
        StopSequence();

        sequence = DOTween.Sequence();
        sequence
            .OnStart(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.Tail);
                BackGroundControl(false);
            })
            .AppendInterval(1.53f)
            .AppendCallback(() =>
            {
                var tailSpawnPoint = transform.position + Vector3.left * 5.5f + Vector3.down * 2.1f;
                Instantiate(mouseEffects.tail, tailSpawnPoint, Quaternion.Euler(0, 0, 6f));
                Managers.Sound.PlaySFX("Mouse_Tail");
            })
            .AppendInterval(.7f)
            .OnComplete(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.Run);
                BackGroundControl(true);
            });

        // 꼬리 공격
        return sequence.Duration();
    }

    public void TailClipEvent()
    {
        if (Dead)
            return;

        AllowAttack(!tailEvent);
        tailEvent = !tailEvent;
    }

    public EMousePattern TakeOne()
    {
        return RandomizerUtil.From(behaviorGacha).TakeOne();
    }

    public float PhaseChangeSequence()
    {
        // Phase2 변경
        StopSequence();
        ProductionWaitSetting();
        phaseChange = true;
        Phase2?.Invoke();

        Managers.Sound.PlaySFX("Boss_Phase");

        sequence = DOTween.Sequence();
        sequence
            .OnStart(() =>
            {
                isPhaseChanging = true;
                mouseAnimator.runtimeAnimatorController = phase2Animator;
            })
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                mouseStateMachine.ChangeAnimation(EMouseAnimationType.Run);
                if (mouseSpriteRenderer.flipX == false)
                    mouseSpriteRenderer.flipX = true;
            })
            .Append(transform.DOMove(startPos, 0.5f))
            .OnComplete(() => isPhaseChanging = false);

        return sequence.Duration();
    }

    private void ProductionWaitSetting()
    {
        canHit = false;
        tag = "Untagged";
        gameObject.layer = 0;
        rushEvent = false;
        tailEvent = false;
    }

    async UniTaskVoid SmokeEffect()
    {
        for (int i = 0; i < 6; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            GameObject smoke = Instantiate(mouseEffects.smokePrefab);
            smoke.transform.position = transform.position + Random.insideUnitSphere * 1.25f;
        }
    }

    public override void OnDead()
    {
        Managers.Sound.PlaySFX("Boss_Death");
        StopSequence();
        ProductionWaitSetting();
        StageClear?.Invoke(gameObject);
        Dead = true;
        cts.Cancel();
        mouseAnimator.runtimeAnimatorController = phase2Animator;
        mouseStateMachine.ChangeAnimation(EMouseAnimationType.Dead);
        mouseStateMachine.ChangeAnimation(EMouseAnimationType.Clear);
        SmokeEffect().Forget();

        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(1f)
            .AppendCallback(() => { })
            .AppendInterval(2f)
            .OnComplete(() =>
            {
                Debug.Log("죽음");
                StageClear?.Invoke(gameObject);
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

    private void StopSequence()
    {
        runEffect.SetActive(false);
        AllowAttack(false);
        sequence.Kill();
        transform.DOKill();
        mouseStateMachine.ChangeAnimation(EMouseAnimationType.NoRush);
    }

    public void MinusRandomPecent(float _value)
    {
        if (patternPercent - _value < 10)
        {
            patternPercent = 10;
        }
        else
        {
            patternPercent -= _value;
        }
    }

    public void BackGroundControl(bool _set)
    {
        MovingBackGround?.Invoke(_set);
    }
    
    public void SaveCtsMouse2State(MousePhase2State ctsMouse2State)
    {
        MousePhase2State mp2 = ctsMouse2State;
        cts = mp2.cts;
    }
    
    public void StartRunEffect()
    {
        runEffect.SetActive(true);
    }

    public void StopRunEffect()
    {
        runEffect.SetActive(false);
    }
}
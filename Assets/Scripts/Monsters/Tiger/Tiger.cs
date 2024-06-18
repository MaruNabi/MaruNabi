using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tiger : Entity
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;
    [SerializeField] private TigerHand leftHand;
    [SerializeField] private TigerHand rightHand;
    [SerializeField] private AnimatorController[] animators;
    [SerializeField] private Transform[] bitePositions;
    [SerializeField] private Transform[] riceCakePositions;
    [SerializeField] private GameObject riceCakePrefab;

    private Animator headAnimator;
    private SpriteRenderer headSpriteRenderer;
    private TigerEffects tigerEffects;
    private Dictionary<ETigerBitePosition, int> behaviorGacha;
    private Sequence sequence;
    private TigerStateMachine stateMachine;
    private bool isPhaseChanging;
    private CancellationTokenSource cts;
    private Vector3 startPos;
    private List<IDelete> riceCakes;
    public bool IsPhaseChanging => isPhaseChanging;
    public int Phase { get; private set; }

    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "TIGER_MONSTER");
        maxHP = Data.LIFE;
        HP = 6000;
        headAnimator = head.GetComponent<Animator>();
        headSpriteRenderer = head.GetComponent<SpriteRenderer>();
        stateMachine = Utils.GetOrAddComponent<TigerStateMachine>(gameObject);
        cts = new CancellationTokenSource();
        behaviorGacha = new Dictionary<ETigerBitePosition, int>();
        tigerEffects = GetComponent<TigerEffects>();
        sequence = DOTween.Sequence();
        Phase = 1;
        startPos = transform.position;
        riceCakes = new List<IDelete>();

        behaviorGacha.Add(ETigerBitePosition.Left, 40);
        behaviorGacha.Add(ETigerBitePosition.Middle, 20);
        behaviorGacha.Add(ETigerBitePosition.Right, 40);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            headAnimator.runtimeAnimatorController = animators[1];
            Inhale();
        }
        // if (Input.GetKeyDown(KeyCode.M))
        //     rightHand.SideHandAttack2(cts.Token).Forget();
    }

    public override void OnDamage(float _damage)
    {
        BeHitEffect();
        HP -= _damage;

        if (HP <= 0)
        {
            HP = 0;
            CanHit(false);
        }
    }

    private void BeHitEffect()
    {
        var hitSequence = DOTween.Sequence();
        hitSequence
            .Append(headSpriteRenderer.DOFade(0.75f, 0.3f))
            .Append(headSpriteRenderer.DOFade(1f, 0.3f));
    }

    public void StageStartProduction()
    {
        sequence = DOTween.Sequence();
        sequence
            .PrependCallback(() => leftHand.gameObject.SetActive(true))
            .AppendInterval(1f)
            .AppendCallback(() => rightHand.gameObject.SetActive(true))
            .AppendInterval(2f)
            .Append(head.transform.DOScale(0.8f, 2f))
            .Join(head.transform.DOMove(head.transform.position + Vector3.up * 5f, 2f))
            .Join(body.transform.DOMove(body.transform.position + Vector3.up * 5f, 2f))
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                head.GetComponent<SpriteRenderer>().sortingOrder = 0;
                headAnimator.enabled = true;
            })
            .AppendInterval(2.5f)
            .OnComplete(() =>
            {
                CanHit(true);
                stateMachine.Initialize("Phase1", this);
                startPos = transform.position;
            });
    }

    public void CanHit(bool _canHit)
    {
        if (_canHit)
        {
            head.tag = "Enemy";
            head.gameObject.layer = 7;
        }
        else
        {
            head.tag = "NoDeleteEnemyBullet";
            head.gameObject.layer = 0;
        }
    }

    private async UniTaskVoid SmokeEffect()
    {
        for (int i = 0; i < 10; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            GameObject smoke = Instantiate(tigerEffects.smokePrefab);
            smoke.transform.position = transform.position + Random.insideUnitSphere * 2f;
        }
    }

    public float SideAtk()
    {
        StopSequence();

        int random = Random.Range(0, 2);

        if (random == 0)
            leftHand.SideHandAttack(cts.Token).Forget();
        else
            rightHand.SideHandAttack(cts.Token).Forget();

        sequence = DOTween.Sequence();
        sequence.AppendInterval(5f);


        return sequence.Duration();
    }
    
    public float SideAtk2()
    {
        StopSequence();

        int random = Random.Range(0, 2);

        if (random == 0)
            leftHand.SideHandAttack2(cts.Token).Forget();
        else
            rightHand.SideHandAttack2(cts.Token).Forget();

        sequence = DOTween.Sequence();
        sequence.AppendInterval(5f);


        return sequence.Duration();
    }

    public float DigAtk()
    {
        StopSequence();

        int random = Random.Range(0, 2);

        if (random == 0)
            leftHand.DigHandAttack(cts.Token).Forget();
        else
            rightHand.DigHandAttack(cts.Token).Forget();

        sequence = DOTween.Sequence();
        sequence.AppendInterval(5f);


        return sequence.Duration();
    }

    public float SlapAtk()
    {
        StopSequence();
        
        int random = Random.Range(0, 2);
        
        if(random == 0)
            leftHand.SlapHandAttack(cts.Token).Forget();
        else
            rightHand.SlapHandAttack(cts.Token).Forget();
        
        sequence = DOTween.Sequence();
        sequence.AppendInterval(5f);

        return sequence.Duration();
    }

    public void StopSequence()
    {
        //CanHit(false);
        sequence.Kill();
        transform.DOKill();
    }

    public bool CheckPhase2ChangeHp()
    {
        return HP <= 5000 && Phase == 1;
    }

    public bool CheckPhase3ChangeHp()
    {
        return HP <= 4000 && Phase == 2;
    }
    
    public bool CheckMiniPhaseChangeHP()
    {
        return HP <= 2000 && Phase == 3;
    }
    
    public bool CheckStageClear()
    {
        return HP <= 0 && Phase == 3;
    }

    public float ChangePhase2()
    {
        Debug.Log("ÆäÀÌÁî2");
        StopSequence();

        CanHit(false);
        cts.Cancel();
        leftHand.DeleteHands();
        rightHand.DeleteHands();

        sequence = DOTween.Sequence();
        sequence.OnStart(() =>
            {
                SmokeEffect().Forget();
                headAnimator.runtimeAnimatorController = animators[0];
                headAnimator.speed = 0f;
                isPhaseChanging = true;
                Phase = 2;
            })
            .AppendInterval(2.5f)
            .OnComplete(() =>
            {
                headAnimator.speed = 1f;
                stateMachine.SetState("Phase2");
                isPhaseChanging = false;
                CanHit(true);
                
                cts = new CancellationTokenSource();
                SpawnRiceCakePhase2(cts.Token).Forget();
            });

        return sequence.Duration();
    }

    public float Bite()
    {
        StopSequence();
        CanHit(false);

        var gachar = RandomizerUtil.From(behaviorGacha).TakeOne();
        Vector3 bitePos = Vector3.zero;

        if (gachar == ETigerBitePosition.Left)
            bitePos = bitePositions[0].position;
        else if (gachar == ETigerBitePosition.Middle)
            bitePos = bitePositions[1].position;
        else
            bitePos = bitePositions[2].position;

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1f, 1f))
            .Join(transform.DOMove(bitePos, 1f))
            .JoinCallback(() => { headAnimator.SetTrigger("Attack"); })
            .Append(transform.DOMove(startPos, 0.5f))
            .Join(transform.DOScale(0.8f, 1f))
            .OnComplete(() => CanHit(true));

        return sequence.Duration();
    }
    
    public float Inhale()
    {
        StopSequence();
        CanHit(false);
        
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1f, 1f))
            .JoinCallback(() => { headAnimator.SetTrigger("Inhale"); })
            .Join(transform.DOMoveY(transform.position.y - 1f, 1f))
            .AppendInterval(2f)
            .Append(transform.DOMove(startPos, 0.5f))
            .Join(transform.DOScale(0.8f, 1f))
            .JoinCallback(() => headAnimator.SetTrigger("Up"))
            .AppendInterval(1f)
            .OnComplete(() => CanHit(true));

        return sequence.Duration();
    }

    public float ChangePhase3()
    {
        stateMachine.SetState("Phase3");

        StopSequence();
        CanHit(false);
        
        cts.Cancel();
        
        riceCakes.ForEach(riceCake => riceCake.Delete());

        sequence = DOTween.Sequence();
        sequence.OnStart(() =>
            {
                SmokeEffect().Forget();
                headAnimator.runtimeAnimatorController = animators[1];
                Phase = 3;
                headAnimator.speed = 0f;
                isPhaseChanging = true;
            })
            .AppendInterval(2.5f)
            .AppendCallback(() =>
            {
                headAnimator.speed = 1f;
            })
            .AppendInterval(4f)
            .OnComplete(() =>
            {
                stateMachine.SetState("Phase3");
                isPhaseChanging = false;
                cts = new CancellationTokenSource();
                SpawnRiceCakePhase3(cts.Token).Forget();
                CanHit(true);
            });

        return sequence.Duration();
    }

    public void StageClear()
    {
        StopSequence();
        CanHit(false);
        cts.Cancel();
        riceCakes.ForEach(riceCake => riceCake.Delete());
        Destroy(gameObject);
        
        SmokeEffect().Forget();
        
        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                head.GetComponent<SpriteRenderer>().sortingOrder = -20;
                headAnimator.SetTrigger("Growling");
            })
            .Join(head.transform.DOScale(0f, 2f))
            .Join(head.transform.DOMove(head.transform.position + Vector3.down * 5f, 2f))
            .Join(body.transform.DOMove(body.transform.position + Vector3.down * 5f, 2f))
            .AppendInterval(1.5f)

            .AppendCallback(() =>
            {
                leftHand.ExitAnimation();
            })
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                rightHand.ExitAnimation();
            })
            .AppendInterval(2.5f)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    private async UniTask SpawnRiceCakePhase2(CancellationToken _cts)
    {
        try
        {
            int randomTime = Random.Range(30, 51);
            float time = randomTime * 0.1f;
            
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken:_cts);
            int randomInt = Random.Range(0, 2);
            GameObject riceCake = Instantiate(riceCakePrefab, transform.position, Quaternion.identity);
            riceCake.transform.position = riceCakePositions[randomInt].position;
            
            if(randomInt == 1)
                riceCake.GetComponent<DDuck>().SetVariables(this,false);
            
            riceCakes.Add(riceCake.GetComponent<IDelete>());
            
            _cts.ThrowIfCancellationRequested();
            
            await SpawnRiceCakePhase2(_cts);
        }
        catch (OperationCanceledException)
        {
            // Handle the cancellation if needed
            Debug.Log("RandomPattern cancelled");
        }
    }
    
    private async UniTask SpawnRiceCakePhase3(CancellationToken _cts)
    {
        try
        {
            int randomTime = Random.Range(30, 51);
            float time = randomTime * 0.1f;
            
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken:_cts);
            int randomInt = Random.Range(2, 4);
            GameObject riceCake = Instantiate(riceCakePrefab, transform.position, Quaternion.identity);
            riceCake.transform.position = riceCakePositions[randomInt].position;

            randomInt = Random.Range(0, 1);
            riceCake.GetComponent<DDuck>().SetVariables(this, randomInt == 1);

            riceCakes.Add(riceCake.GetComponent<IDelete>());
            
            _cts.ThrowIfCancellationRequested();
            
            await SpawnRiceCakePhase2(_cts);
        }
        catch (OperationCanceledException)
        {
            // Handle the cancellation if needed
            Debug.Log("RandomPattern cancelled");
        }
    }
}

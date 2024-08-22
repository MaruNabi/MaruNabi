using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Tiger : Entity
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;
    [SerializeField] private TigerHand leftHand;
    [SerializeField] private TigerHand rightHand;
    [SerializeField] private RuntimeAnimatorController[] animators;
    [SerializeField] private Transform[] bitePositions;
    [SerializeField] private Transform[] riceCakePositions;
    [SerializeField] private GameObject riceCakePrefab;
    [SerializeField] private GameObject magnet;
    [SerializeField] private GameObject underWall;
    [SerializeField] private GameObject[] howlingVFXs;
    [FormerlySerializedAs("stageSwitchingManager")] [SerializeField] private StageManager stageManager;
    [SerializeField] private CloudsController cloudsController;
    
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
    private Vector2 startColliderSize;
    private BoxCollider2D boxCollider2D;
    private Vector2 startColliderPos;
    public bool IsPhaseChanging => isPhaseChanging;
    public int Phase { get; private set; }

    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "TIGER_MONSTER");
        maxHP = Data.LIFE;
        HP = maxHP;
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
        
        boxCollider2D = GetComponent<BoxCollider2D>();
        startColliderSize = boxCollider2D.size;
        startColliderPos = boxCollider2D.offset;
        boxCollider2D.enabled = true;
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
            .Append(headSpriteRenderer.DOFade(0.5f, 0.3f))
            .Append(headSpriteRenderer.DOFade(1f, 0.3f));
    }

    public void StageStartProduction()
    {
        sequence = DOTween.Sequence();
        sequence
            .PrependCallback(() =>
            {
                leftHand.gameObject.SetActive(true);
                underWall.SetActive(true);
            })
            .AppendInterval(1f)
            .AppendCallback(() => rightHand.gameObject.SetActive(true))
            .AppendInterval(2f)
            .Append(head.transform.DOScale(0.8f, 2f))
            .Join(head.transform.DOMove(head.transform.position + Vector3.up * 5f, 2f))
            .Join(body.transform.DOMove(body.transform.position + Vector3.up * 5f, 2f))
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                head.GetComponent<SpriteRenderer>().sortingOrder = 1;
                headAnimator.enabled = true;
                Managers.Sound.PlaySFX("Tiger_Growling");
                GrowlingVFX().Forget();
                stateMachine.Initialize("Phase1", this);
            })
            .AppendInterval(2.5f)
            .OnComplete(() =>
            {
                Managers.Sound.StopBGM();
                Managers.Sound.SetBGMVolume(0.25f);
                Managers.Sound.PlayBGM("Tiger_Stage");

                CanHit(true);
                startPos = transform.position;
            });
    }

    public void CanHit(bool _canHit)
    {
        if (_canHit)
        {
            head.tag = "NoBumpEnemy";
            head.gameObject.layer = 7;
        }
        else
        {
            head.tag = "Untagged";
            head.gameObject.layer = 0;
        }
    }

    private async UniTaskVoid SmokeEffect(int _count = 10)
    {
        for (int i = 0; i < _count; i++)
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

        return 5f;
    }

    public float SlapAtk()
    {
        StopSequence();

        int random = Random.Range(0, 2);

        if (random == 0)
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
        return HP <= 9000 && Phase == 1;
    }

    public bool CheckPhase3ChangeHp()
    {
        return HP <= 6000 && Phase == 2;
    }

    public bool CheckMiniPhaseChangeHP()
    {
        return HP <= 3000 && Phase == 3;
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
        
        Managers.Sound.PlaySFX("Boss_Phase");
        sequence = DOTween.Sequence();
        sequence.OnStart(() =>
            {
                SmokeEffect().Forget();
                Managers.Sound.PlaySFX("Boss_Death");
                headAnimator.runtimeAnimatorController = animators[0];
                headAnimator.speed = 0f;
                isPhaseChanging = true;
                Phase = 2;
                Managers.Sound.PlaySFX("Tiger_Roar");
                if(!leftHand.gameObject.activeSelf)
                    leftHand.gameObject.SetActive(true);
        
                if(!rightHand.gameObject.activeSelf)
                    rightHand.gameObject.SetActive(true);
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
        gameObject.layer = 0;

        var gachar = RandomizerUtil.From(behaviorGacha).TakeOne();
        Vector3 bitePos;

        if (gachar == ETigerBitePosition.Left)
            bitePos = bitePositions[0].position;
        else if (gachar == ETigerBitePosition.Middle)
            bitePos = bitePositions[1].position;
        else
            bitePos = bitePositions[2].position;

        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOShakePosition(1f, 1f))
            .Append(transform.DOScale(1f, 2f))
            .Join(transform.DOMove(bitePos, 2f))
            .AppendCallback(() =>
            {
                // boxCollider2D.enabled = true;
                // boxCollider2D.size = new Vector2(startColliderSize.x * 1.4f, startColliderSize.y * 1.5f);
                // boxCollider2D.offset = new Vector2(startColliderPos.x, startColliderPos.y - 2.0f);
                headAnimator.SetTrigger("Attack");
                Managers.Sound.PlaySFX("Tiger_Bite");
                cloudsController.DisapCloud((int)gachar);
                cloudsController.DisapCloud((int)gachar + 3);
            })
            .AppendInterval(0.5f)
            .Append(transform.DOMove(startPos, 0.5f))
            .Join(transform.DOScale(0.8f, 0.5f))
            .OnComplete(() =>
            {
                // boxCollider2D.size = startColliderSize;
                // boxCollider2D.offset = startColliderPos;
                CanHit(true);
            });

        return sequence.Duration();
    }

    public float Inhale()
    {
        StopSequence();
        CanHit(false);
        magnet.SetActive(true);

        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOScale(1f, 1f))
            .JoinCallback(() =>
            {
                headAnimator.SetTrigger("Inhale");
                boxCollider2D.size = new Vector2(startColliderSize.x * 1.4f, startColliderSize.y * 2);
                boxCollider2D.offset = new Vector2(startColliderPos.x, startColliderPos.y - 1f);
                boxCollider2D.enabled = true;
                tag = "NoDeleteEnemyBullet";
            })
            .Join(transform.DOMoveY(transform.position.y - 1f, 1f))
            .JoinCallback(() => Managers.Sound.PlaySFX("Tiger_Inhale"))
            .AppendInterval(2f)
            .Append(transform.DOMove(startPos, 0.5f))
            .Join(transform.DOScale(0.8f, 1f))
            .JoinCallback(() =>
            {
                magnet.SetActive(false);
                headAnimator.SetTrigger("Up");
                boxCollider2D.size = startColliderSize;
                boxCollider2D.offset = startColliderPos;
                boxCollider2D.enabled = false;
            })
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                CanHit(true);
            });
        return sequence.Duration();
    }

    public float ChangePhase3()
    {
        cts.Cancel();
        StopSequence();
        CanHit(false);
        
        leftHand.DeleteHands();
        rightHand.DeleteHands();
        Managers.Sound.PlaySFX("Boss_Phase");
        transform.localScale = Vector3.one;
        transform.position = startPos;

        riceCakes.ForEach(riceCake =>
        {
            DOTween.Kill(riceCake);
            riceCake.Delete();
        });

        sequence = DOTween.Sequence();
        sequence.OnStart(() =>
            {
                SmokeEffect(15).Forget();
                Managers.Sound.PlaySFX("Boss_Death");
                headAnimator.runtimeAnimatorController = animators[1];
                Phase = 3;
                headAnimator.speed = 0f;
                isPhaseChanging = true;
            })
            .AppendInterval(2.5f)
            .AppendCallback(() =>
            {
                headAnimator.speed = 1f;
                Managers.Sound.PlaySFX("Tiger_Growling");
                GrowlingVFX().Forget();
            })
            .AppendInterval(4f)
            .OnComplete(() =>
            {
                isPhaseChanging = false;
                cts = new CancellationTokenSource();
                SpawnRiceCakePhase3(cts.Token).Forget();
                stateMachine.SetState("Phase3");
                CanHit(true);
            });

        return sequence.Duration();
    }

    public void StageClear()
    {
        stageManager.DisableBehavior();
        StopSequence();
        CanHit(false);
        cts.Cancel();
        stateMachine.Stop();
        Phase = 4;

        tag = "Untagged";
        gameObject.layer = 0;
        
        leftHand.DeleteHands();
        rightHand.DeleteHands();
        riceCakes.ForEach(riceCake => riceCake.Delete());
        
        SmokeEffect(20).Forget();
        Managers.Sound.PlaySFX("Boss_Death");
        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(4f)
            .AppendCallback(() =>
            {
                head.GetComponent<SpriteRenderer>().sortingOrder = -18;
            })
            .Join(head.transform.DOScale(0f, 5f))
            .Join(head.transform.DOMove(head.transform.position + Vector3.down * 5f, 4f))
            .Join(body.transform.DOMove(body.transform.position + Vector3.down * 5f, 4f))
            .JoinCallback(() => { leftHand.ExitAnimation(); })
            .JoinCallback(() => { rightHand.ExitAnimation(); })
            .AppendInterval(2f)
            .OnComplete(() => { stageManager.StageAllClear(); });
    }

    private async UniTask SpawnRiceCakePhase2(CancellationToken _cts)
    {
        try
        {
            int randomTime = Random.Range(30, 51);
            float time = randomTime * 0.1f;

            _cts.ThrowIfCancellationRequested();
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: _cts);
            _cts.ThrowIfCancellationRequested();


            GameObject riceCake = Instantiate(riceCakePrefab, transform.position, Quaternion.identity);
            
            int randomInt = Random.Range(0, 2);
            riceCake.transform.position = riceCakePositions[randomInt].position;

            riceCake.GetComponent<Tteok>().SetVariables(this, randomInt != 1);

            riceCakes.Add(riceCake.GetComponent<IDelete>());

            await SpawnRiceCakePhase2(_cts);
        }
        catch (OperationCanceledException)
        {
            // Handle the cancellation if needed
            Debug.Log("spawnRice2 cancelled");
        }
    }

    private async UniTask SpawnRiceCakePhase3(CancellationToken _cts)
    {
        try
        {
            int randomTime = Random.Range(30, 51);
            float time = randomTime * 0.1f;

            _cts.ThrowIfCancellationRequested();
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: _cts);
            _cts.ThrowIfCancellationRequested();

            int randomInt = Random.Range(2, 4);
            GameObject riceCake = Instantiate(riceCakePrefab, transform.position, Quaternion.identity);
            riceCake.transform.position = riceCakePositions[randomInt].position;

            randomInt = Random.Range(0, 2);
            riceCake.GetComponent<Tteok>().SetVariables(this, randomInt == 1);

            riceCakes.Add(riceCake.GetComponent<IDelete>());


            await SpawnRiceCakePhase3(_cts);
        }
        catch (OperationCanceledException)
        {
            // Handle the cancellation if needed
            Debug.Log("RiceCake3 cancelled");
        }
    }

    private async UniTaskVoid GrowlingVFX()
    {
        foreach (var t in howlingVFXs)
        {
            t.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
        }

        foreach (var howling in howlingVFXs)
        {
            howling.SetActive(false);
        }
    }
}
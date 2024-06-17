using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class Tiger : Entity
{
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject body;
    [SerializeField] private TigerHand leftHand;
    [SerializeField] private TigerHand rightHand;
    
    private Animator headAnimator;
    private SpriteRenderer headSpriteRenderer;
    private TigerEffects tigerEffects;
    private Dictionary<ETigerBitePosition, int> behaviorGacha;
    private Sequence sequence;
    private TigerStateMachine stateMachine;

    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "TIGER_MONSTER");
        maxHP = Data.LIFE;
        HP = 150000000;
        headAnimator = head.GetComponent<Animator>();
        headSpriteRenderer = head.GetComponent<SpriteRenderer>();
        stateMachine = Utils.GetOrAddComponent<TigerStateMachine>(gameObject);
        
        behaviorGacha = new Dictionary<ETigerBitePosition, int>();
        tigerEffects = GetComponent<TigerEffects>();
        sequence = DOTween.Sequence();

        behaviorGacha.Add(ETigerBitePosition.Left, 40);
        behaviorGacha.Add(ETigerBitePosition.Middle, 20);
        behaviorGacha.Add(ETigerBitePosition.Right, 40);
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.N))
    //         leftHand.DigHandAttack().Forget();
    //     if (Input.GetKeyDown(KeyCode.M))
    //         rightHand.DigHandAttack().Forget();
    // }

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
            .Append(headSpriteRenderer.DOFade(0.75f, 0.3f))
            .Append(headSpriteRenderer.DOFade(1f, 0.3f));
    }
    
    public void StageStartProduction()
    {
        Sequence sequence = DOTween.Sequence();
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
            .AppendInterval(1.5f)
            .OnComplete(()=>
            {
                AllowAttack(true);
                stateMachine.Initialize("Enter", this);
            });
    }
    
    public void AllowAttack(bool _canHit)
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
        for (int i = 0; i < 6; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            GameObject smoke = Instantiate(tigerEffects.smokePrefab);
            smoke.transform.position = transform.position + Random.insideUnitSphere * 1.25f;
        }
    }

    public float SideAtk()
    {
        StopSequence();
        
        int random = Random.Range(0,2);
        
        if(random == 0)
            leftHand.SideHandAttack().Forget();
        else
            rightHand.SideHandAttack().Forget();
        
        sequence = DOTween.Sequence();
        sequence.AppendInterval(5f);


        return sequence.Duration();
    }
    
    public void StopSequence()
    {
        //AllowAttack(false);
        sequence.Kill();
        transform.DOKill();
    }
}
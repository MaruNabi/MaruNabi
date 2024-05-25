using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fox : Entity
{
    public static Action Stage3Clear;

    [SerializeField] private Transform ground;
    
    private const int DAMAGE_VALUE = 100;
    
    private FoxEffects foxEffects;
    private FoxStateMachine foxStateMachine;
    
    private Animator foxAnimator;
    private SpriteRenderer foxSpriteRenderer;
    private Sequence sequence;
    
    private bool canHit;
    private int currentPhase;
    
    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "MOUSE_MONSTER");
        
        foxStateMachine = Utils.GetOrAddComponent<FoxStateMachine>(gameObject);
        foxSpriteRenderer = GetComponent<SpriteRenderer>();
        
        maxHP = Data.LIFE;
        HP = 500;
        
        foxEffects = GetComponent<FoxEffects>();
        foxAnimator = GetComponent<Animator>();
        
        //foxStateMachine.Initialize("Enter", this, GetComponent<Animator>());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
            Attack();
    }

    public void StateEnter()
    {
        foxStateMachine.Initialize("Enter", this);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (!canHit && !Dead)
            {
                BeHitEffect();
                OnDamage(DAMAGE_VALUE);
            }
        }
    }
    
    private void BeHitEffect()
    {
        var hitSequence = DOTween.Sequence();
        hitSequence
            .Append(foxSpriteRenderer.DOFade(0.75f, 0.3f))
            .Append(foxSpriteRenderer.DOFade(1f, 0.3f));
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
    
    public void StopSequence()
    {
        AllowAttack(false);
        sequence.Kill();
        transform.DOKill();
    }
    
    private void ProductionWaitSetting()
    {
        canHit = false;
        tag = "Untagged";
        gameObject.layer = 0;
    }

    public void Attack()
    {
        StopSequence();

        Vector3 upPosition = transform.position + Vector3.up * 3f;

        Vector3[] startPos = new []{upPosition + Vector3.left*3f, upPosition, upPosition + Vector3.right*3f};
        List<GameObject> attackObjects = new List<GameObject>();
        
        ChangeAnimation(EFoxAnimationType.Attack);
        
        for (int i = 0; i < startPos.Length; i++)
        {
            GameObject attackObject = Instantiate(foxEffects.throwObjects[Random.Range(0, foxEffects.throwObjects.Length)]);
            attackObject.transform.position = transform.position;
            attackObjects.Add(attackObject);
        }
        
        sequence = DOTween.Sequence();
        sequence
            .Append(attackObjects[0].transform.DOMove(startPos[0], 0.25f))
            .Append(attackObjects[1].transform.DOMove(startPos[1], 0.25f))
            .Append(attackObjects[2].transform.DOMove(startPos[2], 0.25f))
            .AppendInterval(0.25f)
            .AppendCallback(() =>
            {
                attackObjects.ForEach(x => x.GetComponent<FoxBullet>().Throw());
            });
    }

    public void ChangeAnimation(EFoxAnimationType _type)
    {
        
        switch (_type)
        {
            case EFoxAnimationType.Laugh:
                foxAnimator.SetTrigger("Laugh");
                break;
            case EFoxAnimationType.Die:
                foxAnimator.SetTrigger("Die");
                break;
            case EFoxAnimationType.Attack:
                foxAnimator.SetTrigger("Attack");
                break;
        }
    }
    
    public float GetAnimPlayTime()
    {
        var currentAnimatorStateInfo = foxAnimator.GetCurrentAnimatorStateInfo(0);
        return currentAnimatorStateInfo.length * currentAnimatorStateInfo.normalizedTime;
    }

    public override void OnDead()
    {
        StopSequence();
        ProductionWaitSetting();
        Dead = true;
        ChangeAnimation(EFoxAnimationType.Die);
        Stage3Clear?.Invoke();
    }
}
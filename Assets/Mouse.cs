using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Mouse : Entity
{
    private MouseStateMachine mouseStateMachine;
    private SpriteRenderer mouseSpriteRenderer;
    private BoxCollider2D headCollider;
    private Animator mouseAnimator;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private GameObject rats;
    [SerializeField] private GameObject rock;
    
    private Sequence hitSequence;
    private Sequence headButtSequence;

    private bool canHit;
    private bool phase2;

    public bool Phase2
    {
        get => phase2;
        set => phase2 = value;
    }

    private float randomPatternPercent;

    private Dictionary<EMousePattern, int> gachaProbability;

    private void Start()
    {
        Init();
    }

    protected override void Init()
    {
        //Data = Utils.GetDictValue(Managers.Data.monsterDict, "MOUSESCHOLAR_MONSTER");

        maxHP = 51000;
        gachaProbability = new Dictionary<EMousePattern, int>();
        
        mouseStateMachine = Utils.GetOrAddComponent<MouseStateMachine>(gameObject);
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();
        
        hitSequence = DOTween.Sequence();
        headButtSequence = DOTween.Sequence();
        
        mouseStateMachine.Initialize("Enter", this, GetComponent<Animator>());

        HP = maxHP;
        hpText.text = HP.ToString();
        randomPatternPercent = 30f;
        
        gachaProbability.Add(EMousePattern.HeadButt, 35);
        gachaProbability.Add(EMousePattern.SpawnRats, 15);
        gachaProbability.Add(EMousePattern.Rock, 25);
        gachaProbability.Add(EMousePattern.Tail, 25);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
             SpawnRats();
    }

    public bool CheckPhaseChangeHp()
    {
        return HP <= 49000;
    }
    
    public void SetCanHit(bool hit)
    {
        canHit = hit;
    }

    public Dictionary<EMousePattern, int> GetGacha()
    {
        return gachaProbability;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 머리에만 맞을 수 있게 콜라이더 영역 설정
        if (collision.CompareTag("Bullet"))
        {
            if (canHit)
            {
                BeHitEffect();
                OnDamage(100);
                hpText.text = HP.ToString();
            }
        }
    }

    private void BeHitEffect()
    {
        if (hitSequence.IsPlaying())
            return;

        hitSequence = DOTween.Sequence();
        hitSequence
            .Append(mouseSpriteRenderer.DOFade(0.25f, 0.25f))
            .Append(mouseSpriteRenderer.DOFade(1f, 0.25f));
    }

    public float HeadButt()
    {
        if (headButtSequence.IsPlaying())
            return 0;
        
        headButtSequence = DOTween.Sequence();
        headButtSequence.OnStart(() =>
            {
                if (mouseSpriteRenderer.flipX == false)
                    mouseSpriteRenderer.flipX = true;

                canHit = true;
            })
            .AppendInterval(0.5f)
            .Append(transform.DOMoveX(transform.position.x - 14f, 0.75f).SetEase(Ease.InCubic))
            .AppendCallback(() =>
            {
                mouseSpriteRenderer.flipX = false;
                canHit = false;
            })
            .AppendInterval(0.25f)
            .Append(transform.DOMoveX(transform.position.x, 0.75f));
        
        return headButtSequence.Duration();
    }

    public float SpawnRats()
    {
        transform.DOShakePosition(1f);
        
        var spawnPos= transform.position + Vector3.right * 5f;
        spawnPos.y -= 1f;
        Instantiate(rats, spawnPos, Quaternion.identity);

        return 2f;
    }
    
    public float Rock()
    {
        var spawnPos= transform.position + Vector3.up * 15f;
        Instantiate(rock, spawnPos, Quaternion.identity);
        
        return 4.5f;
    }
}
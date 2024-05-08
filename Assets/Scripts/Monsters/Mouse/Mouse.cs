using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Mouse : Entity
{
    [SerializeField] private GameObject rats;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject tail;
    [SerializeField] private Sprite mouseStand;
    [SerializeField] private Sprite mouseDead;
    
    private MouseStateMachine mouseStateMachine;
    private SpriteRenderer mouseSpriteRenderer;
    private BoxCollider2D headCollider;
    private Animator mouseAnimator;
    private Sequence sequence;
    private TMP_Text hpText;
    private Vector3 startPos;
    

    private Dictionary<EMousePattern, int> behaviorGacha;
    public Dictionary<EMousePattern, int> BehaviorGacha => behaviorGacha;

    private float randomPatternPercent;
    private bool canHit;
    private bool phaseChange;

    public bool PhaseChange
    {
        get => phaseChange;
        set => phaseChange = value;
    }

    private void Start()
    {
        Init();
    }

    protected override void Init()
    {
        //Data = Utils.GetDictValue(Managers.Data.monsterDict, "MOUSESCHOLAR_MONSTER");

        maxHP = 51000;
        behaviorGacha = new Dictionary<EMousePattern, int>();

        mouseStateMachine = Utils.GetOrAddComponent<MouseStateMachine>(gameObject);
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();
        hpText = Utils.FindChild<TMP_Text>(gameObject);
        sequence = DOTween.Sequence();

        HP = maxHP;
        hpText.text = HP.ToString();
        randomPatternPercent = 30f;

        behaviorGacha.Add(EMousePattern.HeadButt, 35);
        behaviorGacha.Add(EMousePattern.SpawnRats, 15);
        behaviorGacha.Add(EMousePattern.Rock, 25);
        behaviorGacha.Add(EMousePattern.Tail, 25);
        startPos = transform.position;
        
        mouseStateMachine.Initialize("Enter", this, GetComponent<Animator>());
    }

    public bool CheckPhaseChangeHp()
    {
        return HP <= 49000;
    }
    
    public bool CheckDead()
    {
        return HP <= 0;
    }

    public void SetCanHit(bool hit)
    {
        canHit = hit;
    }

    public Dictionary<EMousePattern, int> GetGacha()
    {
        return behaviorGacha;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 머리에만 맞을 수 있게 콜라이더 영역 설정
        if (collision.CompareTag("Bullet"))
        {
            if (canHit)
            {
                HitEffect();
                OnDamage(1000);
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

    public float HeadButt()
    {
        if (sequence.IsPlaying())
            return 0;

        sequence = DOTween.Sequence();
        sequence
            .OnStart(() =>
            {
                if (mouseSpriteRenderer.flipX == false)
                    mouseSpriteRenderer.flipX = true;

                canHit = true;
            })
            .AppendInterval(1f)
            .Append(transform.DOMoveX(transform.position.x - 14f, 0.75f).SetEase(Ease.InCubic))
            .Join(transform.DOMoveY(transform.position.y - 1.5f, 0.75f).SetEase(Ease.InCubic))
            .AppendCallback(() =>
            {
                mouseSpriteRenderer.flipX = false;
                canHit = false;
            })
            .AppendInterval(0.25f)
            .Append(transform.DOMove(transform.position, 0.75f));

        return sequence.Duration();
    }

    public float SpawnRats()
    {
        if (sequence.IsPlaying())
            return 0;

        sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOShakePosition(1f))
            .AppendCallback(() =>
            {
                var spawnPos = transform.position + Vector3.right * 5f + Vector3.down * 0.25f;
                Instantiate(rats, spawnPos, Quaternion.identity);
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
                Instantiate(rock, rockSpawnPoint, Quaternion.identity);
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
                // 애니메이션 정지 후 뒤 돌기
                if (mouseSpriteRenderer.flipX == false)
                    mouseSpriteRenderer.flipX = true;

                canHit = true;
            })
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                var tailSpawnPoint = transform.position + Vector3.right * 5f + Vector3.down * 0.5f;
                Instantiate(tail, tailSpawnPoint, Quaternion.Euler(0, 0, 6.6f));
            })
            .AppendInterval(1.5f)
            .OnComplete(() =>
            {
                mouseSpriteRenderer.flipX = false;
                canHit = false;
            });


        // 꼬리 공격
        return sequence.Duration();
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
        mouseSpriteRenderer.sprite = mouseStand;
        
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
            .AppendCallback(() =>
            {
                mouseSpriteRenderer.sprite = mouseDead;
            })
            .AppendInterval(2f)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
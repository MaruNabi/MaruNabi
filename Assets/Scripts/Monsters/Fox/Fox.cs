using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Fox : Entity
{
    public static Action<GameObject> Stage3Clear;

    [SerializeField] private Transform ground;
    [SerializeField] private Transform bongsanSpawnPos;
    [SerializeField] private Transform[] hahwoiSpawnPos;
    [SerializeField] private Transform owkwangSpawnPos;
    [SerializeField] private GameObject owkwangBeamPos;
    [SerializeField] private GameObject[] maskPrefabs;
    [SerializeField] AnimatorController[] tailAnimators;

    private GameObject light;
    private FoxEffects effects;
    private FoxStateMachine stateMachine;
    private List<GameObject> attackObjects;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sequence sequence;

    private int tailCount;
    private int hahwoiDeathCount;
    private int hahwoiDisapCount;
    private int phase;
    private float time;
    private bool canAttack;

    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "FOX_MONSTER");
        stateMachine = Utils.GetOrAddComponent<FoxStateMachine>(gameObject);
        spriteRenderer = GetComponent<SpriteRenderer>();
        effects = GetComponent<FoxEffects>();
        animator = GetComponent<Animator>();
        attackObjects = new List<GameObject>();

        light = Instantiate(effects.lightPrefab);
        light.transform.position = transform.position + Vector3.down * 2f;
        light.SetActive(false);
        maxHP = Data.LIFE;
        HP = 13900;
        tailCount = 9;
        phase = 1;
    }

    private void Update()
    {
        if (canAttack)
        {
            time += Time.deltaTime;

            if (time >= 4f)
            {
                if (phase == 2)
                    Attack(9);
                else if (phase == 3)
                    Attack(10);

                time = 0;
            }
        }

        if (hahwoiDeathCount >= 2)
        {
            IsPhaseChange();
            hahwoiDeathCount = 0;
        }

        if (hahwoiDisapCount >= 2)
        {
            RestartPhase().Forget();
            hahwoiDisapCount = 0;
        }


        if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnOwkwangMask().Forget();
        }
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
            .Append(spriteRenderer.DOFade(0.75f, 0.3f))
            .Append(spriteRenderer.DOFade(1f, 0.3f));
    }

    public void AllowAttack(bool _canHit)
    {
        if (_canHit)
        {
            tag = "Enemy";
            gameObject.layer = 7;
        }
        else
        {
            tag = "NoDeleteEnemyBullet";
            gameObject.layer = 0;
        }
    }

    public void StopSequence()
    {
        AllowAttack(false);
        if (attackObjects.Count > 0)
        {
            attackObjects.ForEach(Destroy);
            attackObjects.Clear();
        }

        sequence.Kill();
        transform.DOKill();
    }

    private void ProductionWaitSetting()
    {
        tag = "Untagged";
        gameObject.layer = 0;
    }

    public void Attack(int _attackType)
    {
        StopSequence();

        Vector3 upPosition = transform.position + Vector3.up * 2f;

        Vector3[] startPos = new[] { upPosition + Vector3.left * 3f, upPosition, upPosition + Vector3.right * 3f };

        ChangeAnimation(EFoxAnimationType.Attack);

        var randomInt = Random.Range(0, _attackType);

        if (randomInt < 0)
            randomInt = 0;
        else if (randomInt > effects.throwObjects.Length - 1)
            randomInt = effects.throwObjects.Length - 1;

        for (int i = 0; i < startPos.Length; i++)
        {
            GameObject attackObject = Instantiate(effects.throwObjects[Random.Range(0, randomInt)]);
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
                foreach (var item in attackObjects)
                {
                    if (item.TryGetComponent<FoxBullet>(out var foxBullet))
                    {
                        foxBullet.Throw();
                    }
                    else
                    {
                        item.GetComponent<FoxSkullBullet>().Throw();
                    }
                }
                Managers.Sound.PlaySFX("Fox_Throw");
            });
    }

    public async UniTaskVoid SpawnBongsanMask()
    {
        StopSequence();

        GameObject smoke = Instantiate(effects.smokePrefab);
        smoke.transform.position = transform.position;

        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));

        GameObject mask = Instantiate(maskPrefabs[0], transform.position, Quaternion.identity);
        mask.GetComponent<BongsanMask>().SetVariables(this, bongsanSpawnPos);
    }


    public async UniTaskVoid SpawnHahwoiMask()
    {
        StopSequence();

        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));

        for (int i = 0; i < hahwoiSpawnPos.Length; i++)
        {
            GameObject smoke = Instantiate(effects.smokePrefab);
            smoke.transform.position = hahwoiSpawnPos[i].transform.position;

            GameObject mask = Instantiate(maskPrefabs[1], hahwoiSpawnPos[i].transform.position, Quaternion.identity);
            mask.GetComponent<HahwoiMask>().SetVariables(this);
        }
    }

    public async UniTaskVoid SpawnOwkwangMask()
    {
        StopSequence();

        GameObject smoke = Instantiate(effects.smokePrefab);
        smoke.transform.position = transform.position;

        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));

        GameObject mask = Instantiate(maskPrefabs[2], transform.position, Quaternion.identity);
        mask.GetComponent<OwkwangMask>().SetVariables(this, owkwangSpawnPos, owkwangBeamPos);
    }

    public async UniTaskVoid UseTailPhase1()
    {
        StopSequence();
        tailCount--;
        animator.runtimeAnimatorController = tailAnimators[8 - tailCount];
        Managers.Sound.PlaySFX("Fox_Tail");

        light.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f));
        light.SetActive(false);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        SpawnBongsanMask().Forget();
    }

    public async UniTaskVoid UseTailPhase2()
    {
        // phase 2 시작
        StopSequence();
        tailCount--;
        animator.runtimeAnimatorController = tailAnimators[8 - tailCount];
        Managers.Sound.PlaySFX("Fox_Tail");

        light.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f));
        light.SetActive(false);

        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        int random = Random.Range(0, 2);

        if (random == 0)
        {
            SpawnBongsanMask().Forget();
        }
        else
        {
            SpawnHahwoiMask().Forget();
        }

        canAttack = true;
    }

    public async UniTaskVoid UseTailPhase3()
    {
        // phase 3 시작
        StopSequence();
        tailCount -= 3;
        animator.runtimeAnimatorController = tailAnimators[5];
        Managers.Sound.PlaySFX("Fox_Tail");

        light.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f));
        light.SetActive(false);

        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        SpawnOwkwangMask().Forget();
        canAttack = true;
    }

    public async void IsPhaseChange()
    {
        time = 0;
        canAttack = false;
        hahwoiDeathCount = 0;

        StopSequence();

        if (tailCount <= 4)
        {
            phase = 3;
            ChangeAnimation(EFoxAnimationType.Angry);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            UseTailPhase3().Forget();
            Managers.Sound.PlaySFX("Boss_Phase");
            // 페이즈 3 시작
        }
        else if (tailCount <= 7 && tailCount > 4)
        {
            phase = 2;
            ChangeAnimation(EFoxAnimationType.Angry);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            UseTailPhase2().Forget();
            Managers.Sound.PlaySFX("Boss_Phase");
        }
        else
        {
            UseTailPhase1().Forget();
        }
    }

    public void HahwoiDeathCountUp()
    {
        hahwoiDeathCount++;
    }

    public void HahwoiDisapCountUp()
    {
        hahwoiDisapCount++;
    }

    public async UniTaskVoid CanHitState()
    {
        StopSequence();
        ChangeAnimation(EFoxAnimationType.Shake);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        tag = "Enemy";
        gameObject.layer = 7;
        canAttack = false;
        // 하얀 테두리
    }

    public async UniTaskVoid RestartPhase()
    {
        StopSequence();

        hahwoiDisapCount = 0;

        ChangeAnimation(EFoxAnimationType.Laugh);
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        ChangeAnimation(EFoxAnimationType.Scrub);

        light.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f));
        light.SetActive(false);

        if (phase == 1)
            SpawnBongsanMask().Forget();
        else if (phase == 2)
        {
            int random = Random.Range(0, 2);

            if (random == 0)
            {
                SpawnBongsanMask().Forget();
            }
            else
            {
                SpawnHahwoiMask().Forget();
            }
        }
        else
        {
            SpawnOwkwangMask().Forget();
        }
    }

    public void ChangeAnimation(EFoxAnimationType _type)
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        switch (_type)
        {
            case EFoxAnimationType.Laugh:
                animator.SetTrigger("Laugh");
                Managers.Sound.PlaySFX("Fox_Laugh");
                break;
            case EFoxAnimationType.Die:
                animator.SetTrigger("Die");
                break;
            case EFoxAnimationType.Attack:
                animator.SetTrigger("Attack");
                break;
            case EFoxAnimationType.Scrub:
                animator.SetTrigger("Scrub");
                Managers.Sound.PlaySFX("Fox_Charging");
                break;
            case EFoxAnimationType.Angry:
                animator.SetTrigger("Angry");
                Managers.Sound.PlaySFX("Fox_Angry");
                break;
            case EFoxAnimationType.Shake:
                animator.SetTrigger("Shake");
                Managers.Sound.PlaySFX("Fox_Breath");
                break;
        }
    }

    public override void OnDead()
    {
        StopSequence();
        ProductionWaitSetting();
        if (animator.runtimeAnimatorController != tailAnimators[5])
            animator.runtimeAnimatorController = tailAnimators[5];
        Managers.Sound.PlaySFX("Boss_Death");

        var soul = Instantiate(effects.soulPrefab, transform.position, Quaternion.identity);
        soul.SetActive(true);
        soul.GetComponent<SoulProduction>().ClearProduction();
        Dead = true;
        ChangeAnimation(EFoxAnimationType.Die);
        Stage3Clear?.Invoke(gameObject);
    }
}
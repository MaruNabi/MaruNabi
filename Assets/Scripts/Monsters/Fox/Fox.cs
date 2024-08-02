using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;
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
    [SerializeField] RuntimeAnimatorController[] tailAnimators;
    [SerializeField] private GameObject soulVFX;
    [SerializeField] private GameObject smokePrefab;
    [Header("VFX")]
    [SerializeField] private GameObject coreLight;
    
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
    private bool isAttackState;

    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "FOX_MONSTER");
        stateMachine = Utils.GetOrAddComponent<FoxStateMachine>(gameObject);
        spriteRenderer = GetComponent<SpriteRenderer>();
        effects = GetComponent<FoxEffects>();
        animator = GetComponent<Animator>();
        attackObjects = new List<GameObject>();
        maxHP = Data.LIFE;
        HP = 3000;
        tailCount = 9;
        phase = 1;
    }

    private void Update()
    {
        if (canAttack)
        {
            time += Time.deltaTime;
            isAttackState = false;
            if (time >= 4f)
            {
                isAttackState = true;
                
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
            .Append(spriteRenderer.DOFade(0.5f, 0.3f))
            .Append(spriteRenderer.DOFade(1f, 0.3f));
    }

    private void AllowAttack(bool _canHit)
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

    private void StopSequence()
    {
        AllowAttack(false);
        if (attackObjects.Count > 0)
        {
            attackObjects.ForEach(Destroy);
            attackObjects.Clear();
        }

        sequence.Kill();
        transform.DOKill();
        ChangeAnimation(EFoxAnimationType.Idle);
    }

    private void ProductionWaitSetting()
    {
        tag = "Untagged";
        gameObject.layer = 0;
    }

    private void Attack(int _attackType)
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

    private async UniTaskVoid SpawnBongsanMask()
    {
        GameObject smoke = Instantiate(effects.smokePrefab);
        smoke.transform.position = transform.position;

        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
        GameObject mask = Instantiate(maskPrefabs[0], transform.position, Quaternion.identity);
        mask.GetComponent<BongsanMask>().SetVariables(this, bongsanSpawnPos);
    }

    private async UniTaskVoid SpawnHahwoiMask()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));

        for (int i = 0; i < hahwoiSpawnPos.Length; i++)
        {
            GameObject smoke = Instantiate(effects.smokePrefab);
            smoke.transform.position = hahwoiSpawnPos[i].transform.position;

            GameObject mask = Instantiate(maskPrefabs[1], hahwoiSpawnPos[i].transform.position, Quaternion.identity);
            mask.GetComponent<HahwoiMask>().SetVariables(this);
        }
    }

    private async UniTaskVoid SpawnOwkwangMask()
    {
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

        coreLight.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        coreLight.SetActive(false);
        
        SpawnBongsanMask().Forget();
    }

    public async UniTaskVoid UseTailPhase2()
    {
        // phase 2 시작
        tailCount--;
        animator.runtimeAnimatorController = tailAnimators[8 - tailCount];
        Managers.Sound.PlaySFX("Fox_Tail");

        coreLight.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        coreLight.SetActive(false);
        
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
        tailCount -= 3;
        animator.runtimeAnimatorController = tailAnimators[5];
        
        Managers.Sound.PlaySFX("Fox_Tail");
        coreLight.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        coreLight.SetActive(false);
        
        SpawnOwkwangMask().Forget();
        canAttack = true;
    }

    public async void IsPhaseChange()
    {
        await UniTask.WaitUntil(()=> isAttackState == false);
        
        if(attackObjects.Count > 0)
        {
            foreach (var item in attackObjects)
            {
                if(item == null)
                    continue;
                
                item.GetComponent<FoxBullet>().DestroyBullet();
            }
            
            attackObjects.Clear();
        }
        
        time = 0;
        canAttack = false;
        hahwoiDeathCount = 0;

        StopSequence();

        if (tailCount <= 4)
        {
            phase = 3;
            ChangeAnimation(EFoxAnimationType.Angry);
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            UseTailPhase3().Forget();
            Managers.Sound.PlaySFX("Boss_Phase");
            // 페이즈 3 시작
        }
        else if (tailCount <= 7 && tailCount > 4)
        {
            phase = 2;
            ChangeAnimation(EFoxAnimationType.Angry);
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
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
        Managers.Sound.StopBGM();
        Managers.Sound.PlaySFX("Boss_Phase");
        ChangeAnimation(EFoxAnimationType.Shake);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        tag = "Enemy";
        gameObject.layer = 7;
        HP = 1500;
        canAttack = false;
        // 하얀 테두리
    }

    public async UniTaskVoid RestartPhase()
    {
        await UniTask.WaitUntil( ()=> isAttackState == false);
        
        if(attackObjects.Count > 0)
        {
            foreach (var item in attackObjects)
            {
                if(item == null)
                    continue;
                
                item?.GetComponent<FoxBullet>().DestroyBullet();
            }
            attackObjects.Clear();
        }
        
        StopSequence();
        hahwoiDisapCount = 0;

        ChangeAnimation(EFoxAnimationType.Laugh);
        await UniTask.Delay(TimeSpan.FromSeconds(.85f));
        
        Managers.Sound.PlaySFX("Fox_Charging");
        coreLight.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(.5f));
        coreLight.SetActive(false);

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
    
    async UniTaskVoid SmokeEffect()
    {
        for (int i = 0; i < 6; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            GameObject smoke = Instantiate(effects.smokePrefab);
            smoke.transform.position = transform.position + Random.insideUnitSphere * 1.25f;
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
            case EFoxAnimationType.Idle:
                animator.SetTrigger("Idle");
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
        Dead = true;
        SmokeEffect().Forget();
        Managers.Sound.PlaySFX("Boss_Death");
        ChangeAnimation(EFoxAnimationType.Die);
        OnDeadEffect().Forget();
    }
    
    private async UniTaskVoid OnDeadEffect()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        soulVFX.SetActive(true);
        soulVFX.GetComponent<SoulProduction>().ClearProduction();
        Stage3Clear?.Invoke(gameObject);
    }
}
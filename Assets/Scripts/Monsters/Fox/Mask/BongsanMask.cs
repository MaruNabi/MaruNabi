using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class BongsanMask : Entity
{
    [SerializeField] GameObject[] energyBeamsTransform;
    [SerializeField] GameObject energyBeamPrefab;
    
    private Fox fox;
    private Vector3 targetPos;
    private Vector3[] teleportPoints;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Sequence sequence;
    
    private bool isStart;
    private float time;
    private float disappearTime;
    
    private void Update()
    {
        if (isStart)
        {
            time += Time.deltaTime;

            if (time >= disappearTime)
            {
                OnDisappear();
                time = 0;
                isStart = false;
            }
        }
    }
    
    protected override void Init()
    {
        HP = Utils.GetDictValue(Managers.Data.monsterDict, "BONGSAN_MONSTER").LIFE;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SpawnAnimation(targetPos);
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

    public void SetVariables(Fox _fox, Transform _targetPoint, float _time = 15f)
    {
        fox = _fox;
        targetPos = _targetPoint.position;
        disappearTime = _time;
        time = 0;
        isStart = true;
    }
    
    private void BeHitEffect()
    {
        var hitSequence = DOTween.Sequence();
        hitSequence
            .Append(spriteRenderer.DOFade(0.75f, 0.3f))
            .Append(spriteRenderer.DOFade(1f, 0.3f));
    }

    private void SpawnAnimation(Vector3 _targetPos)
    {
        Managers.Sound.PlaySFX("Mask_Normal");
        animator.enabled = true;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(-42f, 0.5f))
            .Join(spriteRenderer.DOFade(1, 0.5f))
            .Append(transform.DOMove(_targetPos, 0.5f))
            .OnComplete(() =>
            {
                SaveTeleportPoints();
                Teleport();
            });
    }
    
    private void SaveTeleportPoints()
    {
        Vector3 referencePosition = transform.position;
        teleportPoints = new Vector3[3];
        teleportPoints[0] = referencePosition + Vector3.left * 5;
        teleportPoints[1] = referencePosition;
        teleportPoints[2] = referencePosition + Vector3.right * 5;
    }

    private void Teleport()
    {
        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                tag = "Enemy";
                gameObject.layer = 7;
                
                for (int i = 0; i < teleportPoints.Length; i++)
                {
                    animator.SetTrigger("Teleport");
                    transform.position = teleportPoints[Random.Range(0, 3)];
                    Managers.Sound.PlaySFX("Mask_Teleport");
                }

                Attack();
            });
    }

    private void Attack()
    {
        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(2f)
            .AppendCallback(() => animator.SetTrigger("Attack"))
            .Append(transform.DOMoveY(-48.6f, 0.5f))
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                for (int i = 0; i < 2; i++)
                {
                    var beam = Instantiate(energyBeamPrefab, energyBeamsTransform[i].transform.position,
                        Quaternion.identity);
                    var randomInt = Random.Range(0, 2);
                    if (randomInt == 0)
                    {
                        beam.AddComponent<EnergyBeam>().Init(true);
                        beam.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    else
                    {
                        beam.AddComponent<EnergyBeam>().Init(false);
                    }
                }
                Managers.Sound.PlaySFX("Mask_Energy");
            })
            .AppendInterval(0.5f)
            .Append(transform.DOMoveY(targetPos.y, 0.5f))
            .OnComplete(() => Teleport());

        // 공격 애니메이션 실행
    }

    private void OnDead()
    {
        sequence.Kill();
        tag = "Untagged";
        gameObject.layer = 0;
        animator.SetTrigger("Die");
        Managers.Sound.PlaySFX("Mask_Death");
        sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0, 1f))
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                // 여우에게 죽음 알리기
                fox.IsPhaseChange();
                Destroy(gameObject);
            });
    }
    
    private void OnDisappear()
    {
        sequence.Kill();
        tag = "Untagged";
        gameObject.layer = 0;
        animator.SetTrigger("Disappear");
        sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0, 1f))
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                // 여우에게 사라짐 알리기
                fox.RestartPhase().Forget();
                Destroy(gameObject);
            });
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class OwkwangMask : Entity
{
    [SerializeField] private GameObject energyBeamPrefab;
    [SerializeField] private GameObject earthBeamPrefab;
    
    private Fox fox;
    private Vector3 targetPos;
    private Vector3[] teleportPoints;
    private Vector3[] attackPoints;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isStart;

    private float time;
    private float attackTime;
    private float disappearTime;
    private Sequence sequence;

    private void Update()
    {
        if (isStart)
        {
            time += Time.deltaTime;
            attackTime += Time.deltaTime;
            
            if (time >= disappearTime)
            {
                OnDisappear();
                time = 0;
            }

            if (attackTime >= 2f)
            {
                EarthEnergyBeam();
                attackTime = 0;
            }
        }
    }
    
    protected override void Init()
    {
        HP = Utils.GetDictValue(Managers.Data.monsterDict, "OWKWANG_MONSTER").LIFE;
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

    public void SetVariables(Fox _fox, Transform _spawnPoint, GameObject targetPoints, float _time = 30f)
    {
        fox = _fox;
        targetPos = _spawnPoint.position;
        disappearTime = _time;
        attackPoints = targetPoints.GetComponentsInChildren<Transform>().Select(t => t.position).ToArray();
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
        Managers.Sound.PlaySFX("Mask_Owkwang");

        animator.enabled = true;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveY(-42f, 0.5f))
            .Join(spriteRenderer.DOFade(1, 0.5f))
            .Append(transform.DOMove(_targetPos, 0.5f))
            .OnComplete(() =>
            {
                SaveTeleportPoints();
                tag = "Enemy";
                gameObject.layer = 7;
                //Teleport();
            });
    }
    
    private void SaveTeleportPoints()
    {
        Vector3 referencePosition = transform.position;
        teleportPoints = new Vector3[3];
        teleportPoints[0] = referencePosition;
        teleportPoints[1] = referencePosition + Vector3.down * 3.5f;
        teleportPoints[2] = referencePosition + Vector3.down * 7f;

        Move();
    }

    private void Move()
    {
        Vector3[] points = new Vector3[3];
        if (transform.position.y == teleportPoints[2].y)
        {
            points[0] = teleportPoints[0];
            points[1] = teleportPoints[2];
            points[2] = teleportPoints[0];
        }
        else
        {
            points[0] = teleportPoints[2];
            points[1] = teleportPoints[0];
            points[2] = teleportPoints[2];
        }
        
        sequence = DOTween.Sequence();
        sequence
            .AppendInterval(1f)
            .Append(transform.DOMoveY(points[0].y, 1f))
            .JoinCallback(() => animator.SetTrigger("Move"))
            .Append(transform.DOMoveY(points[1].y, 1f))
            .Append(transform.DOMoveY(points[2].y, 1f))
            .AppendCallback(() =>
            {
                Attack();
            });
    }

    private void Attack()
    {
        int randomInt = Random.Range(0, 3);
        
        if(transform.position.y < teleportPoints[randomInt].y -5)
            animator.SetTrigger("Up");
        else if (transform.position.y > teleportPoints[randomInt].y+5)
        {
            animator.SetTrigger("Down");
        }
        
        sequence = DOTween.Sequence();
        sequence
            //.AppendCallback(() => animator.SetTrigger("Attack"))
            .Append(transform.DOMoveY(teleportPoints[randomInt].y, 0.5f).SetEase(Ease.InCirc))
            .AppendCallback(() =>
            {
                    var beam = Instantiate(energyBeamPrefab, transform.position + Vector3.right * 11,
                        Quaternion.identity);
                    beam.transform.eulerAngles = new Vector3(0, 0, 90);
                    beam.AddComponent<EnergyBeam>().Init(false);
                    Managers.Sound.PlaySFX("Mask_Energy");
            })
            .AppendInterval(0.5f)
            .JoinCallback(() => animator.SetTrigger("Beam"))
            .AppendInterval(1f)
            .JoinCallback(() => animator.SetTrigger("Idle"))
            .OnComplete(() => Move());
        
        // ���� �ִϸ��̼� ����
    }

    private void EarthEnergyBeam()
    {
        var beam = Instantiate(earthBeamPrefab, attackPoints[Random.Range(1, attackPoints.Length-1)] + Vector3.up*5,
            Quaternion.identity);
        beam.transform.eulerAngles = new Vector3(-180, 0, 0);
    }

    private void OnDead()
    {
        sequence.Kill();
        isStart = false;
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
                // ���쿡�� ���� �˸���
                fox.CanHitState().Forget();
                Destroy(gameObject);
            });
    }
    
    private void OnDisappear()
    { 
        sequence.Kill();
        isStart = false;
        tag = "Untagged";
        gameObject.layer = 0;
        animator.SetTrigger("Die");
        sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(0, 1f))
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                // ���쿡�� ����� �˸���
                fox.RestartPhase().Forget();
                Destroy(gameObject);
            });
    }
}
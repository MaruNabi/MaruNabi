using System;
using System.Collections;
using DG.Tweening;
using Mono.Cecil.Cil;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class Scholar : Entity
{
    // TODO : 결합도 낮추기
    public Animator scholarAnimator;
    public bool Idle
    {
        get { return isIdle; }
        set { isIdle = value; }
    }

    private ScholarStateMachine scholarStateMachine;
    private SpriteRenderer scholarSpriteRenderer;
    //private TMP_Text hpTextBox;
    private ScholarEffects scholarEffects;
    private Sequence sequence;
    private const float DAMAGE_VALUE = 1;
    private bool isIdle;
    
    private bool isHit;
    public bool IsHit
    {
        get => isHit;
        set => isHit = value;
    }

    private void Start()
    {
        base.Start();
        MouseScholar.PunishProduction += SetStatePunish;
    }

    private void OnDestroy()
    {
        MouseScholar.PunishProduction -= SetStatePunish;
    }

    protected override void Init()
    {
        //hpTextBox = Utils.FindChild<TMP_Text>(gameObject, "", true);
        scholarStateMachine = Utils.GetOrAddComponent<ScholarStateMachine>(gameObject);
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "SCHOLAR_MONSTER");

        scholarSpriteRenderer = GetComponent<SpriteRenderer>();
        scholarAnimator = GetComponent<Animator>();
        scholarEffects = GetComponent<ScholarEffects>();
        entityCollider = GetComponent<Collider2D>();

        scholarStateMachine.Initialize("Appearance", this, scholarAnimator);

        maxHP = Data.LIFE;
        HP = maxHP;
        //hpTextBox.text = HP.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 총알에 맞았을 때
        if (collision.gameObject.tag == "Bullet")
        {
            if (Idle)
            {
                isHit = true;
                BeHitEffect();
                OnDamage(DAMAGE_VALUE);
                //hpTextBox.text = HP.ToString();
            }
        }
    }

    // TODO : 이펙트 풀오브젝트 사용, 스크립트 분리
    public void AppearanceEffect()
    {
        GameObject smoke = Instantiate(scholarEffects.smokePrefab);
        smoke.transform.position = transform.position;

        float timer = 0;
        DOTween.To(() => timer, x => timer = x, 1f, 0.4f)
            .OnComplete(() => Destroy(smoke));
        
        sequence = DOTween.Sequence();
        sequence.OnStart(() => scholarSpriteRenderer.color = new Color(1, 1, 1, 0))
            .Append(scholarSpriteRenderer.DOFade(1f, 1f));
    }

    private void BeHitEffect()
    {
        sequence = DOTween.Sequence();
        sequence
            .Append(scholarSpriteRenderer.DOFade(0.5f, 0.3f))
            .Append(scholarSpriteRenderer.DOFade(1f, 0.3f));
    }
    
    public void SmokeEffect()
    {
        GameObject smoke = Instantiate(scholarEffects.smokePrefab);
        smoke.transform.position = transform.position;
        
        scholarSpriteRenderer.DOFade(0, 0.4f);
        //hpTextBox.DOFade(0, 0.4f);
    }
    
    public GameObject MakeFan(Vector3 fanPosition)
    {
        GameObject fan = Instantiate(scholarEffects.fanPrefab);
        fan.transform.position = fanPosition;
        return fan;
    }

    public void DestroyFan(GameObject fan)
    {
        Destroy(fan);
    }
    
    public override void OnDead()
    {
        DOTween.KillAll(scholarSpriteRenderer);
        DOTween.KillAll(this);
        isHit = false;
        base.OnDead();
    }

    public void AttackBlocking()
    {
        // Enemy 태그 없애서 피격 상태로 전환 방지
        tag = "Untagged";
        // 총알 삭제 방지
        entityCollider.enabled = false;
    }

    public void SetStatePunish()
    {
        scholarStateMachine.SetState("Punish");
    }

    public void StopStateMachine()
    {
        scholarStateMachine.Stop();
    }
}
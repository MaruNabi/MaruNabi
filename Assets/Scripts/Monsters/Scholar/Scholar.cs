using System;
using System.Collections;
using DG.Tweening;
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

    protected void Start()
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
    
    public override void OnDamage(float _damage)
    {
        isHit = true;
        BeHitEffect();
        HP -= _damage;
    
        if (HP <= 0)
        {
            HP = 0;
            OnDead();
        }
    }

    // TODO : 이펙트 풀오브젝트 사용, 스크립트 분리
    public void AppearanceEffect()
    {
        GameObject smoke = Instantiate(scholarEffects.smokePrefab);
        smoke.transform.position = transform.position;

        Managers.Sound.PlaySFX("Scholar_Enter");
        
        float timer = 0;
        DOTween.To(() => timer, x => timer = x, 1f, 0.4f)
            .OnComplete(() => Destroy(smoke));
        
        sequence = DOTween.Sequence();
        sequence.OnStart(() => scholarSpriteRenderer.color = new Color(1, 1, 1, 0))
            .Append(scholarSpriteRenderer.DOFade(1f, 1f));
    }

    private void BeHitEffect()
    {
        if (scholarSpriteRenderer == null)
            return;
        
        Sequence hitSequence = DOTween.Sequence();
        hitSequence
            .Append(scholarSpriteRenderer.DOFade(0.75f, 0.3f))
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
        DOTween.Kill(scholarSpriteRenderer);
        DOTween.Kill(this);
        isHit = false;
        base.OnDead();
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
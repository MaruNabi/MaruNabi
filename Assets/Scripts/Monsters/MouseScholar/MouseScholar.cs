using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class MouseScholar : Entity
{
    public static Action OnDeath;
    public static Action OnRoundEnd;
    
    private MouseScholarStateMachine _mouseScholarStateMachine;
    private SpriteRenderer mouseSpriteRenderer;
    private ScholarEffects mouseEffects;
    private TMP_Text hpText;
    private Sequence sequence;
    
    private const float DAMAGE_VALUE = 13000;
    private bool isHit;
    public bool IsHit { get => isHit; set => isHit = value; }
    
    private bool isIdle;
    public bool Idle { get => isIdle;  set => isIdle = value; }
    
    protected override void Init()
    {
        Data = Utils.GetDictValue(Managers.Data.monsterDict, "MOUSESCHOLAR_MONSTER");
        hpText = Utils.FindChild<TMP_Text>(gameObject, "", true);
        _mouseScholarStateMachine = Utils.GetOrAddComponent<MouseScholarStateMachine>(gameObject);
        
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();
        mouseEffects = GetComponent<ScholarEffects>();
        _mouseScholarStateMachine.Initialize("Appearance", this, GetComponent<Animator>());

        maxHP = Data.LIFE;
        HP = maxHP;
        hpText.text = HP.ToString();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (Idle)
            {
                isHit = true;
                BeHitEffect();
                OnDamage(DAMAGE_VALUE);
                hpText.text = HP.ToString();
            }
        }
    }

    public void AppearanceEffect()
    {
        GameObject smoke = Instantiate(mouseEffects.smokePrefab);
        smoke.transform.position = transform.position;
        
        GameObject straw = Instantiate(mouseEffects.strawPrefab);
        straw.transform.position = transform.position;

        float timer = 0;
        DOTween.To(() => timer, x => timer = x, 1f, 0.4f)
            .OnComplete(() =>
            {
                Destroy(smoke);
                Destroy(straw);
            });
        
        sequence = DOTween.Sequence();
        sequence.OnStart(() => mouseSpriteRenderer.color = new Color(1, 1, 1, 0))
            .Append(mouseSpriteRenderer.DOFade(1f, 1f));
    }
    
    public void SmokeEffect()
    {
        GameObject smoke = Instantiate(mouseEffects.smokePrefab);
        smoke.transform.position = transform.position;
        
        float timer = 0;
        DOTween.To(() => timer, x => timer = x, 1f, 0.4f)
            .OnComplete(() =>
            {
                Destroy(smoke);
            });
        mouseSpriteRenderer.DOFade(0, 0.4f);
        hpText.DOFade(0,0.4f);
    }

    private void BeHitEffect()
    {
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(mouseSpriteRenderer.DOFade(0.5f, 0.3f))
            .Append(mouseSpriteRenderer.DOFade(1f, 0.3f));
    }
    
    public GameObject MakeFan(Vector3 fanPosition)
    {
        GameObject fan = Instantiate(mouseEffects.fanPrefab);
        fan.transform.position = fanPosition;
        return fan;
    }

    public void DestroyFan(GameObject fan)
    {
        Destroy(fan);
    }
    
    public override void OnDead()
    {
        DOTween.KillAll(mouseSpriteRenderer);
        DOTween.KillAll(this);
        base.OnDead();
    }

    public void RoundEnd()
    {
        isHit = false;
        OnRoundEnd?.Invoke();
    }

    public void Death()
    {
        if(sequence.IsPlaying())
            sequence.Kill();
        
        Debug.Log("MouseScholar Death");
        mouseSpriteRenderer.DOFade(100, 2f);
        mouseSpriteRenderer.sprite = mouseEffects.mouseDeadSprite;
        OnDeath?.Invoke();
    }
}
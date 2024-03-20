using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class Scholar : LivingEntity
{
    private SpriteRenderer scholarSpriteRenderer;
    private TMP_Text hpTextBox;
    private Color BehitColor;
    private Color fadeColor;
    
    private float transparent = 0.5f;
    private float normal = 1.0f;
    
    private ScholarStateMachine scholarStateMachine;
    public ScholarManager scholarManager;
    [SerializeField] public Animator scholarAnimator;
    
    public bool isIdle = false;

    public bool IsIdle
    {
        get { return isIdle; }
        set { isIdle = value; }
    }

    private void Awake()
    {
        this.scholarStateMachine = this.gameObject.AddComponent<ScholarStateMachine>();

        this.scholarSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        Transform scholarCanvas = transform.GetChild(0);
        Transform textMeshProTransform = scholarCanvas.GetChild(0);
        this.hpTextBox = textMeshProTransform.GetComponent<TextMeshProUGUI>();

        // TO DO: 20240225 kimyeonmo HP 다른 곳에서 관리
        this.startingHP = 999999999;

        this.scholarManager = transform.parent.GetComponent<ScholarManager>();
        this.scholarAnimator = transform.GetComponent<Animator>();
    }

    void Start()
    {
        this.scholarStateMachine.Initialize("Appearance", this);

        // TO DO: 20240225 kimyeonmo UI 로직 분리 (delegate 이용)
        this.hpTextBox.text = HP.ToString();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if(this.IsIdle == true)
            {
                scholarAnimator.SetBool("BeHit", true);
                Debug.Log("총알 맞았다!!");

                scholarManager.SetSchloarBehit(true);
                OnDamage(100);

                Debug.Log("비히트?? " + scholarManager.GetIsSchloarBehit());
                
                this.hpTextBox.text = HP.ToString();

                StartCoroutine(BeHitEffect());
            }
        }
    }
    
    public IEnumerator AppearanceCoroutine()
    {
        this.fadeColor = scholarSpriteRenderer.color;
        this.fadeColor.a = 0f;
        scholarSpriteRenderer.color = this.fadeColor;

        while (this.fadeColor.a <= 1f)
        {
            this.fadeColor = scholarSpriteRenderer.color;
            this.fadeColor.a += 0.05f;
            scholarSpriteRenderer.color = this.fadeColor;
            
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator BeHitEffect()
    {
        this.BehitColor = this.scholarSpriteRenderer.color;

        this.BehitColor.a = this.transparent;

        this.scholarSpriteRenderer.color = this.BehitColor;

        yield return new WaitForSeconds(0.3f);

        this.BehitColor.a = this.normal;

        this.scholarSpriteRenderer.color = this.BehitColor;
    }
}
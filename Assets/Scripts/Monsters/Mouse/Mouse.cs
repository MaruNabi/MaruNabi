using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public partial class Mouse : LivingEntity
{
    private TMP_Text hpTextBox;

    public bool isIdle = false;

    public bool IsIdle
    {
        get { return isIdle; }
        set { isIdle = value; }
    }
    
    private MouseStateMachine mouseStateMachine;
    public ScholarManager scholarManager;

    public MouseManager mouseManager;
    [SerializeField] public Animator mouseAnimator;
    
    protected override void Init()
    {
        this.mouseStateMachine = this.gameObject.AddComponent<MouseStateMachine>();

        this.mouseSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        Transform mouseCanvas = transform.GetChild(0);
        Transform textMeshProTransform = mouseCanvas.GetChild(0);
        this.hpTextBox = textMeshProTransform.GetComponent<TextMeshProUGUI>();

        // TO DO: HP 다른 곳에서 관리
        this.maxHp = 999999999;

        this.mouseManager = transform.parent.GetComponent<MouseManager>();
        this.scholarManager = transform.parent.GetComponent<ScholarManager>();
        this.mouseAnimator = transform.GetComponent<Animator>();
    }

    private void Start()
    {
        this.mouseStateMachine.Initialize("Appearance", this);
        this.hpTextBox.text = HP.ToString();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (this.IsIdle == true)
            {
                Debug.Log("총알 맞았다!!");
                mouseAnimator.SetBool("BeHit", true);
                mouseManager.SetMouseBehit(true);
                OnDamage(523456789);

                Debug.Log("비히트?? " + mouseManager.GetIsMouseBehit());

                this.hpTextBox.text = HP.ToString();

                StartCoroutine(BeHitEffect());
            }
        }
    }
    
    private SpriteRenderer mouseSpriteRenderer;

    private Color fadeColor;

    private float transparent = 0.5f;
    private float normal = 1.0f;

    private Color BehitColor;

    public IEnumerator AppearanceCoroutine()
    {
        this.fadeColor = mouseSpriteRenderer.color;
        this.fadeColor.a = 0f;
        mouseSpriteRenderer.color = this.fadeColor;

        while (this.fadeColor.a <= 1f)
        {
            this.fadeColor = mouseSpriteRenderer.color;
            this.fadeColor.a += 0.05f;
            mouseSpriteRenderer.color = this.fadeColor;

            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator BeHitEffect()
    {
        this.BehitColor = this.mouseSpriteRenderer.color;

        this.BehitColor.a = this.transparent;

        this.mouseSpriteRenderer.color = this.BehitColor;

        yield return new WaitForSeconds(0.3f);

        this.BehitColor.a = this.normal;

        this.mouseSpriteRenderer.color = this.BehitColor;
    }
}

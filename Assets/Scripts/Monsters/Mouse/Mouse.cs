using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class Mouse : Entity
{
    [SerializeField] public Animator mouseAnimator;
    public ScholarManager scholarManager;
    public MouseManager mouseManager;
    
    private bool isIdle;
    public bool Idle
    {
        get { return isIdle; }
        set { isIdle = value; }
    }
    
    private MouseStateMachine mouseStateMachine;
    private SpriteRenderer mouseSpriteRenderer;
    private TMP_Text hpTextBox;
    private Color fadeColor;
    private Color BehitColor;

    private float transparent = 0.5f;
    private float normal = 1.0f;
    
    protected override void Init()
    {
        hpTextBox = Util.FindChild<TextMeshProUGUI>(gameObject, "", true);
        data = Util.GetDictValue(Managers.Data.monsterDict, "MOUSESCHOLAR_MONSTER");
        mouseStateMachine = Util.GetOrAddComponent<MouseStateMachine>(gameObject);
        
        mouseSpriteRenderer = GetComponent<SpriteRenderer>();
        mouseAnimator = GetComponent<Animator>();
        mouseManager = transform.parent.GetComponent<MouseManager>();
        scholarManager = transform.parent.GetComponent<ScholarManager>();
        
        mouseStateMachine.Initialize("Appearance", this);
    
        maxHP = data.LIFE;
        HP = maxHP;
        hpTextBox.text = HP.ToString();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (Idle)
            {
                mouseAnimator.SetBool("BeHit", true);
                mouseManager.SetMouseBehit(true);
                OnDamage(5);
                
                hpTextBox.text = HP.ToString();

                StartCoroutine(BeHitEffect());
            }
        }
    }

    public IEnumerator AppearanceCoroutine()
    {
        fadeColor = mouseSpriteRenderer.color;
        fadeColor.a = 0f;
        mouseSpriteRenderer.color = fadeColor;

        while (fadeColor.a <= 1f)
        {
            fadeColor = mouseSpriteRenderer.color;
            fadeColor.a += 0.05f;
            mouseSpriteRenderer.color = fadeColor;

            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator BeHitEffect()
    {
        BehitColor = mouseSpriteRenderer.color;
        BehitColor.a = transparent;
        mouseSpriteRenderer.color = BehitColor;

        yield return new WaitForSeconds(0.3f);
        
        BehitColor.a = normal;
        mouseSpriteRenderer.color = BehitColor;
    }
}

using System.Collections;
using UnityEngine;
using TMPro;

public class Scholar : Entity
{
    public ScholarManager scholarManager;
    public Animator scholarAnimator;
    
    private bool isIdle;
    public bool Idle
    {
        get { return isIdle; }
        set { isIdle = value; }
    }
    
    private ScholarStateMachine scholarStateMachine;
    private SpriteRenderer scholarSpriteRenderer;
    private TMP_Text hpTextBox;
    private Color BehitColor;
    private Color fadeColor;
    
    private float transparent = 0.5f;
    private float normal = 1.0f;

    protected override void Init()
    {
        hpTextBox = Util.FindChild<TextMeshProUGUI>(gameObject, "", true);
        scholarStateMachine = Util.GetOrAddComponent<ScholarStateMachine>(gameObject);
        data = Util.GetDictValue(Managers.Data.monsterDict, "SCHOLAR_MONSTER");
        
        scholarSpriteRenderer = GetComponent<SpriteRenderer>();
        scholarAnimator = GetComponent<Animator>();
        scholarManager = transform.parent.GetComponent<ScholarManager>();
        
        scholarStateMachine.Initialize("Appearance", this);
        
        maxHP = data.LIFE;
        HP = maxHP;
        hpTextBox.text = HP.ToString();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if(Idle)
            {
                scholarAnimator.SetBool("BeHit", true);

                scholarManager.SetSchloarBehit(true);
                OnDamage(100);
                
                hpTextBox.text = HP.ToString();
                StartCoroutine(BeHitEffect());
            }
        }
    }
    
    public IEnumerator AppearanceCoroutine()
    {
        fadeColor = scholarSpriteRenderer.color;
        fadeColor.a = 0f;
        scholarSpriteRenderer.color = fadeColor;

        while (fadeColor.a <= 1f)
        {
            fadeColor = scholarSpriteRenderer.color;
            fadeColor.a += 0.05f;
            scholarSpriteRenderer.color = fadeColor;
            
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator BeHitEffect()
    {
        BehitColor = scholarSpriteRenderer.color;
        BehitColor.a = transparent;
        scholarSpriteRenderer.color = BehitColor;

        yield return new WaitForSeconds(0.3f);

        BehitColor.a = normal;
        scholarSpriteRenderer.color = BehitColor;
    }
}
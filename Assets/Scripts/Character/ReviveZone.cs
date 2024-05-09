using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviveZone : MonoBehaviour
{
    private bool characterID;
    private bool isReviving;
    //public static int deathCount = 0;
    private int deathCount = 0;
    private float reviveTime = 1.5f;
    private const float ADD_REVIVE_TIME = 0.8f;
    private const float MAX_REVIVE_TIME = 9.5f;
    private float fillAmountUnit;

    public GameObject player;
    [SerializeField] private Image reviveBarImage;
    [SerializeField] private Canvas reviveBarCanvas;
    [SerializeField] private Transform effectPos;
    [SerializeField] private GameObject successEffect;

    private SpriteRenderer reviveSpriteRenderer;

    void Awake()
    {
        
    }

    private void OnEnable()
    {
        reviveSpriteRenderer = GetComponent<SpriteRenderer>();
        reviveBarCanvas.gameObject.SetActive(false);
        if (deathCount < 10)
        {
            reviveTime = 1.5f + (ADD_REVIVE_TIME * deathCount);
        }
        else
        {
            reviveTime = MAX_REVIVE_TIME;
        }
        deathCount++;

        StartCoroutine(ReviveZoneBlink());
        reviveBarImage.fillAmount = 0.0f;       //reviveBarImage Init
        fillAmountUnit = 1.0f / reviveTime * 0.02f;
        Debug.Log("deathCount" + deathCount);
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (isReviving)
        {
            reviveBarImage.fillAmount += fillAmountUnit;
            if (reviveBarImage.fillAmount >= 1.0f)
            {
                Instantiate(successEffect, effectPos.transform.position, effectPos.transform.rotation);
                Player.isReviveSuccess = true;
                reviveBarCanvas.gameObject.SetActive(false);
                reviveBarImage.fillAmount = 0.0f;
            }
        }
        else
        {
            reviveBarImage.fillAmount = 0.0f;
        }
    }

    private IEnumerator ReviveZoneBlink()
    {
        float blinkTime = 1.0f;
        float remainingTime = 0.0f;
        float startTime = Time.time;

        while (remainingTime < 10.0f)
        {
            reviveSpriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(blinkTime);
            reviveSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
            yield return new WaitForSeconds(blinkTime);
            blinkTime -= 0.1f;
            remainingTime = Time.time - startTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !(player.gameObject.GetComponent<Animator>().GetBool("isDead")))
        {
            isReviving = true;
            reviveBarCanvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isReviving = false;
            reviveBarCanvas.gameObject.SetActive(false);
        }
    }
}

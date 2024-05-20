using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFadeObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private GameObject maruTransform;
    private GameObject nabiTransform;

    private float maruDistance;
    private float nabiDistance;
    private float nearDistance;

    private float spriteAlpha;

    void Start()
    {
        maruTransform = GameObject.Find("Maru_Test");
        nabiTransform = GameObject.Find("Nabi_Test");

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            maruDistance = Vector3.Distance(gameObject.transform.position, maruTransform.transform.position);
            nabiDistance = Vector3.Distance(gameObject.transform.position, nabiTransform.transform.position);

            nearDistance = maruDistance > nabiDistance ? nabiDistance : maruDistance;

            if (nearDistance < 5.0f)
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
            else
            {
                spriteAlpha = (15.0f - nearDistance) / 10.0f;
                spriteRenderer.color = new Color(1, 1, 1, spriteAlpha);
            }
        }
    }
}

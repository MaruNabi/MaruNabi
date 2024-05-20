using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialText : MonoBehaviour
{
    private TextMeshPro tutorialText;

    private GameObject maruTransform;
    private GameObject nabiTransform;

    private float maruDistance;
    private float nabiDistance;
    private float nearDistance;

    private float textAlpha;

    void Start()
    {
        maruTransform = GameObject.Find("Maru_Test");
        nabiTransform = GameObject.Find("Nabi_Test");

        tutorialText = gameObject.GetComponent<TextMeshPro>();

        tutorialText.color = new Color(0, 0, 0, 0);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            maruDistance = Vector3.Distance(gameObject.transform.position, maruTransform.transform.position);
            nabiDistance = Vector3.Distance(gameObject.transform.position, nabiTransform.transform.position);

            nearDistance = maruDistance > nabiDistance ? nabiDistance : maruDistance;

            if (nearDistance < 10.0f)
            {
                tutorialText.color = new Color(0, 0, 0, 1);
            }
            else
            {
                textAlpha = (20.0f - nearDistance) / 10.0f;
                tutorialText.color = new Color(0, 0, 0, textAlpha);
            }
        }
    }
}

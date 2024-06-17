using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameOverUIController : MonoBehaviour
{
    private const int TEXT_TARGET_Y = 130;

    [SerializeField] private Image[] buttons;
    private int selectedButtonIndex = 0;
    private int buttonCount;
    [SerializeField] private Sprite selectedSprite, unSelectedSprite;

    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text buttonText1, buttonText2;
    private bool isSetEnd = false;

    void Start()
    {
        buttonCount = buttons.Length;
        StartCoroutine("GameOverInit");
    }

    void Update()
    {
        if (isSetEnd)
            ButtonsControl();
    }

    private void ButtonsControl()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttonCount) % buttonCount;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % buttonCount;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == selectedButtonIndex)
                buttons[i].GetComponent<Image>().sprite = selectedSprite;
            else
                buttons[i].GetComponent<Image>().sprite = unSelectedSprite;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            ExeFunction();
        }
    }

    private void ExeFunction()
    {
        switch (selectedButtonIndex)
        {
            case 0:
                Debug.Log("다시하기");
                break;
            case 1:
                Debug.Log("돌아가기");
                break;
            default:
                Debug.LogError("Not Found Button");
                break;
        }
    }

    private IEnumerator GameOverInit()
    {
        text.transform.DOScale(Vector3.one * 2.5f, 0.5f).SetEase(Ease.InExpo).From();
        yield return new WaitForSeconds(0.8f);
        buttonText1.DOFade(1, 0.5f);
        buttonText2.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        isSetEnd = true;
    }
}

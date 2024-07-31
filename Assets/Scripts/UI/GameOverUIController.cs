using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    private KeyCode selectKey;
    private bool isPadMoveOnce;

    void Start()
    {
        buttonCount = buttons.Length;
        StartCoroutine("GameOverInit");

        if (KeyData.isMaruPad)
            selectKey = KeyCode.Joystick1Button0;
        else
            selectKey = KeyCode.Z;
    }

    void Update()
    {
        if (isSetEnd)
            ButtonsControl();
    }

    private void ButtonsControl()
    {
        if (KeyData.isMaruPad)
        {
            if (Input.GetAxis("Horizontal_J1") == 0 && Input.GetAxis("Vertical_J1") == 0)
            {
                isPadMoveOnce = true;
            }

            if (Input.GetAxis("Horizontal_J1") >= 0.5f && isPadMoveOnce)
            {
                selectedButtonIndex = (selectedButtonIndex + 1) % buttonCount;
                isPadMoveOnce = false;
            }

            else if (Input.GetAxis("Horizontal_J1") <= -0.5f && isPadMoveOnce)
            {
                selectedButtonIndex = (selectedButtonIndex - 1 + buttonCount) % buttonCount;
                isPadMoveOnce = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                selectedButtonIndex = (selectedButtonIndex - 1 + buttonCount) % buttonCount;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                selectedButtonIndex = (selectedButtonIndex + 1) % buttonCount;
            }
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == selectedButtonIndex)
                buttons[i].GetComponent<Image>().sprite = selectedSprite;
            else
                buttons[i].GetComponent<Image>().sprite = unSelectedSprite;
        }

        if (Input.GetKeyDown(selectKey))
        {
            ExeFunction();
        }
    }

    private void ExeFunction()
    {
        switch (selectedButtonIndex)
        {
            case 0:
                LoadingScene.nextScene = "Stage2 1";
                SceneManager.LoadScene("LoadingScene");
                //SceneManager.LoadScene("Stage2");
                break;
            case 1:
                SceneManager.LoadScene("StageSelectionScene");
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

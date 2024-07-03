using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerLinkUIController : MonoBehaviour
{
    private const string keyboard = "키보드";
    private const string pad = "게임패드";

    [SerializeField] private Image maruImage;
    [SerializeField] private Image nabiImage;
    [SerializeField] private TMP_Text maruText;
    [SerializeField] private TMP_Text nabiText;
    [SerializeField] private Sprite keyboardImage, padImage;
    [SerializeField] private Sprite activeButton, unActiveButton;

    [SerializeField] private Image maruButton, nabiButton;
    [SerializeField] private TMP_Text maruBText, nabiBText;
    [SerializeField] private GameObject KeyInfo;

    private bool isMaruFirstSet = true;
    private bool isNabiFirstSet = true;
    private bool isMaruSelectEnd = false;
    private bool isNabiSelectEnd = false;

    private bool isMaruSelectPad;
    private bool isNabiSelectPad;

    private bool canSelectMaru = true;
    private bool canSelectNabi = true;

    private bool isActiveInfo = false;
    private bool isSetOnce = true;

    private Color disableColor = new Color(0, 0, 0, 0);
    private Color enableColorBlack = new Color(0, 0, 0, 1);
    private Color enableColorWhite = new Color(1, 1, 1, 1);

    void Start()
    {
        maruImage.color = disableColor;
        nabiImage.color = disableColor;
        maruText.color = disableColor;
        nabiText.color = disableColor;

        maruButton.color = disableColor;
        nabiButton.color = disableColor;
        maruBText.color = disableColor;
        nabiBText.color = disableColor;
    }

    void Update()
    {
        if (canSelectMaru)
            SelectMaru();

        if (canSelectNabi)
            SelectNabi();

        if (isActiveInfo)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                canSelectMaru = true;
                canSelectNabi = true;
                isMaruSelectEnd = false;
                isNabiSelectEnd = false;
                maruButton.sprite = activeButton;
                nabiButton.sprite = activeButton;

                isActiveInfo = false;
                KeyInfo.SetActive(false);
                isSetOnce = true;
            }
        }

        if (!isMaruFirstSet)
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                canSelectMaru = !canSelectMaru;
                isMaruSelectEnd = !isMaruSelectEnd;
                if (maruButton.sprite == activeButton)
                    maruButton.sprite = unActiveButton;
                else
                    maruButton.sprite = activeButton;
            }
        }

        if (!isNabiFirstSet)
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                canSelectNabi = !canSelectNabi;
                isNabiSelectEnd = !isNabiSelectEnd;
                if (nabiButton.sprite == activeButton)
                    nabiButton.sprite = unActiveButton;
                else
                    nabiButton.sprite = activeButton;
            }
        }

        if (isMaruSelectEnd && isNabiSelectEnd)
        {
            if (!isMaruSelectPad && !isNabiSelectPad)
            {
                DuplicationKey();
                return;
            }

            KeyData.isMaruPad = isMaruSelectPad;
            KeyData.isNabiPad = isNabiSelectPad;
            if (isMaruSelectPad && isNabiSelectPad)
                KeyData.isBothPad = true;
            else
                KeyData.isBothPad = false;
            SceneManager.LoadScene("TutorialScene");
        }
    }

    private void SelectMaru()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            maruImage.sprite = keyboardImage;
            maruText.text = keyboard;
            isMaruSelectPad = false;
            if (isMaruFirstSet)
            {
                maruImage.color = enableColorWhite;
                maruText.color = enableColorBlack;
                maruButton.color = enableColorWhite;
                maruBText.color = enableColorBlack;
                isMaruFirstSet = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            maruImage.sprite = padImage;
            maruText.text = pad;
            isMaruSelectPad = true;
            if (isMaruFirstSet)
            {
                maruImage.color = enableColorWhite;
                maruText.color = enableColorBlack;
                maruButton.color = enableColorWhite;
                maruBText.color = enableColorBlack;
                isMaruFirstSet = false;
            }
        }
    }

    private void SelectNabi()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            nabiImage.sprite = keyboardImage;
            nabiText.text = keyboard;
            isNabiSelectPad = false;
            if (isNabiFirstSet)
            {
                nabiImage.color = enableColorWhite;
                nabiText.color = enableColorBlack;
                nabiButton.color = enableColorWhite;
                nabiBText.color = enableColorBlack;
                isNabiFirstSet = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            nabiImage.sprite = padImage;
            nabiText.text = pad;
            isNabiSelectPad = true;
            if (isNabiFirstSet)
            {
                nabiImage.color = enableColorWhite;
                nabiText.color = enableColorBlack;
                nabiButton.color = enableColorWhite;
                nabiBText.color = enableColorBlack;
                isNabiFirstSet = false;
            }
        }
    }

    private void DuplicationKey()
    {
        if (isSetOnce)
        {
            isSetOnce = false;
            isActiveInfo = true;
            KeyInfo.SetActive(true);
        }
    }
}

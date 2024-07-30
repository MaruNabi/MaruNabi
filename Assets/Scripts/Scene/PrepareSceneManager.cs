using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PrepareSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject finalGuess;
    [SerializeField]
    private Image[] buttons;
    [SerializeField]
    private GameObject selectActivateSprite;
    private KeyCode selectKey;
    private int selectedButtonIndex = 0;
    private int buttonCount;
    private bool isActive;
    private bool canStageEnter;
    private bool isPadMoveOnce;

    public PrepareScene maruUIManager;
    public PrepareScene nabiUIManager;

    [SerializeField] private FirstInformationUI firstInformationUI = new FirstInformationUI();

    void Start()
    {
        buttonCount = buttons.Length;
        isActive = false;
        canStageEnter = false;
        isPadMoveOnce = true;
        
        if (KeyData.isMaruPad)
            selectKey = KeyCode.Joystick1Button0;
        else
            selectKey = KeyCode.Z;

        finalGuess.SetActive(false);
    }

    void Update()
    {
        if (firstInformationUI.IsActiveInformation)
            return;

        if (!canStageEnter)
        {
            if (maruUIManager.isReady == true && nabiUIManager.isReady == true)
            {
                Invoke("FinalGuessActive", 0.5f);
            }
            else
            {
                isActive = false;
                finalGuess.SetActive(false);
            }
        }

        if (isActive && finalGuess != null)
        {
            finalGuess.SetActive(true);
            ButtonsControl();
        }
    }

    private void FinalGuessActive()
    {
        isActive = true;
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
            {
                selectActivateSprite.transform.localPosition = buttons[i].transform.localPosition;
            }
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
                OnClickGameStart();
                break;
            case 1:
                break;
            default:
                Debug.LogError("Not Found Button");
                break;
        }
    }

    public void OnClickGameStart()
    {
        isActive = false;
        canStageEnter = true;
        maruUIManager.isGameStart = true;

        LoadingScene.nextScene = "Stage2 1";

        SceneManager.LoadScene("LoadingScene");
        //Managers.Scene.LoadScene(ESceneType.LoadingScene);
    }
}

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
    private int selectedButtonIndex = 0;
    private int buttonCount;
    private bool isActive;
    private bool canStageEnter;

    public PrepareScene maruUIManager;
    public PrepareScene nabiUIManager;

    void Start()
    {
        buttonCount = buttons.Length;
        isActive = false;
        canStageEnter = false;
        finalGuess.SetActive(false);
    }

    void Update()
    {
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
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttonCount) % buttonCount;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % buttonCount;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == selectedButtonIndex)
            {
                selectActivateSprite.transform.localPosition = buttons[i].transform.localPosition;
            }
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

        LoadingScene.nextScene = "Stage2";

        SceneManager.LoadScene("LoadingScene");
        //Managers.Scene.LoadScene(ESceneType.LoadingScene);
    }
}

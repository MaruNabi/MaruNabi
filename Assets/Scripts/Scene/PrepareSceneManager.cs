using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrepareSceneManager : BaseScene
{
    [SerializeField]
    private GameObject finalGuess;
    [SerializeField]
    private Image[] buttons;
    [SerializeField]
    private GameObject selectActivateSprite;
    private int selectedButtonIndex = 0;
    private int buttonCount;

    public PrepareScene maruUIManager;
    public PrepareScene nabiUIManager;

    void Start()
    {
        buttonCount = buttons.Length;

        finalGuess.SetActive(false);
    }

    void Update()
    {
        if (maruUIManager.isReady == true && nabiUIManager.isReady == true)
        {
            finalGuess.SetActive(true);
            ButtonsControl();
        }
    }

    protected override void Init()
    {
        base.Init();
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

        if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.RightBracket))
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
                finalGuess.SetActive(false);
                break;
            default:
                Debug.LogError("Not Found Button");
                break;
        }
    }

    public void OnClickGameStart()
    {
        LoadingScene.nextScene = "TestScene";

        Managers.Scene.LoadScene(ESceneType.LoadingScene);
    }

    public override void Clear()
    {

    }
}

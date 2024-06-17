using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private Image[] buttons;
    private int selectedButtonIndex = 0;
    private int buttonCount;

    [SerializeField] private Sprite selectedSprite, unSelectedSprite;
    [SerializeField] private GameObject firstInformation;

    private bool isActiveInformation;

    void Start()
    {
        isActiveInformation = true;
        buttonCount = buttons.Length;
    }

    void Update()
    {
        if (isActiveInformation)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                firstInformation.SetActive(false);
                isActiveInformation = false;
            }
        }
        else
        {
            ButtonsControl();
        }
    }

    private void ButtonsControl()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttonCount) % buttonCount;
        }
        else if (Input.GetKeyDown(KeyCode.S))
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
        switch(selectedButtonIndex)
        {
            case 0:
                OnClickGameStart();
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                Application.Quit();
                break;
            default:
                Debug.LogError("Not Found Button");
                break;
        }
    }

    public void OnClickGameStart()
    {
        //LoadingScene.nextScene = "TestScene";

        SceneManager.LoadScene("PlayerLinkScene");

        //Managers.Scene.LoadScene(ESceneType.StageSelectionScene);
    }
}

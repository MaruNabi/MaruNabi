using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private Image[] buttons;
    private int selectedButtonIndex = 0;
    private int buttonCount;

    [SerializeField] private Sprite selectedSprite, unSelectedSprite;
    [SerializeField] private GameObject firstInformation;
    [SerializeField] private FirstInformationUI firstInformationUI = new FirstInformationUI();

    void Start()
    {
        buttonCount = buttons.Length;

        Managers.Sound.PlayBGM("Title");
        Managers.Sound.SetBGMVolume(1);
    }

    void Update()
    {
        if (firstInformationUI.IsActiveInformation)
            return;
        else
        {
            ButtonsControl();

            if (Input.GetKeyDown(KeyCode.F7))
                SceneManager.LoadScene("Stage2 1");

            if (Input.GetKeyDown(KeyCode.F8))
                SceneManager.LoadScene("StageSelectionScene");

            if (Input.GetKeyDown(KeyCode.F9))
                SceneManager.LoadScene("StagePrepareScene");
        }
    }

    private void ButtonsControl()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttonCount) % buttonCount;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StopCoroutine("Movement");
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
        SceneManager.LoadScene("PlayerLinkScene");
    }
}

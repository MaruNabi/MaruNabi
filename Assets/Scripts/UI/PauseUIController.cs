using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseUIController : MonoBehaviour
{
    [SerializeField] private Image[] buttons;
    private int selectedButtonIndex = 0;
    private int buttonCount;
    private KeyCode selectKey;
    private bool isPadMoveOnce;

    [SerializeField] private Sprite selectedSprite, unSelectedSprite;

    void Start()
    {
        buttonCount = buttons.Length;

        if (KeyData.isMaruPad)
            selectKey = KeyCode.Joystick1Button0;
        else
            selectKey = KeyCode.Z;
    }

    void Update()
    {
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
                Stage2UI.isGamePaused = false;
                Stage2UI.isSetOnce = false;
                break;
            case 1:
                Stage2UI.isGamePaused = false;
                Stage2UI.isSetOnce = false;
                Time.timeScale = 1;
                LoadingScene.nextScene = "Stage2 1";
                SceneManager.LoadScene("LoadingScene");
                //SceneManager.LoadScene("Stage2");
                break;
            case 2:
                Stage2UI.isGamePaused = false;
                Stage2UI.isSetOnce = false;
                Time.timeScale = 1;
                SceneManager.LoadScene("StageSelectionScene");
                break;
            default:
                Debug.LogError("Not Found Button");
                break;
        }
    }
}

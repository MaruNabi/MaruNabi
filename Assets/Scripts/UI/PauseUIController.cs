using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseUIController : MonoBehaviour
{
    [SerializeField] private Image[] buttons;
    private int selectedButtonIndex = 0;
    private int buttonCount;

    [SerializeField] private Sprite selectedSprite, unSelectedSprite;

    void Start()
    {
        buttonCount = buttons.Length;
    }

    void Update()
    {
        ButtonsControl();
    }

    void OnEnable()
    {
        
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
                PauseUI.isGamePaused = false;
                PauseUI.isSetOnce = false;
                break;
            case 1:
                Debug.Log("다시 하기");
                break;
            case 2:
                Debug.Log("돌아가기");
                break;
            default:
                Debug.LogError("Not Found Button");
                break;
        }
    }
}

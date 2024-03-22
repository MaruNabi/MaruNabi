using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : BaseScene
{
    [SerializeField]
    private Image[] buttons;
    private int selectedButtonIndex = 0;
    private int buttonCount;

    [SerializeField]
    private Sprite selectedSprite, unSelectedSprite;

    void Start()
    {
        buttonCount = buttons.Length;
    }

    void Update()
    {
        ButtonsControl();
    }

    protected override void Init()
    {
        base.Init();
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
                buttons[i].GetComponent<Image>().color = Color.white;
            else
                buttons[i].GetComponent<Image>().color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.Return))
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

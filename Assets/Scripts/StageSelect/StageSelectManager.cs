using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StageSelectManager : MonoBehaviour
{
    int stageCursor = 0;
    int settingCursor = 0;

    int prevStageCursor = 0;
    int prevSettingCursor = 0;

    int stageNum = 3;
    int settingNum = 3;

    [SerializeField] GameObject stage1Go;
    [SerializeField] GameObject stage2Go;
    [SerializeField] GameObject stage3Go;

    [SerializeField] GameObject spinSettingGo;

    [SerializeField] GameObject practiceRoomGo;
    [SerializeField] GameObject shopGo;
    [SerializeField] GameObject InventoryGo;

    List<GameObject> stages = new List<GameObject>();
    List<GameObject> settings = new List<GameObject>();

    [SerializeField] TMP_Text testText;

    Color selectColor = new Color(0f, 1f, 0f);
    Color normalColor = new Color(1f, 1f, 1f);

    bool isStage = true;

    private void Start()
    {
        stages.Add(stage1Go);
        stages.Add(stage2Go);
        stages.Add(stage3Go);

        settings.Add(practiceRoomGo);
        settings.Add(shopGo);
        settings.Add(InventoryGo);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(isStage == true)
            {
                stageCursor++;
                SelectStage();
            }
            else
            {
                settingCursor++;
                SelectSetting();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (isStage == true)
            {
                stageCursor--;
                SelectStage();
            }
            else
            {
                settingCursor--;
                SelectSetting();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isStage == true)
            {
                MoveStage();
            }
            else
            {
                MoveSetting();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isStage = !isStage;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isStage = !isStage;
        }
    }

    void SelectStage()
    {
        stageCursor += stageNum;
        stageCursor %= stageNum;

        stages[prevStageCursor].GetComponent<Image>().color = normalColor;
        stages[stageCursor].GetComponent<Image>().color = selectColor;

        prevStageCursor = stageCursor;
        Debug.Log("현재 스테이지 인덱스: " + stageCursor);
    }

    void SelectSetting()
    {
        settingCursor += settingNum;
        settingCursor %= settingNum;

        settings[prevSettingCursor].GetComponent<Image>().color = normalColor;
        settings[settingCursor].GetComponent<Image>().color = selectColor;

        prevSettingCursor = settingCursor;
        Debug.Log("현재 설정 인덱스: " + settingCursor);
    }

    void MoveStage()
    {
        EventSystem.current.SetSelectedGameObject(stages[stageCursor]);
    }

    void MoveSetting()
    {
        EventSystem.current.SetSelectedGameObject(settings[settingCursor]);
    }

    public void LoadStage1Scene()
    {
        testText.text = "Load Stage 1";
    }

    public void LoadStage2Scene()
    {
        testText.text = "Load Stage 2";
    }

    public void LoadStage3Scene()
    {
        testText.text = "Load Stage 3";
    }

    public void LoadPractice()
    {
        testText.text = "Practice Room";
    }

    public void LoadShop()
    {
        testText.text = "Shop";
    }

    public void LoadInventory()
    {
        testText.text = "Inventory";
    }
}

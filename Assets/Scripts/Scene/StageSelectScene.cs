using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StageSelectScene : BaseScene
{
    public GameObject[] stage;
    public GameObject[] slots;
    public GameObject[] stageName = new GameObject[6];
    public GameObject slotBorderImage;
    public GameObject stageBorderImage;

    [SerializeField]
    private int currentStageNum;

    private GameObject[][] stageMatrics = new GameObject[2][];
    private GameObject selectedUI;
    private int currentDepth;
    private int selectedButtonIndex = 0;
    private int buttonCount;
    private bool isSetPos = false;

    private float slotMovePos;
    private float slotOriginPos;

    private Color originColor = new Color(1, 1, 1, 1);
    private Color disableColor = new Color(1, 1, 1, 0);

    void Start()
    {
        buttonCount = stage.Length;
        currentDepth = 0;
        slotMovePos = -347;
        slotOriginPos = -463.82f;

        for (int i = 0; i < stageName.Length; i++)
        {
            if (i <= currentStageNum)
                stageName[i].SetActive(true);
            else
                stageName[i].SetActive(false);
        }

        stageMatrics[0] = stage;
        stageMatrics[1] = slots;
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
        SelectSlotNum();

        if (Input.anyKeyDown)
        {
            isSetPos = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttonCount) % buttonCount;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % buttonCount;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentDepth != 0)
            {
                currentDepth -= 1;
                buttonCount = stageMatrics[currentDepth].Length;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentDepth < 1)
            {
                currentDepth += 1;
                buttonCount = stageMatrics[currentDepth].Length;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            switch (currentDepth)
            {
                case 0:
                    StageFunction();
                    break;
                case 1:
                    SlotFunction();
                    break;
                default:
                    break;
            }    
        }
    }

    private void SelectSlotNum()
    {
        if (stageMatrics[currentDepth].Length - 1 <= selectedButtonIndex)
        {
            selectedButtonIndex = stageMatrics[currentDepth].Length - 1;
        }

        selectedUI = stageMatrics[currentDepth][selectedButtonIndex];
        
        if (!isSetPos)
        {
            isSetPos = true;

            if (currentDepth == 0)
            {
                stageBorderImage.SetActive(true);
                slotBorderImage.SetActive(false);

                for (int i = 0; i < slots.Length; i++)
                {
                    slots[i].transform.DOLocalMoveY(slotOriginPos, 0.1f);
                }

                stageBorderImage.transform.position = selectedUI.transform.position;
            }
            else
            {
                slotBorderImage.SetActive(true);
                stageBorderImage.SetActive(false);

                for (int i = 0; i < slots.Length; i++)
                {
                    Vector2 temp = slots[i].transform.localPosition;
                    temp.y = slotOriginPos;
                    slots[i].transform.localPosition = temp;
                }
                slotBorderImage.transform.position = selectedUI.transform.position;

                selectedUI.transform.DOLocalMoveY(slotMovePos, 0.1f);
                slotBorderImage.transform.DOLocalMoveY(slotMovePos, 0.1f);
            }
        }
    }

    private void StageFunction()
    {
        if (selectedButtonIndex <= currentStageNum + 1)
        {
            switch (selectedButtonIndex)
            {
                case 0:
                    Debug.Log("서문");
                    break;
                case 1:
                    Debug.Log("재화문 이야기");
                    break;
                case 2:
                    Debug.Log("재화문 관문");
                    break;
                case 3:
                    Debug.Log("선악문 이야기");
                    break;
                case 4:
                    Debug.Log("선악문 관문");
                    break;
                case 5:
                    Debug.Log("수호문 이야기");
                    break;
                case 6:
                    Debug.Log("수호문 관문");
                    break;
                case 7:
                    Debug.Log("급제");
                    break;
                default:
                    break;
            }
        }
    }

    private void SlotFunction()
    {
        switch (selectedButtonIndex)
        {
            case 0:
                Debug.Log("연습실 입장");
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
    }

    public override void Clear()
    {
        
    }
}

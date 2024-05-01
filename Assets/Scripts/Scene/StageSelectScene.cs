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
    private int currentDepth;
    private int selectedButtonIndex = 0;
    private int buttonCount;

    private float slotMovePos;
    private float slotOriginPos;

    private Color originColor = new Color(1, 1, 1, 1);
    private Color disableColor = new Color(1, 1, 1, 0);

    void Start()
    {
        buttonCount = stage.Length;
        currentDepth = 0;
        slotMovePos = 193;
        slotOriginPos = 76.18f;

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
        if (Input.GetKeyDown(KeyCode.A))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttonCount) % buttonCount;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % buttonCount;
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

    }

    private void StageFunction()
    {

    }

    private void SlotFunction()
    {

    }

    public override void Clear()
    {
        
    }
}

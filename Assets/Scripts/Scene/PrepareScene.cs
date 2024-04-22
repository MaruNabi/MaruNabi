using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PrepareScene : BaseScene
{
    [SerializeField]
    private GameObject[] skillSlots;
    public TextMeshProUGUI selectedSlotName;
    public TextMeshProUGUI selectedSlotTitle;
    public TextMeshProUGUI selectedSlotDiscription;

    private GameObject[] skill = new GameObject[3];
    private GameObject[] skillDescript = new GameObject[3];
    private GameObject[][] UIMatrics = new GameObject[4][];
    private int currentDepth;

    public GameObject skillSetWindow;
    public GameObject traitWindow;

    private int selectedButtonIndex = 0;
    private int buttonCount;

    private UIObject[] UIData;
    private PrepareSceneUIData selectedUI;

    private float movePos;
    private float originPos;

    void Start()
    {
        buttonCount = skillSlots.Length;

        currentDepth = 0;

        originPos = 495;
        movePos = 406;

        UIData = new UIObject[skillSlots.Length];

        for (int i = 0; i < buttonCount; i++)
        {
            UIData[i] = skillSlots[i].GetComponent<UIObject>();
        }

        for (int i = 0; i < 3; i++)
        {
            skill[i] = GameObject.Find("M_Skill_" + (i + 1));
            skillDescript[i] = GameObject.Find("M_SkillDescript_" + (i + 1));
        }

        UIMatrics[0] = skillSlots;
        UIMatrics[1] = skill;
        UIMatrics[2] = skillDescript;
    }

    void Update()
    {
        ButtonsControl();
    }

    protected override void Init()
    {
        base.Init();

        SetSkill();
    }

    private void SetSlots(int curRow)
    {
        buttonCount = UIMatrics[curRow].Length;

        UIData = new UIObject[UIMatrics[curRow].Length];

        for (int i = 0; i < buttonCount; i++)
        {
            UIData[i] = UIMatrics[curRow][i].GetComponent<UIObject>();
        }
    }

    private void ButtonsControl()
    {
        SelectSlot();

        if (Input.GetKeyDown(KeyCode.A))
        {
            UIMatrics[currentDepth][selectedButtonIndex].GetComponent<Image>().sprite = selectedUI.unSelectedImage;
            selectedButtonIndex = (selectedButtonIndex - 1 + buttonCount) % buttonCount;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            UIMatrics[currentDepth][selectedButtonIndex].GetComponent<Image>().sprite = selectedUI.unSelectedImage;
            selectedButtonIndex = (selectedButtonIndex + 1) % buttonCount;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentDepth != 0)
            {
                UIMatrics[currentDepth][selectedButtonIndex].GetComponent<Image>().sprite = selectedUI.unSelectedImage;
                SetSlots(currentDepth - 1);
                currentDepth -= 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            UIMatrics[currentDepth][selectedButtonIndex].GetComponent<Image>().sprite = selectedUI.unSelectedImage;
            SetSlots(currentDepth + 1);
            currentDepth += 1;
        }

        for (int i = 0; i < UIMatrics[currentDepth].Length; i++)
        {
            if (i == selectedButtonIndex)
            {
                UIMatrics[currentDepth][i].GetComponent<Image>().sprite = selectedUI.selectedImage;
                //skillSlots[i].GetComponent<Image>().sprite = selectedUI.selectedImage;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            SkillSlotFunction();
        }
    }

    private void SkillSlotFunction()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            skillSlots[i].transform.DOLocalMoveY(originPos, 0.1f);
        }

        skillSlots[selectedButtonIndex].transform.DOLocalMoveY(movePos, 0.1f);

        switch (selectedButtonIndex)
        {
            case 0:
            case 2:
                SetSkill();
                break;
            case 1:
            case 3:
                SetTrait();
                break;
            default:
                Debug.LogError("Not Found Button");
                break;
        }
    }

    private void SelectSlot()
    {
        if (UIMatrics[currentDepth].Length - 1 <= selectedButtonIndex)
        {
            selectedButtonIndex = UIMatrics[currentDepth].Length - 1;
        }

        selectedUI = UIData[selectedButtonIndex].uiData;

        selectedSlotName.text = null;
        selectedSlotTitle.text = null;
        selectedSlotDiscription.text = null;

        if (selectedUI.type.ToString() == "Simple")
        {
            selectedSlotName.text = selectedUI.displayName;
        }
            
        else
        {
            selectedSlotTitle.text = selectedUI.displayName;
            selectedSlotDiscription.text = selectedUI.description;
        }

    }

    private void SetSkill()
    {
        traitWindow.SetActive(false);
        skillSetWindow.SetActive(true);
    }

    private void SetTrait()
    {
        skillSetWindow.SetActive(false);
        traitWindow.SetActive(true);
    }

    public override void Clear()
    {

    }
}

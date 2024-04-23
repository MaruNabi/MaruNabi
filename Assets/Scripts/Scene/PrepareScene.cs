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
    public GameObject skillBorderImage;
    public GameObject traitBorderImage;
    public GameObject descriptionTail;
    public TextMeshProUGUI selectedSlotName;
    public TextMeshProUGUI selectedSlotTitle;
    public TextMeshProUGUI selectedSlotDiscription;

    private GameObject[] skill = new GameObject[3];
    private GameObject[] skillDescript = new GameObject[3];
    private GameObject[][] UIMatrics = new GameObject[4][];
    private int currentSlot;
    private int currentDepth;
    private int currentSelectSkill;

    private Image[] skillSlotImage;
    private Image[] skillDescriptImage;

    public GameObject skillSetWindow;
    public GameObject traitWindow;

    private int selectedButtonIndex = 0;
    private int buttonCount;
    private bool isSkillSetWindow = true;
    private bool isSetPosAndSprite = false;

    private UIObject[] UIData;
    private PrepareSceneUIData selectedUI;

    private Color originColor = new Color(1, 1, 1, 1);
    private Color disableColor = new Color(1, 1, 1, 0);

    private float movePos;
    private float originPos;

    void Start()
    {
        buttonCount = skillSlots.Length;

        currentDepth = 0;
        originPos = 495;
        movePos = 406;
        skillBorderImage.SetActive(false);
        traitBorderImage.SetActive(false);
        descriptionTail.SetActive(false);

        UIData = new UIObject[skillSlots.Length];
        skillSlotImage = new Image[skillSlots.Length];
        skillDescriptImage = new Image[3];

        for (int i = 0; i < 3; i++)
        {
            skill[i] = GameObject.Find("M_Skill_" + (i + 1));
            skillDescript[i] = GameObject.Find("M_SkillDescript_" + (i + 1));
            skillDescriptImage[i] = skillDescript[i].transform.GetChild(0).GetComponent<Image>();
        }

        for (int i = 0; i < buttonCount; i++)
        {
            UIData[i] = skillSlots[i].GetComponent<UIObject>();
            skillSlotImage[i] = skillSlots[i].transform.GetChild(0).GetComponent<Image>();
            skillSlotImage[i].color = disableColor;
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

        if (Input.anyKeyDown)
            isSetPosAndSprite = false;

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
            if (currentDepth < UIMatrics.Length)
            {
                UIMatrics[currentDepth][selectedButtonIndex].GetComponent<Image>().sprite = selectedUI.unSelectedImage;
                SetSlots(currentDepth + 1);
                currentDepth += 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            switch (currentDepth)
            {
                case 0:
                    SkillSlotFunction();
                    break;
                case 1:
                    SkillFunction();
                    break;
                case 2:
                    break;
                default:
                    break;
            }
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
                currentSlot = selectedButtonIndex;
                SetSkill();
                break;
            case 1:
            case 3:
                currentSlot = selectedButtonIndex;
                SetTrait();
                break;
            default:
                Debug.LogError("Not Found Button");
                break;
        }
    }

    private void SkillFunction()
    {
        for (int i = 0; i < selectedUI.changeImagePoolDescript.Length; i++)
        {
            skillDescriptImage[i].GetComponent<Image>().sprite = selectedUI.changeImagePoolDescript[i];
        }

        skillSlotImage[currentSlot].GetComponent<Image>().sprite = selectedUI.changeImagePoolSlot;
        skillSlotImage[currentSlot].color = originColor;
        currentSelectSkill = selectedButtonIndex;
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

        if (!isSetPosAndSprite)
        {
            isSetPosAndSprite = true;

            if (isSkillSetWindow && currentDepth == 1)
            {
                skillBorderImage.transform.position = UIData[selectedButtonIndex].transform.position;
                skillBorderImage.SetActive(true);
            }
            else if (!isSkillSetWindow && currentDepth == 1 || !isSkillSetWindow && currentDepth == 2)
            {
                traitBorderImage.transform.position = UIData[selectedButtonIndex].transform.position;
                traitBorderImage.SetActive(true);
            }
            else
            {
                skillBorderImage.SetActive(false);
                traitBorderImage.SetActive(false);
                UIMatrics[currentDepth][selectedButtonIndex].GetComponent<Image>().sprite = selectedUI.selectedImage;
            }

            if (isSkillSetWindow && currentDepth == 2)
            {
                Vector3 temp = descriptionTail.transform.position;
                temp.x = UIData[selectedButtonIndex].transform.position.x;
                descriptionTail.transform.position = temp;
                descriptionTail.SetActive(true);
            }
            else
            {
                descriptionTail.SetActive(false);
            }
        }

        if (selectedUI.type.ToString() == "Simple")
        {
            selectedSlotName.text = selectedUI.displayName[0];
        }
        else
        {
            selectedSlotTitle.text = selectedUI.displayName[currentSelectSkill];
            selectedSlotDiscription.text = selectedUI.description[currentSelectSkill];
        }
    }

    private void SetSkill()
    {
        traitWindow.SetActive(false);
        skillSetWindow.SetActive(true);
        isSkillSetWindow = true;
    }

    private void SetTrait()
    {
        skillSetWindow.SetActive(false);
        traitWindow.SetActive(true);
        isSkillSetWindow = false;
    }

    public override void Clear()
    {

    }
}

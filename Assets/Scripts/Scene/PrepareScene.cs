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
    public GameObject[] readyButton = new GameObject[0];
    public GameObject skillBorderImage;
    public GameObject traitBorderImage;
    public GameObject descriptionTail;
    public TextMeshProUGUI selectedSlotName;
    public TextMeshProUGUI selectedSlotTitle;
    public TextMeshProUGUI selectedSlotDiscription;

    public GameObject pannel;
    public GameObject skillSetWindow;
    public GameObject traitWindow;

    private GameObject[] skill = new GameObject[3];
    private GameObject[] skillDescript = new GameObject[3];
    private GameObject[] trait_1 = new GameObject[4];
    private GameObject[] trait_2 = new GameObject[4];
    private GameObject[][] UIMatrics = new GameObject[4][];
    private int[] skillSlotSelect;
    private int currentSlot;
    private int currentDepth;
    private int currentSelectSkill;
    private int previousSlot;

    private Image[] skillSlotImage;
    private Image[] skillDescriptImage;

    private int selectedButtonIndex = 0;
    private int buttonCount;
    private bool isSkillSetWindow = true;
    private bool isSetPosAndSprite = false;
    private bool canReady = false;

    private UIObject[] UIData;
    private PrepareSceneUIData selectedUI;

    private Color originColor = new Color(1, 1, 1, 1);
    private Color disableColor = new Color(1, 1, 1, 0);

    private float skillSetMovePos;
    private float skillSetOriginPos;
    private float pannelMovePos;
    private float pannelOriginPos;

    void Start()
    {
        buttonCount = skillSlots.Length;

        currentDepth = 0;
        skillSetOriginPos = 495;
        skillSetMovePos = 406;
        pannelOriginPos = 35.87f;
        pannelMovePos = 874;
        skillBorderImage.SetActive(false);
        traitBorderImage.SetActive(false);
        descriptionTail.SetActive(false);

        UIData = new UIObject[skillSlots.Length];
        skillSlotImage = new Image[skillSlots.Length];
        skillSlotSelect = new int[skillSlots.Length];
        skillDescriptImage = new Image[3];

        for (int i = 0; i < skill.Length; i++)
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
        UIMatrics[3] = readyButton;
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
        {
            isSetPosAndSprite = false;
            if (currentDepth != 1)
            {
                UIMatrics[currentDepth][selectedButtonIndex].GetComponent<Image>().sprite = selectedUI.unSelectedImage;
            }
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
                SetSlots(currentDepth - 1);
                currentDepth -= 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentDepth < 3)
            {
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
                case 3:
                    ReadyButtonFunction();
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
            skillSlots[i].transform.DOLocalMoveY(skillSetOriginPos, 0.1f);
        }

        skillSlots[selectedButtonIndex].transform.DOLocalMoveY(skillSetMovePos, 0.1f);

        switch (selectedButtonIndex)
        {
            case 0:
            case 2:
                previousSlot = currentSlot;
                currentSlot = selectedButtonIndex;
                currentDepth = 0;
                SetSkill();
                break;
            case 1:
            case 3:
                previousSlot = currentSlot;
                currentSlot = selectedButtonIndex;
                currentDepth = 0;
                SetTrait();
                break;
            default:
                Debug.LogError("Not Found Button");
                break;
        }
    }

    private void SkillFunction()
    {
        if (isSkillSetWindow)
        {
            if (skillSlotSelect[2 - currentSlot] == selectedButtonIndex + 1)
                return;
        }

        if (selectedUI.displayName[0] == "ºó ½½·Ô")
            return;

        skillSlotSelect[currentSlot] = selectedButtonIndex + 1;

        for (int i = 0; i < selectedUI.changeImagePoolDescript.Length; i++)
        {
            skillDescriptImage[i].GetComponent<Image>().sprite = selectedUI.changeImagePoolDescript[i];
        }

        skillSlotImage[currentSlot].GetComponent<Image>().sprite = selectedUI.changeImagePoolSlot;
        skillSlotImage[currentSlot].color = originColor;
        currentSelectSkill = selectedButtonIndex;

        for (int i = 0; i < skill.Length; i++)
        {
            if (i == selectedButtonIndex)
            {
                UIMatrics[currentDepth][i].GetComponent<Image>().sprite = selectedUI.selectingImage;
            }
            else if (UIMatrics[currentDepth][i].GetComponent<Image>().sprite == selectedUI.selectingImage)
            {
                UIMatrics[currentDepth][i].GetComponent<Image>().sprite = selectedUI.unSelectedImage;
            }
        }

        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (skillSlotSelect[i] == 0)
                canReady = false;
            else
                canReady = true;
        }
    }

    private void ReadyButtonFunction()
    {
        if (!canReady)
            return;
        else
        {
            //.SetEase(Ease.InOutBack)
            pannel.transform.DOLocalMoveY(pannelMovePos, 0.5f);
            Debug.Log(UIMatrics[currentDepth][0]);
            UIMatrics[currentDepth][0].GetComponent<Image>().sprite = selectedUI.disableImage;

            for (int i = 0; i < skillSlotSelect.Length; i++)
            {
                //player data send
            }
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
        else if (!isSkillSetWindow)
        {
            selectedSlotTitle.text = selectedUI.displayName[0];
            selectedSlotDiscription.text = selectedUI.description[0];
        }
        else
        {
            selectedSlotTitle.text = selectedUI.displayName[currentSelectSkill];
            selectedSlotDiscription.text = selectedUI.description[currentSelectSkill];
        }
    }

    private void SetSkill()
    {
        if (previousSlot == currentSlot)
            return;

        traitWindow.SetActive(false);
        skillSetWindow.SetActive(true);
        isSkillSetWindow = true;

        UIMatrics[0] = skillSlots;
        UIMatrics[1] = skill;
        UIMatrics[2] = skillDescript;

        for (int i = 0; i < skill.Length; i++)
        {
            skill[i] = GameObject.Find("M_Skill_" + (i + 1));
            skillDescript[i] = GameObject.Find("M_SkillDescript_" + (i + 1));
            skillDescriptImage[i] = skillDescript[i].transform.GetChild(0).GetComponent<Image>();
        }

        if (skillSlotSelect[2- currentSlot] != 0)
        {
            UIMatrics[1][skillSlotSelect[2 - currentSlot] - 1].GetComponent<Image>().sprite = selectedUI.disableImage;
        }

        if (skillSlotSelect[currentSlot] != 0)
        {
            UIMatrics[1][skillSlotSelect[currentSlot] - 1].GetComponent<Image>().sprite = selectedUI.selectingImage;
        }
    }

    private void SetTrait()
    {
        if (previousSlot == currentSlot)
            return;

        skillSetWindow.SetActive(false);
        traitWindow.SetActive(true);
        isSkillSetWindow = false;

        UIMatrics[0] = skillSlots;
        UIMatrics[1] = trait_1;
        UIMatrics[2] = trait_2;

        for (int i = 0; i < trait_1.Length; i++)
        {
            trait_1[i] = GameObject.Find("M_Trait_" + (i + 1));
            trait_2[i] = GameObject.Find("M_Trait_" + (i + 5));
        }

        if (skillSlotSelect[currentSlot] != 0)
        {
            UIMatrics[1][skillSlotSelect[currentSlot] - 1].GetComponent<Image>().sprite = selectedUI.selectingImage;
        }

        if (skillSlotSelect[4 - currentSlot] != 0)
        {
            UIMatrics[1][skillSlotSelect[4 - currentSlot] - 1].GetComponent<Image>().sprite = selectedUI.disableImage;
        }
    }

    public override void Clear()
    {

    }
}

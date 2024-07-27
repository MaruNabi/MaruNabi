using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstInformationUI : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;
    public Image descriptionImage;
    public Image arrowImage;

    [SerializeField][TextArea] private string[] descriptionArr;
    [SerializeField] private Sprite[] descriptionSprite;
    private Sprite[] arrowSprite = new Sprite[4];
    private int currentPage;

    public bool IsActiveInformation { get; private set; }

    void Start()
    {
        if (descriptionArr.Length != descriptionSprite.Length)
            Debug.LogError("Array length is different!!");

        for (int i = 0; i < arrowSprite.Length; i++)
        {
            arrowSprite[i] = Resources.Load<Sprite>("UI/InformationUI/Arrow/Guidance_" + i);
        }

        IsActiveInformation = true;
        currentPage = 0;

        descriptionText.text = descriptionArr[0];
        descriptionImage.sprite = descriptionSprite[0];
        ArrowImageSet();
    }

    void Update()
    {
        if (IsActiveInformation)
        {
            PageTransition();
        }
        else
            return;
    }

    private void PageTransition()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (currentPage == 0)
                return;
            else
            {
                currentPage -= 1;
                descriptionText.text = descriptionArr[currentPage];
                descriptionImage.sprite = descriptionSprite[currentPage];
                ArrowImageSet();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (currentPage == descriptionArr.Length - 1)
            {
                IsActiveInformation = false;
                gameObject.SetActive(false);
            }
            else
            {
                currentPage += 1;
                descriptionText.text = descriptionArr[currentPage];
                descriptionImage.sprite = descriptionSprite[currentPage];
                ArrowImageSet();
            }
        }
    }

    private void ArrowImageSet()
    {
        if (currentPage == 0)
        {
            arrowImage.sprite = arrowSprite[0];
        }
        else if (currentPage == descriptionArr.Length - 1)
        {
            arrowImage.sprite = arrowSprite[2];
        }
        else
        {
            arrowImage.sprite = arrowSprite[1];
        }

        if (descriptionArr.Length == 1)
            arrowImage.sprite = arrowSprite[3];
    }
}

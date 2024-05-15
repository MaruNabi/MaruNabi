using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerSkillChange : MonoBehaviour
{
    [SerializeField] private bool isMaru;
    private Sprite originImage;
    private Sprite shineImage;
    private Sprite changeImage;
    private Image skillChangeUI;
    private Vector2 originPos;
    private Vector2 movePos;

    void Start()
    {
        if (isMaru)
        {
            originImage = Resources.Load<Sprite>("Images/UI/PlayerSkillChange/UI_Skill_Change_Maru_1");
            shineImage = Resources.Load<Sprite>("Images/UI/PlayerSkillChange/UI_Skill_Change_Maru_S");
            changeImage = Resources.Load<Sprite>("Images/UI/PlayerSkillChange/UI_Skill_Change_Maru_2");
            movePos = new Vector2(-960, -540);
        }
        else
        {
            originImage = Resources.Load<Sprite>("Images/UI/PlayerSkillChange/UI_Skill_Change_Nabi_1");
            shineImage = Resources.Load<Sprite>("Images/UI/PlayerSkillChange/UI_Skill_Change_Nabi_S");
            changeImage = Resources.Load<Sprite>("Images/UI/PlayerSkillChange/UI_Skill_Change_Nabi_2");
            movePos = new Vector2(960, -540);
        }

        skillChangeUI = GetComponent<Image>();
        skillChangeUI.sprite = originImage;
        originPos = transform.localPosition;
    }

    public void SkillChangeImage()
    {
        StartCoroutine(SkillChangImageMovement());
    }

    private IEnumerator SkillChangImageMovement()
    {
        transform.DOLocalMove(movePos, 0.4f);
        yield return new WaitForSeconds(0.8f);
        skillChangeUI.sprite = shineImage;
        yield return new WaitForSeconds(0.2f);
        skillChangeUI.sprite = changeImage;
        yield return new WaitForSeconds(0.5f);
        transform.DOLocalMove(originPos, 0.4f);
        yield return new WaitForSeconds(0.4f);
        skillChangeUI.sprite = originImage;
    }
}

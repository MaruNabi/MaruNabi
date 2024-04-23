using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DialogueType
{
    Simple,
    TitleAndSub
}

[CreateAssetMenu(fileName = "UI", menuName = "New UI")]
public class PrepareSceneUIData : ScriptableObject
{
    [Header("Info")]
    public string[] displayName;
    [TextArea]
    public string[] description;
    public DialogueType type;
    public Sprite selectedImage;        //border
    public Sprite unSelectedImage;      //normal
    public Sprite selectingImage;       //now selected in skill set
    public Sprite disableImage;         //before selected

    [Header("ChangeSkillIcon")]
    public Sprite changeImagePoolSlot;
    public Sprite[] changeImagePoolDescript;
}
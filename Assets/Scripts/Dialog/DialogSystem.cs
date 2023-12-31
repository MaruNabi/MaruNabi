using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private DialogTestManager dialogTestManager;

    [Header("Dialog UI")]

    [SerializeField]
    private DialogSystemUIInfo maruCharacter;

    [SerializeField]
    private DialogSystemUIInfo nabiCharacter;

    [SerializeField]
    private DialogSystemUIInfo godCharacter;

    [SerializeField]
    private Color unActiveCharacterColor;
    [SerializeField]
    private Color activeCharacterColor;

    [SerializeField]
    private DialogSystemUIInfo character;

    [SerializeField]
    private TextAsset[] TextAssets;

    private List<List<DialogData>> dialogData;

    private int dataIndex = 0;

    private int dialogDataIndex = 0;

    private bool isTalking = false;

    private TMP_Text targetTextBox;

    private string currentDialog;

    private Coroutine typingTextCoroutine;

    private float typingSpeed = 0.1f;

    public UnityEvent onDialogEnd;

    void Start()
    {
        this.InitializeDialogScriptData();

        this.isTalking = true;

        this.SetDialogUI(this.dataIndex, this.dialogDataIndex);
    }

    private void InitializeDialogScriptData()
    {
        this.dialogData = new List<List<DialogData>>();

        foreach (TextAsset textAsset in this.TextAssets)
        {
            List<Dictionary<string, object>> scriptData = CSVReader.Read(textAsset);

            var dialogData = new List<DialogData>();

            for (int i = 0; i < scriptData.Count; i++)
            {
                dialogData.Add(new DialogData((int)scriptData[i]["talker"], 
                scriptData[i]["name"].ToString(), 
                scriptData[i]["content"].ToString()));
            }

            this.dialogData.Add(dialogData);
        }
    }

    public void DialogNextButtonClicked()
    {
        if(this.isTalking == true)
        {
            this.StopTextCoroutine();
            this.isTalking = false;
        }
        else if(isTalking == false)
        {
            this.isTalking = true;
            this.NextDialogLoad();
        }
    }

    private void NextDialogLoad()
    {
        if (this.dialogDataIndex + 1 >= this.dialogData[this.dataIndex].Count)
        {
            Debug.Log("End~");

            StartCoroutine(dialogTestManager.CutScene());

            onDialogEnd?.Invoke();

            if (this.dataIndex + 1 < this.dialogData.Count)
            {
                this.dataIndex++;
                this.dialogDataIndex = 0;

                this.SetDialogUI(this.dataIndex, this.dialogDataIndex);
            }
        }
        else
        {
            this.dialogDataIndex++;

            this.SetDialogUI(this.dataIndex, this.dialogDataIndex);
        }
    }

    private void SetDialogUI(int dataIndex, int index)
    {
        DialogSystemUIInfo selectedUIInfo = this.maruCharacter;
        DialogSystemUIInfo opponentUIInfo1 = this.maruCharacter;
        DialogSystemUIInfo opponentUIInfo2 = this.maruCharacter;

        // 0 maru, 1 nabi, 2 god
        int selectedIdx = this.dialogData[dataIndex][index].activeTalker;
        if(selectedIdx == 0) 
        {
            selectedUIInfo = this.maruCharacter;
            opponentUIInfo1 = this.nabiCharacter;
            opponentUIInfo2 = this.godCharacter;
        }
        else if(selectedIdx == 1)
        {
            selectedUIInfo = this.nabiCharacter;
            opponentUIInfo1 = this.maruCharacter;
            opponentUIInfo2 = this.godCharacter;
        }
        else if(selectedIdx == 2)
        {
            selectedUIInfo = this.godCharacter;
            opponentUIInfo1 = this.maruCharacter;
            opponentUIInfo2 = this.nabiCharacter;
        }

        selectedUIInfo.characterImage.color = this.activeCharacterColor;

        opponentUIInfo1.characterImage.color = this.unActiveCharacterColor;
        opponentUIInfo2.characterImage.color = this.unActiveCharacterColor;

        selectedUIInfo.talkerNameText.text = this.dialogData[dataIndex][index].talkerName;

        selectedUIInfo.contentText.text = "";

        this.currentDialog = this.dialogData[dataIndex][index].content;
        this.targetTextBox = selectedUIInfo.contentText;
        this.typingTextCoroutine = StartCoroutine(typingText(currentDialog, targetTextBox));
    }

    private void StopTextCoroutine()
    {
        if (this.typingTextCoroutine != null)
        {
            StopCoroutine(this.typingTextCoroutine);
            this.targetTextBox.text = currentDialog;
        }
    }

    IEnumerator typingText(string text, TMP_Text targetText)
    {
        int count = 0;
        int length = text.Length;
        while (count != length)
        {
            if (count < length)
            {
                targetText.text += text[count].ToString();
                count++;
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        this.isTalking = false;
    }
}

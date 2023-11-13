using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    [Header("Dialog UI")]

    [SerializeField]
    private DialogSystemUIInfo maruCharacter;

    [SerializeField]
    private DialogSystemUIInfo nabiCharacter;

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
                dialogData.Add(new DialogData((int)scriptData[i]["talker"], scriptData[i]["name"].ToString(), scriptData[i]["content"].ToString()));
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
            this.isTalking = false;
            Debug.Log("End~"); // 대화 끝난 이후 로직 추가
            return;
        }
        else
        {
            this.dialogDataIndex++;

            this.SetDialogUI(this.dataIndex, this.dialogDataIndex);
        }
    }

    private void SetDialogUI(int dataIndex, int index)
    {
        // 0 maru, 1 nabi
        DialogSystemUIInfo selectedUIInfo = this.dialogData[dataIndex][index].activeTalker == 0 ? this.maruCharacter : this.nabiCharacter;
        DialogSystemUIInfo opponentUIInfo = this.dialogData[dataIndex][index].activeTalker == 0 ? this.nabiCharacter : this.maruCharacter;

        selectedUIInfo.characterImage.color = this.activeCharacterColor;

        opponentUIInfo.characterImage.color = this.unActiveCharacterColor;

        selectedUIInfo.talkerNameText.text = this.dialogData[dataIndex][index].talkerName;

        selectedUIInfo.contentText.text = "";

        this.currentDialog = this.dialogData[dataIndex][index].content;
        this.targetTextBox = selectedUIInfo.contentText;
        this.typingTextCoroutine = StartCoroutine(typingText(currentDialog, targetTextBox));
    }

    private void StopTextCoroutine()
    {
        if (typingTextCoroutine != null)
        {
            StopCoroutine(typingTextCoroutine);
            targetTextBox.text = currentDialog;
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

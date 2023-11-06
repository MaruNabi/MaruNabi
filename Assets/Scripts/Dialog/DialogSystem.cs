using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private DialogSystemUIInfo character;

    [SerializeField]
    private TextAsset[] TextAssets;

    private List<List<DialogData>> dialogData;

    private int dataIndex = 0;

    private int dialogDataIndex = 0;

    void Start()
    {
        this.InitializeDialogScriptData();

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
        if (this.dialogDataIndex + 1 >= this.dialogData[this.dataIndex].Count)
        {
            Debug.Log("End~"); // 대화 끝난 이후 로직 추가
        }
        else
        {
            this.dialogDataIndex++;

            this.SetDialogUI(this.dataIndex, this.dialogDataIndex);
        }
    }

    private void SetDialogUI(int dataIndex, int index)
    {
        DialogSystemUIInfo dialogUIInfo = character;

        dialogUIInfo.talkerNameText.text = this.dialogData[dataIndex][index].talkerName;

        dialogUIInfo.contentText.text = this.dialogData[dataIndex][index].content;
    }
}

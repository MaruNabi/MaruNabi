using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogData
{
    [SerializeField] public string talkerName { get; private set; }

    [SerializeField] public string content { get; private set; }

    public DialogData(string talkerName, string content)
    {
        this.talkerName = talkerName;
        this.content = content;
    }
}

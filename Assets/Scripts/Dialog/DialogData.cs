using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogData
{
    [SerializeField] public int activeTalker { get; private set; }

    [SerializeField] public string talkerName { get; private set; }

    [SerializeField] public string content { get; private set; }

    public DialogData(int activeTalker, string talkerName, string content)
    {
        this.activeTalker = activeTalker;
        this.talkerName = talkerName;
        this.content = content;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogData
{
    [SerializeField] public int activeTalker { get; private set; }

    [SerializeField] public string talkerName { get; private set; }

    [SerializeField] public string content { get; private set; }

    [SerializeField] public int cutscene { get; private set; }

    [SerializeField] public int background { get; private set; }
    public DialogData(int activeTalker, string talkerName, string content, int cutscene, int background)
    {
        this.activeTalker = activeTalker;
        this.talkerName = talkerName;
        this.content = content;
        this.cutscene = cutscene;
        this.background = background;
    }
}

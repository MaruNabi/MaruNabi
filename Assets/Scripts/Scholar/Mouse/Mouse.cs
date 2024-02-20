using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class Mouse : LivingEntity
{
    private MouseStateMachine mouseStateMachine;

    public MouseManager mouseManager;

    private TMP_Text hpTextBox;

    public bool isIdle = false;

    public bool IsIdle
    {
        get { return isIdle; }
        set { isIdle = value; }
    }

    private void Awake()
    {
        this.mouseStateMachine = this.gameObject.AddComponent<MouseStateMachine>();

        Transform mouseCanvas = transform.GetChild(0);
        Transform textMeshProTransform = mouseCanvas.GetChild(0);
        this.hpTextBox = textMeshProTransform.GetComponent<TextMeshProUGUI>();

        // TO DO: HP 다른 곳에서 관리
        this.startingHP = 999999999;
    }

    private void Start()
    {
        this.mouseStateMachine.Initialize("Idle", this);
        this.mouseStateMachine.Initialize("Appearance", this);
        this.hpTextBox.text = HP.ToString();
    }
}

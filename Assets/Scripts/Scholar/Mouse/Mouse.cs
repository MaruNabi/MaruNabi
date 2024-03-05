using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public partial class Mouse : LivingEntity
{
    private MouseStateMachine mouseStateMachine;

    public MouseManager mouseManager;

    private void Awake()
    {
        this.mouseStateMachine = this.gameObject.AddComponent<MouseStateMachine>();

        this.mouseSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        Transform mouseCanvas = transform.GetChild(0);
        Transform textMeshProTransform = mouseCanvas.GetChild(0);
        this.hpTextBox = textMeshProTransform.GetComponent<TextMeshProUGUI>();

        // TO DO: HP 다른 곳에서 관리
        this.startingHP = 999999999;

        this.mouseManager = transform.parent.GetComponent<MouseManager>();
    }

    private void Start()
    {
        this.mouseStateMachine.Initialize("Appearance", this);
        this.hpTextBox.text = HP.ToString();
    }
}

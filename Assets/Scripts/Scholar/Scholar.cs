using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public partial class Scholar : LivingEntity
{
    private ScholarStateMachine scholarStateMachine;

    private ScholarManager scholarManager;

    private void Awake()
    {
        this.scholarStateMachine = this.gameObject.AddComponent<ScholarStateMachine>();

        this.scholarSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        Transform scholarCanvas = transform.GetChild(0);
        Transform textMeshProTransform = scholarCanvas.GetChild(0);
        this.hpTextBox = textMeshProTransform.GetComponent<TextMeshProUGUI>();

        this.startingHP = 999999999;

        this.scholarManager = transform.parent.GetComponent<ScholarManager>();
    }

    private GameObject GetChild(int v)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        this.scholarStateMachine.Initialize("Idle", this);
        this.scholarStateMachine.Initialize("Appearance", this);

        Debug.Log(HP);
        this.hpTextBox.text = HP.ToString();
    }

    void Update()
    {

    }
}
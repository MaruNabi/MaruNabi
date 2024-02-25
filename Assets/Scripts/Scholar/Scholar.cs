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

    public ScholarManager scholarManager;

    private void Awake()
    {
        this.scholarStateMachine = this.gameObject.AddComponent<ScholarStateMachine>();

        this.scholarSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        Transform scholarCanvas = transform.GetChild(0);
        Transform textMeshProTransform = scholarCanvas.GetChild(0);
        this.hpTextBox = textMeshProTransform.GetComponent<TextMeshProUGUI>();

        // TO DO: HP 다른 곳에서 관리
        this.startingHP = 999999999;

        this.scholarManager = transform.parent.GetComponent<ScholarManager>();
    }

    private GameObject GetChild(int v)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        this.scholarStateMachine.Initialize("Appearance", this);

        // TO DO: UI 로직 분리 (delegate 이용)
        this.hpTextBox.text = HP.ToString();
    }

    void Update()
    {

    }
}
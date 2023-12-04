using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class Scholar : LivingEntity
{
    private ScholarStateMachine scholarStateMachine;

    private void Awake()
    {
        this.scholarStateMachine = this.gameObject.AddComponent<ScholarStateMachine>();

        this.scholarSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        this.scholarStateMachine.Initialize("Idle", this);
        this.scholarStateMachine.Initialize("Appearance", this);

    }

    void Update()
    {
        
    }



}

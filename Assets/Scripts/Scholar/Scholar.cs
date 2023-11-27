using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Scholar : LivingEntity
{
    private ScholarStateMachine scholarStateMachine;

    private void Awake()
    {
        this.scholarStateMachine = this.gameObject.AddComponent<ScholarStateMachine>();

        scholarSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
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

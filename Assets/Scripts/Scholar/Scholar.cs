using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class Scholar : LivingEntity
{
    private ScholarStateMachine scholarStateMachine;

    public static UnityEvent OnAppearance;

    public ScholarManager scholarManager; 

    private void Awake()
    {
        this.scholarStateMachine = this.gameObject.AddComponent<ScholarStateMachine>();

        this.scholarSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        this.scholarManager = GameObject.Find("ScholarManager").GetComponent<ScholarManager>();
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

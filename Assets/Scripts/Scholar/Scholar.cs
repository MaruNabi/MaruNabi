using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scholar : LivingEntity
{
    private ScholarStateMachine scholarStateMachine;

    public Scholar(GameObject obj, Transform trans)
    {
        base.CreateObject(obj, trans);
    }

    private void Awake()
    {
        this.scholarStateMachine = this.gameObject.AddComponent<ScholarStateMachine>();
    }
    void Start()
    {
        this.scholarStateMachine.Initialize("Idle", this);

    }

    void Update()
    {
        
    }

}

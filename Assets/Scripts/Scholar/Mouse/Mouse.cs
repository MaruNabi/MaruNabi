using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : LivingEntity
{
    private MouseStateMachine mouseStateMachine;


    private void Awake()
    {
        this.mouseStateMachine = this.gameObject.AddComponent<MouseStateMachine>();
    }

    private void Start()
    {
        this.mouseStateMachine.Initialize("Idle", this);
    }
}

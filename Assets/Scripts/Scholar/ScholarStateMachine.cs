using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScholarStateMachine : StateMachine<ScholarStateMachine>
{
    private Scholar scholar;

    public Scholar Scholar{
        get { 
            return this.scholar; }
    }

    private void Awake()
    {
        base.states = new Dictionary<string, State<ScholarStateMachine>>{
            {"Idle",new ScholarIdle(this)},
            {"Appearance",new ScholarAppearance(this)},
            {"Fan",new ScholarFan(this)},
        };
    }

    public void Initialize(string stateName, Scholar scholar)
    {
        Debug.Log(stateName);
        this.scholar = scholar;
        base.Initialize(stateName);
    }
}

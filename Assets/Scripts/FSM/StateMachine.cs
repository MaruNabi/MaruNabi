using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StateName = System.String;
public abstract class StateMachine<T> : MonoBehaviour where T : StateMachine<T>
{
    public State<T> currentState;
    protected Dictionary<StateName, State<T>> states;

    private bool isRunning;

    public bool IsRunning
    {
        get { return this.isRunning; }
        set { this.isRunning = value; }
    }

    public virtual void Initialize(string stateName)
    {
        if (!this.states.ContainsKey(stateName))
            return;

        this.currentState = this.states[stateName];
        this.isRunning = true;
        this.currentState.OnEnter();
    }

    void Update()
    {

        if (this.currentState == null || this.isRunning == false)
            return;

        this.currentState?.OnUpdate();
    }

    private void FixedUpdate()
    {
        if (this.currentState == null || this.IsRunning == false)
            return;

        this.currentState?.OnFixedUpdate(); ;
    }


    private void LateUpdate()
    {
        if (this.currentState == null || this.IsRunning == false)
            return;

        this.currentState?.OnLateUpdate(); ;
    }

    public void SetState(string stateName)
    {
        Debug.Log(stateName);
        if (this.currentState == null || this.isRunning == false)
            return;

        if (!this.states.ContainsKey(stateName))
            return;

        this.currentState.OnExit();
        this.currentState = this.states[stateName];
        this.currentState.OnEnter();
    }

    public void AddState(string stateName, State<T> state)
    {
        if (this.currentState == null || this.isRunning == false)
            return;

        this.currentState.OnExit();
        this.states[stateName] = state;
        this.currentState = this.states[stateName];
        this.currentState.OnEnter();
    }
}

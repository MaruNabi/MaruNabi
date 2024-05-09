using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateName = System.String;

public abstract class StateMachine<T> : MonoBehaviour where T : StateMachine<T>
{
    public State<T> currentState;
    protected Dictionary<StateName, State<T>> states;

    private bool isRunning;

    public bool IsRunning { get { return isRunning; } }

    public virtual void Initialize(string _stateName)
    {
        if (states.ContainsKey(_stateName) == false)
            return;

        currentState = states[_stateName];
        isRunning = true;
        currentState.OnEnter();
    }

    void Update()
    {
        if (currentState == null || isRunning == false)
            return;

        currentState?.OnUpdate();
    }

    private void FixedUpdate()
    {
        if (currentState == null || IsRunning == false)
            return;

        currentState?.OnFixedUpdate();
    }


    private void LateUpdate()
    {
        if (currentState == null || IsRunning == false)
            return;

        currentState?.OnLateUpdate(); ;
    }

    public void SetState(string _stateName)
    {
        if (currentState == null || isRunning == false)
            return;

        if (states.ContainsKey(_stateName) == false)
            return;

        currentState.OnExit();
        currentState = states[_stateName];
        currentState.OnEnter();
    }

    public void AddState(string _stateName, State<T> _state)
    {
        if (currentState == null || isRunning == false)
            return;

        currentState.OnExit();
        states[_stateName] = _state;
        
        currentState = states[_stateName];
        currentState.OnEnter();
    }
    
    public void Stop()
    {
        if (currentState == null || isRunning == false)
            return;

        currentState.OnExit();
    }
}

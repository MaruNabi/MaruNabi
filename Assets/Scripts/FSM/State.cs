using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    protected T stateMachine;

    public State(T _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    virtual public void OnEnter()
    {

    }
    virtual public void OnFixedUpdate()
    {

    }
    virtual public void OnUpdate()
    {

    }
    virtual public void OnLateUpdate()
    {

    }
    virtual public void OnExit()
    {

    }
}

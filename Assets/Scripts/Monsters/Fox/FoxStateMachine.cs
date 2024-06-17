using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxStateMachine : StateMachine<FoxStateMachine>
{
    public Fox Fox => fox;
    private Fox fox;
    
    private void Awake()
    {
        base.states = new Dictionary<string, State<FoxStateMachine>>{
            {"Enter",new FoxEnterState(this)}
        };
    }
    
    public void Initialize(string _stateName, Fox _fox)
    {
        fox = _fox;
        base.Initialize(_stateName);
    }
}

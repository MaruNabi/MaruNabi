using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxStateMachine : StateMachine<FoxStateMachine>
{
    public Fox Fox => fox;
    private Fox fox;
    
    public void Initialize(string _stateName, Fox _fox)
    {
        fox = _fox;
        base.Initialize(_stateName);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class TigerStateMachine : StateMachine<TigerStateMachine>
{
    public Tiger tiger { get; private set; }
    private Animator animator;

    private void Awake()
    {
        base.states = new Dictionary<string, State<TigerStateMachine>>{
            {"Enter",new TigerEnterState(this)},
            {"Phase1",new TigerPhase1State(this)},
        };
    }

    public void Initialize(string _stateName, Tiger _tiger)
    {
        tiger = _tiger;
        base.Initialize(_stateName);
    }
}

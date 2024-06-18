using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class TigerStateMachine : StateMachine<TigerStateMachine>
{
    public Tiger tiger { get; private set; }

    private void Awake()
    {
        base.states = new Dictionary<string, State<TigerStateMachine>>{
            {"Phase1",new TigerPhase1State(this)},
            {"Phase2",new TigerPhase2State(this)},
            {"Phase3",new TigerPhase3State(this)},
            {"SideAtk1",new TigerSideAttackState(this)},
            {"SideAtk2",new TigerSideAttack2State(this)},
            {"SlapAtk",new TigerSlapAttackState(this)},
            {"InhaleAtk",new TigerInhaleAttackState(this)},
            {"DiagonalAtk",new TigerDiagonalAttackState(this)},
            {"BiteAtk",new TigerBiteAttackState(this)},
            {"PhaseChange",new TigerPhaseChange(this)},
            {"StageClear",new TigerStageClearState(this)},
        };
    }

    public void Initialize(string _stateName, Tiger _tiger)
    {
        tiger = _tiger;
        base.Initialize(_stateName);
    }
}

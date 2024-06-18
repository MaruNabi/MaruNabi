using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TigerPhase3State : TigerState
{
    public TigerPhase3State(TigerStateMachine tigerStateMachine) : base(tigerStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        IdleWait().Forget();
    }

    private async UniTaskVoid IdleWait()
    {
        if(stateMachine.tiger.CheckMiniPhaseChangeHP())
        {
            // �� ����ġ�� or ���Ƶ��̱�
            int random = UnityEngine.Random.Range(0, 2);
            if(random == 0)
            {
                stateMachine.SetState("SlapAtk");
            }
            else
            {
                stateMachine.SetState("InhaleAtk");
            }
        }
        else
        {
            stateMachine.SetState("SideAtk2");
        }
        // ������ 3 ����
    }
}
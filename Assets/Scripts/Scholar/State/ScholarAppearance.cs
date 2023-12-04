using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScholarAppearance : ScholarState
{
    private float elapsedTime = 0f;

    private float escapeTime = 1f;
    public ScholarAppearance(ScholarStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.stateMachine.StartCoroutine(this.stateMachine.Scholar.AppearanceCoroutine());
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        this.elapsedTime += Time.deltaTime;

        if (this.elapsedTime >= this.escapeTime)
        {
            this.stateMachine.SetState("Idle");
        }
    }
    public override void OnExit()
    {
        base.OnExit();

        this.elapsedTime = 0f;
    }
}

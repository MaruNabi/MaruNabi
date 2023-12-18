using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScholarFan : ScholarState
{
    private float elapsedTime = 0f;

    private float escapeTime = 5f;

    private Transform scholarTransform;

    private Vector3 scholarPos;

    private GameObject fan;

    public ScholarFan(ScholarStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ºÎÃ¤");

        this.scholarTransform = this.stateMachine.Scholar.transform;

        this.scholarPos = scholarTransform.position;

        this.fan = this.stateMachine.Scholar.scholarManager.MakeFan(scholarPos);
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        this.elapsedTime += Time.deltaTime;

        if (this.elapsedTime >= this.escapeTime)
        {
            this.stateMachine.Scholar.scholarManager.DestroyFan(this.fan);

            this.stateMachine.SetState("Idle");
        }
    }
    public override void OnExit()
    {
        base.OnExit();
    }
}

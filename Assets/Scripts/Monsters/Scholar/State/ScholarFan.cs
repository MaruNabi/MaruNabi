using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScholarFan : ScholarState
{
    private float elapsedTime = 0f;
    private float escapeTime = 3f;

    private float fanSpeed = 3f;
    private float scaleSpeed = 100f;

    private Transform scholarTransform;

    private Vector3 scholarPos;
    private Vector3 playerPos;

    private GameObject fan;
    private GameObject player;

    public ScholarFan(ScholarStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("��ä");

        this.scholarTransform = this.stateMachine.Scholar.transform;
        this.scholarPos = scholarTransform.position;

        Debug.Log("��ġ: " + this.scholarPos);

        this.fan = this.stateMachine.Scholar.scholarManager.MakeFan(scholarPos);

        this.player = this.stateMachine.Scholar.scholarManager.GetPlayer();
        this.playerPos = this.player.transform.position;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        this.elapsedTime += Time.deltaTime;

        //this.fan.transform.Translate(playerPos * Time.deltaTime * this.fanSpeed);
        // Vector3 currentScale = fan.transform.localScale;
        // currentScale += new Vector3(scaleSpeed, scaleSpeed, 0) * Time.deltaTime;
        // this.fan.transform.localScale = currentScale;

        if (this.elapsedTime >= this.escapeTime)
        {
            this.stateMachine.Scholar.scholarManager.DestroyFan(this.fan);

            this.stateMachine.SetState("Appearance");
        }
    }
    public override void OnExit()
    {
        base.OnExit();

        this.elapsedTime = 0f;

        this.stateMachine.Scholar.scholarManager.SetSchloarBehit(false);

        if (this.stateMachine.Scholar.HP <= 0 && !this.stateMachine.Scholar.dead)
        {
            this.stateMachine.Scholar.scholarManager.SetDeathMonster();
        }

        this.stateMachine.Scholar.Dead();
    }
}

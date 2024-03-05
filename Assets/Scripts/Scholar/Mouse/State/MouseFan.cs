using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFan : MouseState
{
    private float elapsedTime = 0f;
    private float escapeTime = 3f;

    private float fanSpeed = 3f;
    private float scaleSpeed = 7f;

    private Transform mouseTransform;

    private Vector3 mousePos;
    private Vector3 playerPos;

    private GameObject fan;
    private GameObject player;

    public MouseFan(MouseStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("부채");

        this.mouseTransform = this.stateMachine.Mouse.transform;
        this.mousePos = mouseTransform.position;

        Debug.Log("위치: " + this.mousePos);

        this.fan = this.stateMachine.Mouse.mouseManager.MakeFan(mousePos);

        this.player = this.stateMachine.Mouse.mouseManager.GetPlayer();
        this.playerPos = this.player.transform.position;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        this.elapsedTime += Time.deltaTime;

        this.fan.transform.Translate(playerPos * Time.deltaTime * this.fanSpeed);

        Vector3 currentScale = fan.transform.localScale;
        currentScale += new Vector3(scaleSpeed, scaleSpeed, 0) * Time.deltaTime;
        this.fan.transform.localScale = currentScale;


        if (this.elapsedTime >= this.escapeTime)
        {
            this.stateMachine.Mouse.mouseManager.DestroyFan(this.fan);

            this.stateMachine.SetState("Appearance");
        }
    }
    public override void OnExit()
    {
        base.OnExit();

        this.elapsedTime = 0f;

        this.stateMachine.Mouse.mouseManager.SetMouseBehit(false);

        if (this.stateMachine.Mouse.HP <= 0 && !this.stateMachine.Mouse.dead)
        {
            this.stateMachine.Mouse.Dead();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaru : Player
{
    void Start()
    {
        characterID = true;
        characterName = "Maru";
        cLife = 3;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimation = GetComponent<Animator>();

        ultimateGauge = 0.0f;
        maxUltimateGauge = 3.0f;
    }

    void Update()
    { 
        if (isDashing)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) && !isJumping)
        {
            PlayerJump(cMiniJumpPower);
            isJumpingEnd = false;
        }

        if (Input.GetKey(KeyCode.W) && !isJumpingEnd)
        {
            if (cMiniJumpPower < cMaxJumpPower)
            {
                PlayerJumping(cJumpPower);
                cMiniJumpPower += cJumpPower;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //Attack
        }

        if (Input.GetKeyDown(KeyCode.G) && canDash)
        {
            StartCoroutine(Dash());
        }

        //Animation Script
        if (rigidBody.velocity.normalized.x == 0)
        {
            playerAnimation.SetBool("isRunning", false);
        }
        else
        {
            playerAnimation.SetBool("isRunning", true);
        }

        if (rigidBody.velocity.normalized.y > 0)
        {
            playerAnimation.SetBool("isUp", true);
            playerAnimation.SetBool("isDown", false);
        }
        else if (rigidBody.velocity.normalized.y < 0)
        {
            playerAnimation.SetBool("isUp", false);
            playerAnimation.SetBool("isDown", true);
        }
        else
        {
            playerAnimation.SetBool("isUp", false);
            playerAnimation.SetBool("isDown", false);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        PlayerMove();
    }

    protected override void PlayerMove()
    {
        moveHorizontal = 0.0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1.0f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1.0f;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        base.PlayerMove();
    }
}

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
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();

        defaultPlayerColliderSize = playerCollider.size;
        sitPlayerColliderSize = defaultPlayerColliderSize;
        sitPlayerColliderSize.y -= 0.5f;

        ultimateGauge = 0.0f;
        maxUltimateGauge = 3.0f;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isSitting)
        {
            PlayerJump(cMiniJumpPower);
            isJumpingEnd = false;
        }

        //JumpAddForce
        if (Input.GetKey(KeyCode.Space) && !isJumpingEnd && !isSitting)
        {
            if (cMiniJumpPower < cMaxJumpPower)
            {
                PlayerJumping(cJumpPower);
                cMiniJumpPower += cJumpPower;
            }
        }

        if (Input.GetKeyUp(KeyCode.A) && canDash && !isSitting)
        {
            DoubleClickDash(true);
        }

        if (Input.GetKeyUp(KeyCode.D) && canDash && !isSitting)
        {
            DoubleClickDash(false);
        }

        if (Input.GetKeyDown(KeyCode.S) && canSit && !isJumping)
        {
            //Sit Start
            StartCoroutine(PlayerSit());
        }

        if (Input.GetKeyUp(KeyCode.S) && isSitting)
        {
            //Sit End
            isSitting = false;
            canDash = true;
        }

        //Atk
        if (Input.GetKeyDown(KeyCode.V))
        {
            //Attack
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            //Ultimate Attack
        }

        //Animation Script
        if (rigidBody.velocity.normalized.x == 0)
        {
            playerAnimator.SetBool("isRunning", false);
        }
        else
        {
            playerAnimator.SetBool("isRunning", true);
        }

        if (rigidBody.velocity.normalized.y > 0)
        {
            playerAnimator.SetBool("isUp", true);
            playerAnimator.SetBool("isDown", false);
        }
        else if (rigidBody.velocity.normalized.y < 0)
        {
            playerAnimator.SetBool("isUp", false);
            playerAnimator.SetBool("isDown", true);
        }
        else
        {
            playerAnimator.SetBool("isUp", false);
            playerAnimator.SetBool("isDown", false);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        Debug.Log(canDash);

        PlayerMove();
    }

    protected override void PlayerMove()
    {
        moveHorizontal = 0.0f;

        if (Input.GetKey(KeyCode.A))
        {
            if (isSitting)
            {
                moveHorizontal = 0.0f;
            }
            else
            {
                moveHorizontal = -1.0f;
            }
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (isSitting)
            {
                moveHorizontal = 0.0f;
            }
            else
            {
                moveHorizontal = 1.0f;
            }
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        base.PlayerMove();
    }
}

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

        ultimateGauge = 0.0f;
        maxUltimateGauge = 3.0f;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !isJumping)
        {
            cMiniJumpPower = Charging(cMiniJumpPower, cMaxJumpPower, cJumpPower);
        }

        if (Input.GetKeyUp(KeyCode.W) && !isJumping)
        {
            PlayerJump(cMiniJumpPower);
        }
    }

    private void FixedUpdate()
    {
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

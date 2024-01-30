using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNabi : Player
{
    public GameObject bulletPrefab;                 //Bullet Prefab
    public Transform bulletPosition;                //Bullet Instantiate Position
    public float coolTime;
    private float curTime;
    private bool isLock = false;                    //Lock

    void Start()
    {
        characterID = false;
        characterName = "Nabi";
        cLife = 3;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        ultimateGauge = 0.0f;
        maxUltimateGauge = 5.0f;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        //Jump
        if (Input.GetKey(KeyCode.RightControl) && !isJumping && !isLock)
        {
            Debug.Log("RightCtrl");
            cMiniJumpPower = Charging(cMiniJumpPower, cMaxJumpPower, cJumpPower);
        }

        //JumpAddForce
        if (Input.GetKeyUp(KeyCode.RightControl) && !isJumping && !isLock)
        {
            PlayerJump(cMiniJumpPower);
        }

        //LockOn
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLock = true;
            Debug.Log("Locked");
        }
        
        //LockOff
        else if (Input.GetKeyUp(KeyCode.L))
        {
            isLock = false;
            Debug.Log("Unlocked");
        }

        //Atk
        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.RightBracket))
            {
                Instantiate(bulletPrefab, bulletPosition.position, transform.rotation);
                Debug.Log(transform.rotation);
            }
            curTime = coolTime;
        }
        curTime -= Time.deltaTime;
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

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveHorizontal = -1.0f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            bulletPosition.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveHorizontal = 1.0f;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            bulletPosition.rotation = Quaternion.Euler(0, 180, 0);
        }

        base.PlayerMove();
    }
}

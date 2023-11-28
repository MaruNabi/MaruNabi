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
        if (Input.GetKey(KeyCode.UpArrow) && !isJumping && !isLock)
        {
            cMiniJumpPower = Charging(cMiniJumpPower, cMaxJumpPower, cJumpPower);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && !isJumping && !isLock)
        {
            PlayerJump(cMiniJumpPower);
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            isLock = true;
            Debug.Log("Locked");
        }
        
        else if (Input.GetKeyUp(KeyCode.RightShift))
        {
            isLock = false;
            Debug.Log("Unlocked");
        }

        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.RightBracket))
            {
                Instantiate(bulletPrefab, bulletPosition.position, transform.rotation);
            }
            curTime = coolTime;
        }
        curTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    protected override void PlayerMove()
    {
        moveHorizontal = 0.0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveHorizontal = -1.0f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveHorizontal = 1.0f;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        base.PlayerMove();
    }
}

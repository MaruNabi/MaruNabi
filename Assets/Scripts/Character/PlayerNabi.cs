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
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Comma) && !isJumping && !isLock)
        {
            cMiniJumpPower = Charging(cMiniJumpPower, cMaxJumpPower, cJumpPower);
        }

        if (Input.GetKeyUp(KeyCode.Comma) && !isJumping && !isLock)
        {
            PlayerJump(cMiniJumpPower);
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            isLock = true;
            Debug.Log("Locked");
        }
        
        else if (Input.GetKeyUp(KeyCode.Slash))
        {
            isLock = false;
            Debug.Log("Unlocked");
        }

        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Period))
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

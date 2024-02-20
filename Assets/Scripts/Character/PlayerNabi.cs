using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNabi : Player
{
    [SerializeField]
    private GameObject bulletPrefab;                 //Bullet Prefab
    [SerializeField]
    private GameObject bulletVectorManager;
    [SerializeField]
    private Transform bulletPosition;                //Bullet Instantiate Position
    [SerializeField]
    private float coolTime;
    private float curTime;
    private bool isLock = false;                    //Lock

    void Start()
    {
        characterID = false;
        characterName = "Nabi";
        cLife = 3;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();

        bulletVectorManager.SetActive(false);

        defaultPlayerColliderSize = playerCollider.size;
        sitPlayerColliderSize = defaultPlayerColliderSize;
        sitPlayerColliderSize.y -= 0.5f;

        ultimateGauge = 0.0f;
        maxUltimateGauge = 1500.0f;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.RightControl) && !isJumping && !isSitting)
        {
            PlayerJump(cMiniJumpPower);
            isJumpingEnd = false;
        }

        //JumpAddForce
        if (Input.GetKey(KeyCode.RightControl) && !isJumpingEnd && !isSitting)
        {
            if (cMiniJumpPower < cMaxJumpPower)
            {
                PlayerJumping(cJumpPower);
                cMiniJumpPower += cJumpPower;
            }
        }

        //LockOn
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLock = true;
            bulletVectorManager.SetActive(true);
        }
        
        //LockOff
        else if (Input.GetKeyUp(KeyCode.L))
        {
            isLock = false;
            BulletVectorManager.bulletVector = new Vector2(0, 0);
            bulletVectorManager.SetActive(false);
        }

        //Atk
        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.RightBracket))
            {
                Instantiate(bulletPrefab, bulletPosition.position, transform.rotation);
            }
            curTime = coolTime;
        }
        curTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            //None
            if (ultimateGauge < 500f)
            {
                return;
            }
            //Special Move
            else if (ultimateGauge == maxUltimateGauge)
            {

            }
            //Ability
            else
            {

            }
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

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (isSitting || isLock)
            {
                moveHorizontal = 0.0f;
            }
            else
            {
                moveHorizontal = -1.0f;
            }
            transform.rotation = Quaternion.Euler(0, 0, 0);
            bulletPosition.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (isSitting || isLock)
            {
                moveHorizontal = 0.0f;
            }
            else
            {
                moveHorizontal = 1.0f;
            }
            transform.rotation = Quaternion.Euler(0, 180, 0);
            bulletPosition.rotation = Quaternion.Euler(0, 180, 0);
        }

        base.PlayerMove();
    }
}

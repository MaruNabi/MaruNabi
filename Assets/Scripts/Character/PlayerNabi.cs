using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNabi : Player
{
    public static float ultimateGauge;
    [SerializeField]
    private GameObject bulletPrefab;                 //Bullet Prefab
    [SerializeField]
    private GameObject skillPrefab;
    [SerializeField]
    private float coolTime;
    private float curTime;
    private bool isLock = false;                    //Lock
    private bool isAttacksNow = false;
    [SerializeField]
    private GameObject playerBullets;               //For Pooling

    [SerializeField]
    private GameObject playerSkills;

    [SerializeField]
    private Image[] nabiLife;
    public Sprite blankHP, fillHP;

    void Start()
    {
        characterID = false;
        characterName = "Nabi";
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();

        reviveZone.SetActive(false);

        defaultPlayerColliderSize = playerCollider.size;
        sitPlayerColliderSize = defaultPlayerColliderSize;
        sitPlayerColliderSize.y -= 0.5f;

        ultimateGauge = 0.0f;

        reviveEffect = Resources.Load<GameObject>("Prefabs/VFX/Player/HyperCasual/Area/Area_heal_green");

        Managers.Pool.CreatePool(bulletPrefab, 20);
        Managers.Pool.CreatePool(skillPrefab, 5);
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.RightShift) && !isJumping && !isSitting && canJump)
        {
            PlayerJump(cMiniJumpPower);
            canJump = false;
            isJumpingEnd = false;
        }

        //JumpAddForce
        if (Input.GetKey(KeyCode.RightShift) && !isJumpingEnd && !isSitting)
        {
            if (cMiniJumpPower < cMaxJumpPower)
            {
                PlayerJumping(cJumpPower);
                cMiniJumpPower += cJumpPower;
            }
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            isLock = true;
        }
        
        else if (Input.GetKeyUp(KeyCode.L))
        {
            isLock = false;
        }

        //ToDo : getkey and curtime switch
        if (curTime <= 0 && canAtk)
        {
            if (Input.GetKey(KeyCode.RightBracket) && !isAttacksNow)
            {
                playerAnimator.SetBool("isAtk", true);
                if (bulletPrefab.name == "NABI_Bullet_SmallSpark")
                {
                    isAttacksNow = true;
                }
                GameObject bulletObject = Managers.Pool.Pop(bulletPrefab, playerBullets.transform).gameObject;
                bulletObject.transform.position = atkPosition.position;
                bulletObject.transform.rotation = transform.rotation;
            }
            curTime = coolTime;
        }
        curTime -= Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.RightBracket))
        {
            isAttacksNow = false;
            playerAnimator.SetBool("isAtk", false);
            //Playeranimator isatk false
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) && canDash && !isSitting)
        {
            DoubleClickDash(true);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) && canDash && !isSitting)
        {
            DoubleClickDash(false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && canSit && !isJumping)
        {
            StartCoroutine(PlayerSit());
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) && isSitting)
        {
            isSitting = false;
            canDash = true;
            canMove = true;
            playerAnimator.SetBool("isSit", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            //None
            if (ultimateGauge < 500.0f)
            {
                return;
            }
            //Special Move
            else if (ultimateGauge == maxUltimateGauge)
            {
                StartCoroutine(PlayerSit(true));
                GameObject skillObject = Managers.Pool.Pop(skillPrefab, playerSkills.transform).gameObject;
                skillObject.transform.position = atkPosition.position;
                skillObject.transform.rotation = transform.rotation;

                ultimateGauge = 0.0f;
            }
            //Ability
            else
            {
                //Animation?
                ultimateGauge -= 500.0f;
            }
        }

        //Animation Script
        if (rigidBody.velocity.normalized.x == 0)
        {
            playerAnimator.SetBool("isMove", false);
        }
        else
        {
            playerAnimator.SetBool("isMove", true);
        }

        if (!isGround)
        {
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
            /*else
            {
                playerAnimator.SetBool("isUp", false);
                playerAnimator.SetBool("isDown", false);
            }*/
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

        if (canMove)
        {
            PlayerMove();
        }
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
            atkPosition.rotation = Quaternion.Euler(0, 0, 0);
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
            atkPosition.rotation = Quaternion.Euler(0, 180, 0);
        }

        base.PlayerMove();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit");
            if (isInvincibleTime)
                return;

            isHit = true;
            canMove = false;

            if (cLife > 1)
            {
                cLife -= 1;
                StartCoroutine(Ondamaged(collision.transform.position));
            }
            else
            {
                cLife -= 1;
                StartCoroutine(Death());
            }

            UpdateLifeUI();
        }
    }

    protected override IEnumerator Revive()
    {
        yield return base.Revive();

        UpdateLifeUI();
    }

    private void UpdateLifeUI()
    {
        for (int i = 0; i < MAX_LIFE; i++)
        {
            if (i < cLife)
            {
                nabiLife[i].sprite = fillHP;
            }
            else
            {
                nabiLife[i].sprite = blankHP;
            }
        }
    }
}

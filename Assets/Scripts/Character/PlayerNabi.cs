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
    private bool isAttacksNow = false;
    [SerializeField]
    private GameObject playerBullets;               //For Pooling

    [SerializeField]
    private GameObject playerSkills;

    [SerializeField]
    private Image playerHpUI;

    private Sprite[] nabiLifeSprite = new Sprite[6];
    private int currentHp;

    void Start()
    {
        characterID = false;
        characterName = "Nabi";
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();

        reviveZone.SetActive(false);

        moveLeft = KeyCode.LeftArrow;
        moveRight = KeyCode.RightArrow;

        defaultPlayerColliderSize = playerCollider.size;
        sitPlayerColliderSize = defaultPlayerColliderSize;
        sitPlayerColliderSize.y -= 0.5f;

        ultimateGauge = 0.0f;
        currentHp = cLife;

        cMaxJumpCount = 1;
        cJumpCount = 0;

        reviveEffect = Resources.Load<GameObject>("Prefabs/VFX/Player/HyperCasual/Area/Area_heal_green");

        for (int i = 0; i < nabiLifeSprite.Length; i++)
        {
            nabiLifeSprite[i] = Resources.Load<Sprite>("UI/PlayerUI/UI_Stage_NabiIcon_" + i);
        }

        for (int i = 0; i < canPlayerState.Length; i++)
        {
            canPlayerState[i] = true;
        }

        UpdateLifeUI();

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
        if (Input.GetKeyDown(KeyCode.RightShift) && !isJumping && !isSitting && canPlayerState[3] && cJumpCount < cMaxJumpCount)
        {
            rigidBody.velocity = Vector2.zero;
            PlayerJump(cMiniJumpPower);
            canPlayerState[3] = false;
            isJumpingEnd = false;
            cJumpCount++;
        }

        //JumpAddForce
        if (Input.GetKey(KeyCode.RightShift) && !isJumpingEnd && !isSitting)
        {
            if (cMiniJumpPower < cMaxJumpPower)
            {
                PlayerJumping(cJumpPower);
                cMiniJumpPower += cJumpPower;
            }
            canPlayerState[3] = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
            canPlayerState[3] = true;
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
        if (curTime <= 0 && canPlayerState[4])
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

        if (Input.GetKeyUp(KeyCode.LeftArrow) && canPlayerState[1] && !isSitting)
        {
            DoubleClickDash(true);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) && canPlayerState[1] && !isSitting)
        {
            DoubleClickDash(false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && canPlayerState[2] && !isJumping)
        {
            StartCoroutine(PlayerSit());
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) && isSitting)
        {
            isSitting = false;
            canPlayerState[0] = true;
            canPlayerState[1] = true;
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

        if (currentHp != cLife)
        {
            UpdateLifeUI();
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

        if (canPlayerState[0])
        {
            PlayerMove();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            PlayerHit(collision.transform.position);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            PlayerHit(collision.transform.position);
        }
    }

    protected override IEnumerator Revive()
    {
        yield return base.Revive();

        UpdateLifeUI();
    }

    private void UpdateLifeUI()
    {
        if (cLife >= 0)
        {
            playerHpUI.GetComponent<Image>().sprite = nabiLifeSprite[cLife];
            currentHp = cLife;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerMaru : Player
{
    public static float ultimateGauge;
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField]
    private GameObject skillPrefab;

    private bool isLock = false;
    private bool attacksNow = false;

    [SerializeField]
    private GameObject playerBullets;

    [SerializeField]
    private GameObject playerSkills;

    [SerializeField]
    private GameObject playerShield;

    [SerializeField]
    private Image[] maruLife;
    public Sprite blankHP, fillHP;

    void Start()
    {
        characterID = true;
        characterName = "Maru";
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();

        playerShield.SetActive(false);
        reviveZone.SetActive(false);

        defaultPlayerColliderSize = playerCollider.size;
        sitPlayerColliderSize = defaultPlayerColliderSize;
        sitPlayerColliderSize.y -= 0.5f;

        ultimateGauge = 0.0f;

        Managers.Pool.CreatePool(swordPrefab, 2);
        Managers.Pool.CreatePool(skillPrefab, 2);
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isSitting && canJump)
        {
            PlayerJump(cMiniJumpPower);
            canJump = false;
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
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isLock = true;
        }

        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isLock = false;
        }

        if (Input.GetKey(KeyCode.V) && canAtk)
        {
            playerAnimator.SetBool("isAtk", true);
            if (!attacksNow)
            {
                attacksNow = true;
                GameObject swordObject = Managers.Pool.Pop(swordPrefab, playerBullets.transform).gameObject;
                swordObject.transform.position = atkPosition.position;
                swordObject.transform.rotation = transform.rotation;
            }

            if (playerBullets.transform.childCount == 0)
            {
                attacksNow = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.V) && canAtk)
        {
            playerAnimator.SetBool("isAtk", false);
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
            StartCoroutine(PlayerSit());
        }

        if (Input.GetKeyUp(KeyCode.S) && isSitting)
        {
            isSitting = false;
            canDash = true;
            canMove = true;
            playerAnimator.SetBool("isSit", false);
        }

        if (Input.GetKeyDown(KeyCode.B))
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

                if (skillPrefab.name == "MARU_Bullet_BigSword")
                {
                    StartCoroutine(MaruSkillBigSword());
                }

                ultimateGauge = 0.0f;
            }
            //Ability
            else
            {
                playerAnimator.SetBool("isDefence", true);
                StartCoroutine(PlayerShield());
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

        if (Input.GetKey(KeyCode.A))
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

        if (Input.GetKey(KeyCode.D))
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
        }
    }

    protected override IEnumerator Revive()
    {
        yield return base.Revive();

        UpdateLifeUI();
    }

    private IEnumerator PlayerShield()
    {
        
        playerShield.SetActive(true);
        
        canMove = false;
        canDash = false;
        canSit = false;
        canJump = false;
        canAtk = false;
        //Shield on
        yield return new WaitForSeconds(0.25f);
        //Shield Idle
        yield return new WaitForSeconds(0.5f);
        //Shield Off
        playerAnimator.SetBool("isDefence", false);
        yield return new WaitForSeconds(0.25f);
        canMove = true;
        canDash = true;
        canSit = true;
        canJump = true;
        canAtk = true;
        playerShield.SetActive(false);
    }

    private void UpdateLifeUI()
    {
        for (int i = 0; i < MAX_LIFE; i++)
        {
            if (i < cLife)
            {
                maruLife[i].sprite = fillHP;
            }
            else
            {
                maruLife[i].sprite = blankHP;
            }
        }
    }

    private IEnumerator MaruSkillBigSword()
    {
        Vector3 target1;
        Vector3 target2;
        Vector3 target3;

        if (transform.rotation.y == 0)
        {
            target1 = transform.position - new Vector3(5, 0, 0);
            target2 = transform.position + new Vector3(4, 0, 0);
            target3 = transform.position - new Vector3(6, 0, 0);
        }
        else
        {
            target1 = transform.position + new Vector3(5, 0, 0);
            target2 = transform.position - new Vector3(4, 0, 0);
            target3 = transform.position + new Vector3(6, 0, 0);
        }

        canMove = false;
        canDash = false;
        canSit = false;
        canJump = false;
        canAtk = false;

        float playerGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0f;
        playerCollider.isTrigger = true;

        yield return new WaitForSeconds(0.25f);

        playerAnimator.SetBool("isDash", true);
        transform.DOMove(target1, 0.215f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.215f);
        transform.Rotate(0, 180, 0);
        transform.DOMove(target2, 0.215f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.215f);
        transform.Rotate(0, 180, 0);
        transform.DOMove(target3, 0.215f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.215f);
        playerAnimator.SetBool("isDash", false);

        rigidBody.gravityScale = playerGravity;
        playerCollider.isTrigger = false;

        canMove = true;
        canDash = true;
        canSit = true;
        canJump = true;
        canAtk = true;
    }
}

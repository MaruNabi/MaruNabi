using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMaru : Player
{
    public static float ultimateGauge;
    [SerializeField]
    private GameObject swordPrefab;
    [SerializeField]
    private GameObject skillPrefab;
    [SerializeField]
    private Transform atkPosition;

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
                ultimateGauge = 0.0f;
            }
            //Ability
            else
            {
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
        //shield on
        yield return new WaitForSeconds(0.25f);
        //shield Idle
        yield return new WaitForSeconds(1.0f);
        //Shield Off
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
}

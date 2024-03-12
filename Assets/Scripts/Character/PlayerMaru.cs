using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMaru : Player
{
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

        reviveZone.SetActive(false);

        defaultPlayerColliderSize = playerCollider.size;
        sitPlayerColliderSize = defaultPlayerColliderSize;
        sitPlayerColliderSize.y -= 0.5f;

        ultimateGauge = 0.0f;
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
            StartCoroutine(PlayerSit());
        }

        if (Input.GetKeyUp(KeyCode.S) && isSitting)
        {
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

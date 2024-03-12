using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNabi : Player
{
    [SerializeField]
    private GameObject bulletPrefab;                 //Bullet Prefab
    [SerializeField]
    private GameObject skillPrefab;
    [SerializeField]
    private GameObject bulletVectorManager;
    [SerializeField]
    private Transform bulletPosition;                //Bullet Instantiate Position
    [SerializeField]
    private float coolTime;
    private float curTime;
    private bool isLock = false;                    //Lock

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

        bulletVectorManager.SetActive(false);

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
            if (ultimateGauge < 500.0f)
            {
                return;
            }
            //Special Move
            else if (ultimateGauge == maxUltimateGauge)
            {
                Instantiate(skillPrefab, bulletPosition.position, transform.rotation);
                ultimateGauge = 0.0f;
            }
            //Ability
            else
            {
                ultimateGauge -= 500.0f;
            }
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

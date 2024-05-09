using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerMaru : Player
{
    public static float ultimateGauge;
    private GameObject swordPrefab;
    private GameObject skillPrefab;

    private bool attacksNow = false;

    [SerializeField] private GameObject playerBullets;

    [SerializeField] private GameObject playerSkills;

    [SerializeField] private GameObject playerShield;

    [SerializeField] private Image playerHpUI;

    private Sprite[] maruLifeSprite = new Sprite[6];
    private int currentHp;

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

        moveLeft = KeyCode.A;
        moveRight = KeyCode.D;

        defaultPlayerColliderSize = playerCollider.size;
        sitPlayerColliderSize = defaultPlayerColliderSize;
        sitPlayerColliderSize.y -= 0.5f;

        ultimateGauge = 0.0f;
        currentHp = cLife;

        cMaxJumpCount = 1;
        cJumpCount = 0;

        reviveEffect = Resources.Load<GameObject>("Prefabs/VFX/Player/HyperCasual/Area/Area_heal_green");

        for (int i = 0; i < maruLifeSprite.Length; i++)
        {
            maruLifeSprite[i] = Resources.Load<Sprite>("UI/PlayerUI/UI_Stage_MaruIcon_" + i);
        }

        for (int i = 0; i < canPlayerState.Length; i++)
        {
            canPlayerState[i] = true;
        }

        if (PlayerSkillDataManager.maruSkillSet[0] != 0)
        {
            swordPrefab = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Bullet_" + PlayerSkillDataManager.maruSkillSet[0]);
            skillPrefab = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Skill_" + PlayerSkillDataManager.maruSkillSet[0]);

            switch (PlayerSkillDataManager.maruSkillSet[1])
            {
                case 1:
                    cMaxJumpCount = 2;
                    break;
                case 2:
                    cLife += 1;
                    break;
                case 3:
                    cSpeed += 2.0f;
                    break;
                default:
                    break;
            }
        }
        else
        {
            swordPrefab = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Bullet_1");
            skillPrefab = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Skill_1");
        }

        UpdateLifeUI();

        Managers.Pool.CreatePool(swordPrefab, 2);
        Managers.Pool.CreatePool(skillPrefab, 2);
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        SlopeCheck();

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isSitting && canPlayerState[3] && cJumpCount < cMaxJumpCount)
        {
            rigidBody.velocity = Vector2.zero;
            PlayerJump(cMiniJumpPower);
            isJumpingEnd = false;
            cJumpCount++;
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

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isLock = true;
        }

        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isLock = false;
        }

        if (Input.GetKey(KeyCode.V) && canPlayerState[4])
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

        if (Input.GetKeyUp(KeyCode.V) && canPlayerState[4])
        {
            playerAnimator.SetBool("isAtk", false);
        }

        if (Input.GetKeyUp(KeyCode.A) && canPlayerState[1] && !isSitting)
        {
            DoubleClickDash(true);
        }

        if (Input.GetKeyUp(KeyCode.D) && canPlayerState[1] && !isSitting)
        {
            DoubleClickDash(false);
        }

        if (Input.GetKeyDown(KeyCode.S) && canPlayerState[2] && !isJumping)
        {
            StartCoroutine(PlayerSit());
        }

        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && isSitting)
        {
            if (canFallDown)
            {
                playerStandCollider.isTrigger = true;
                playerSideFrictionCollider.isTrigger = true;
                playerAnimator.SetBool("isDown", true);
            }
        }

        if (Input.GetKeyUp(KeyCode.S) && isSitting)
        {
            isSitting = false;
            canPlayerState[0] = true;
            canPlayerState[1] = true;
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

        if (moveHorizontal == 0 && !isDashing && !playerAnimator.GetBool("isDead"))
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

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

        if (Mathf.RoundToInt(rigidBody.velocity.normalized.y) == 0)
        {
            playerAnimator.SetBool("isUp", false);
            playerAnimator.SetBool("isDown", false);
            float index = Mathf.Round(rigidBody.velocity.normalized.y * 10) * 0.1f;
            if (groundRay && currentGroundName != groundRay.collider.gameObject.name && Mathf.Abs(index) < 0.1f)
            {
                currentGroundName = groundRay.collider.gameObject.name;
            }
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
            else
            {
                playerAnimator.SetBool("isUp", false);
                playerAnimator.SetBool("isDown", false);
            }
        }
        /*else
        {
            playerAnimator.SetBool("isUp", false);
            playerAnimator.SetBool("isDown", false);
        }*/
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
        else if (collision.gameObject.CompareTag("NoDeleteEnemyBullet"))
        {
            PlayerHit(collision.transform.position, false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            PlayerHit(collision.transform.position);
        }
        else if (collision.gameObject.CompareTag("NoDeleteEnemyBullet"))
        {
            PlayerHit(collision.transform.position, false);
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

        PlayerStateTransition(false, 0);
        //Shield on
        yield return new WaitForSeconds(0.25f);
        //Shield Idle
        yield return new WaitForSeconds(0.5f);
        //Shield Off
        playerAnimator.SetBool("isDefence", false);
        yield return new WaitForSeconds(0.25f);
        PlayerStateTransition(true, 0);
        playerShield.SetActive(false);
    }

    private void UpdateLifeUI()
    {
        if (cLife >= 0)
        {
            playerHpUI.GetComponent<Image>().sprite = maruLifeSprite[cLife];
            currentHp = cLife;
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

        PlayerStateTransition(false, 0);

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

        PlayerStateTransition(true, 0);
    }
}

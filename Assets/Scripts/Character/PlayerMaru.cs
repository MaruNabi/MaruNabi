using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerMaru : Player
{
    public static float ultimateGauge;
    private GameObject swordPrefab_1;
    private GameObject swordPrefab_2;
    private GameObject currentSwordPrefab;
    private GameObject skillPrefab_1;
    private GameObject skillPrefab_2;
    private GameObject currentSkillPrefab;

    private bool attacksNow = false;

    [SerializeField] private GameObject playerBullets;

    [SerializeField] private GameObject playerSkills;

    [SerializeField] private GameObject playerShield;

    [SerializeField] private Image playerHpUI;
    [SerializeField] private GameObject skillChangeUI;

    private Sprite[] maruLifeSprite = new Sprite[6];

    private string selectedPadName;
    private int currentHp;
    private bool isPad;
    
    void Start()
    {
        characterID = true;
        characterName = "Maru";
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        //playerCollider = GetComponent<BoxCollider2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        playerShield.SetActive(false);
        reviveZone.SetActive(false);

        PlayerKeySetting();

        defaultPlayerColliderSize = playerCollider.size;
        sitPlayerColliderSize = defaultPlayerColliderSize;
        sitPlayerColliderSize.y -= 0.5f;

        ultimateGauge = 0.0f;

        cMaxJumpCount = 1;
        cJumpCount = 0;
        canChangeSkillSet = true;

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
            swordPrefab_1 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Bullet_" + PlayerSkillDataManager.maruSkillSet[0]);
            skillPrefab_1 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Skill_" + PlayerSkillDataManager.maruSkillSet[0]);

            swordPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Bullet_" + PlayerSkillDataManager.maruSkillSet[2]);
            skillPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Skill_" + PlayerSkillDataManager.maruSkillSet[2]);

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
            swordPrefab_1 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Bullet_" + 1);
            skillPrefab_1 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Skill_" + 1);
            swordPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Bullet_" + 2);
            skillPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Skill_" + 2);
        }

        if (PlayerSkillDataManager.maruSkillSet[1] == 1 || PlayerSkillDataManager.maruSkillSet[3] == 1)
            cLife -= 1;

        currentHp = cLife;

        Managers.Pool.CreatePool(swordPrefab_1, 5);
        Managers.Pool.CreatePool(skillPrefab_1, 3);

        Managers.Pool.CreatePool(swordPrefab_2, 5);
        Managers.Pool.CreatePool(skillPrefab_2, 3);

        Managers.Input.keyAction -= OnPlayerMove;
        Managers.Input.keyAction -= OnPlayerAttack;
        Managers.Input.keyAction -= OnPlayerDash;
        Managers.Input.keyAction -= OnPlayerJump;
        Managers.Input.keyAction -= OnPlayerSit;
        Managers.Input.keyAction -= OnPlayerSkillChange;

        Managers.Input.keyAction += OnPlayerMove;
        Managers.Input.keyAction += OnPlayerAttack;
        Managers.Input.keyAction += OnPlayerDash;
        Managers.Input.keyAction += OnPlayerJump;
        Managers.Input.keyAction += OnPlayerSit;
        Managers.Input.keyAction += OnPlayerSkillChange;

        currentSwordPrefab = swordPrefab_1;
        currentSkillPrefab = skillPrefab_1;

        UpdateLifeUI();
    }

    void Update()
    {
        SlopeCheck();

        SurfaceEffectorCheck();

        if (InputManager.isNeedInit)
        {
            InputManager.isNeedInit = false;
            OnPlayerInit();
        }

        if (isDashing)
            rigidBody.velocity = new Vector2(dashDirection * 20, 0.0f);

        if (moveHorizontal == 0 && !isDashing && !playerAnimator.GetBool("isDead") && !isSurfaceEffector)
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (currentHp != cLife)
        {
            UpdateLifeUI();
        }

        #region Animation
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
        #endregion
    }

    void OnDisable()
    {
        Managers.Input.keyAction -= OnPlayerMove;
        Managers.Input.keyAction -= OnPlayerAttack;
        Managers.Input.keyAction -= OnPlayerDash;
        Managers.Input.keyAction -= OnPlayerJump;
        Managers.Input.keyAction -= OnPlayerSit;
        Managers.Input.keyAction -= OnPlayerSkillChange;
    }

    #region Trigger and Collision
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
    #endregion

    protected override IEnumerator Revive()
    {
        yield return base.Revive();

        UpdateLifeUI();
    }

    private IEnumerator PlayerShield()
    {
        playerShield.SetActive(true);

        //PlayerStateTransition(false);
        //Shield on
        yield return new WaitForSeconds(0.25f);
        //Shield Idle
        yield return new WaitForSeconds(0.5f);
        //Shield Off
        playerAnimator.SetBool("isDefence", false);
        yield return new WaitForSeconds(0.25f);
        //PlayerStateTransition(true);
        playerShield.SetActive(false);
    }

    private void UpdateLifeUI()
    {
        if (cLife >= 0)
        {
            playerHpUI.GetComponent<Image>().sprite = maruLifeSprite[cLife];
            for (int i = 0; i < cLifeUI.Length; i++)
            {
                if (i < cLife)
                    cLifeUI[i].sprite = fullHeart;
                else
                    cLifeUI[i].sprite = emptyHeart;
            }
            currentHp = cLife;
        }
    }

    private void OnPlayerInit()
    {
        isJumping = false;
        isLock = false;
        isSitting = false;
        playerAnimator.SetBool("isSit", false);
        playerAnimator.SetBool("isAtk", false);
        if (!isDashing)
        {
            moveHorizontal = 0.0f;
        }
    }

    protected override void OnPlayerMove()
    {
        base.OnPlayerMove();

        if (!isPad)
        {
            if (Input.GetKey(moveLeftKey))
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

                PlayerMovement();
            }

            //if (!(Input.GetKey(moveLeftKey)) && !(Input.GetKey(moveRightKey)))
            //moveHorizontal = 0.0f;

            if (Input.GetKey(moveRightKey))
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

                PlayerMovement();
            }
        }
        else
        {
            if (Input.GetAxis(selectedPadName) > 0)
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

                PlayerMovement();
            }

            if (Input.GetAxis(selectedPadName) < 0)
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

                PlayerMovement();
            }
        }
    }

    protected override void OnPlayerDash()
    {
        if (Input.GetKeyDown(dashKey) && !isSitting && !isLock && isDashCoolEnd)
        {
            StartCoroutine("PlayerDash");
        }
        /*if (!isPad)
        {
            if (Input.GetKeyDown(moveLeftKey) && !isSitting && !isLock && isDashCoolEnd) //canPlayerState[1]
            {
                DoubleClickDash(true);
            }

            if (Input.GetKeyDown(moveRightKey) && !isSitting && !isLock && isDashCoolEnd) //canPlayerState[1]
            {
                DoubleClickDash(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(dashKey) && !isSitting && !isLock && isDashCoolEnd)
            {
                StartCoroutine("PlayerDash");
            }
        }*/
    }

    protected override void OnPlayerAttack()
    {
        base.OnPlayerAttack();

        if (Input.GetKey(normalAtkKey)) //canPlayerState[4]
        {
            playerAnimator.SetBool("isAtk", true);
            if (!attacksNow)
            {
                attacksNow = true;
                GameObject swordObject = Managers.Pool.Pop(currentSwordPrefab, playerBullets.transform).gameObject;
                swordObject.transform.position = atkPosition.position;
                swordObject.transform.rotation = transform.rotation;
            }

            if (playerBullets.transform.childCount == 0)
            {
                attacksNow = false;
            }
        }
        else if (!(Input.GetKey(normalAtkKey)))
        {
            playerAnimator.SetBool("isAtk", false);
        }

        if (Input.GetKeyDown(specialAtkKey)) //canPlayerState[4]
        {
            if (ultimateGauge == maxUltimateGauge)
            {
                StartCoroutine(PlayerSit(true));
                GameObject skillObject = Managers.Pool.Pop(currentSkillPrefab, playerSkills.transform).gameObject;
                skillObject.transform.position = atkPosition.position;
                skillObject.transform.rotation = transform.rotation;

                if (currentSkillPrefab.name == "MARU_Skill_1")
                {
                    StartCoroutine(MaruSkillBigSword());
                }

                ultimateGauge = 0.0f;
            }
            else if (ultimateGauge >= 1000.0f)
            {
                playerAnimator.SetBool("isDefence", true);
                StartCoroutine(PlayerShield());
                ultimateGauge -= 1000.0f;
            }
            else
                return;
        }
    }

    protected override void OnPlayerSkillChange()
    {
        if (Input.GetKeyDown(skillChangeKey) && canChangeSkillSet)
        {
            Managers.Sound.PlaySFX("Transition");
            canChangeSkillSet = false;
            skillChangeUI.GetComponent<PlayerSkillChange>().SkillChangeImage();
            currentSwordPrefab = swordPrefab_2;
            currentSkillPrefab = skillPrefab_2;
            switch (PlayerSkillDataManager.maruSkillSet[3])
            {
                case 1:
                    cMaxJumpCount = 2;
                    cSpeed = 6.0f;
                    break;
                case 2:
                    cMaxJumpCount = 1;
                    cSpeed = 6.0f;
                    if (cLife + 1 <= MAX_LIFE)
                        cLife += 1;
                    break;
                case 3:
                    cMaxJumpCount = 1;
                    cSpeed = 8.0f;
                    break;
                default:
                    break;
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
            target2 = transform.position + new Vector3(10, 0, 0);
            target3 = transform.position; //- new Vector3(5, 0, 0);
        }
        else
        {
            target1 = transform.position + new Vector3(5, 0, 0);
            target2 = transform.position - new Vector3(10, 0, 0);
            target3 = transform.position; //+ new Vector3(5, 0, 0);
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

    private void PlayerKeySetting()
    {
        if (KeyData.isMaruPad)
        {
            isPad = true;
            selectedPadName = "Horizontal_J1";
            jumpKey = KeyCode.Joystick1Button0;
            lockKey = KeyCode.Joystick1Button4;
            sitKey = KeyCode.Joystick1Button8;
            normalAtkKey = KeyCode.Joystick1Button5;
            specialAtkKey = KeyCode.Joystick1Button3;
            skillChangeKey = KeyCode.Joystick1Button2;
            dashKey = KeyCode.Joystick1Button1;
        }
        else
        {
            isPad = false;
            moveLeftKey = KeyCode.LeftArrow;
            moveRightKey = KeyCode.RightArrow;
            jumpKey = KeyCode.Z;
            lockKey = KeyCode.C;
            sitKey = KeyCode.DownArrow;
            normalAtkKey = KeyCode.X;
            specialAtkKey = KeyCode.V;
            skillChangeKey = KeyCode.Tab;
            dashKey = KeyCode.LeftShift;
        }
    }
}

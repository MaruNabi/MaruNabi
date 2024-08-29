using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNabi : Player
{
    public static float ultimateGauge;
    public static bool isNabiTraitActivated;
    private GameObject bulletPrefab_1;                 //Bullet Prefab
    private GameObject bulletPrefab_2;
    private GameObject currentBulletPrefab;
    private GameObject skillPrefab_1;
    private GameObject skillPrefab_2;
    private GameObject currentSkillPrefab;

    [SerializeField] private float coolTime;
    private float curTime;
    private bool isAttacksNow = false;
    [SerializeField] private GameObject playerBullets;               //For Pooling

    [SerializeField] private GameObject playerSkills;

    [SerializeField] private Image playerHpUI;
    [SerializeField] private GameObject skillChangeUI;
    [SerializeField] private Player playerMaru;

    private Sprite[] nabiLifeSprite = new Sprite[6];
    private string selectedPadName;
    private int currentHp;
    private bool isPad;
    public int NabiTraitScore { get; private set; }

    void Start()
    {
        characterID = false;
        characterName = "Nabi";
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        //playerCollider = GetComponent<BoxCollider2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        isNabiTraitActivated = false;
        NabiTraitScore = 0;

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

        for (int i = 0; i < nabiLifeSprite.Length; i++)
        {
            nabiLifeSprite[i] = Resources.Load<Sprite>("UI/PlayerUI/UI_Stage_NabiIcon_" + i);
        }

        for (int i = 0; i < canPlayerState.Length; i++)
        {
            canPlayerState[i] = true;
        }

        if (PlayerSkillDataManager.nabiSkillSet[0] != 0)
        {
            bulletPrefab_1 = Resources.Load<GameObject>("Prefabs/Player/Bullets/NABI_Bullet_" + PlayerSkillDataManager.nabiSkillSet[0]);
            skillPrefab_1 = Resources.Load<GameObject>("Prefabs/Player/Bullets/NABI_Skill_" + PlayerSkillDataManager.nabiSkillSet[0]);

            bulletPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/NABI_Bullet_" + PlayerSkillDataManager.nabiSkillSet[2]);
            skillPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/NABI_Skill_" + PlayerSkillDataManager.nabiSkillSet[2]);

            switch (PlayerSkillDataManager.nabiSkillSet[1])
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
            bulletPrefab_1 = Resources.Load<GameObject>("Prefabs/Player/Bullets/NABI_Bullet_" + 1);
            skillPrefab_1 = Resources.Load<GameObject>("Prefabs/Player/Bullets/NABI_Skill_" + 1);
            bulletPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/NABI_Bullet_" + 3);
            skillPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/NABI_Skill_" + 3);
        }

        if (PlayerSkillDataManager.nabiSkillSet[1] == 1 || PlayerSkillDataManager.nabiSkillSet[3] == 1)
            cLife -= 1;

        currentHp = cLife;

        Managers.Pool.CreatePool(bulletPrefab_1, 20);
        Managers.Pool.CreatePool(skillPrefab_1, 5);

        Managers.Pool.CreatePool(bulletPrefab_2, 20);
        Managers.Pool.CreatePool(skillPrefab_2, 5);

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

        currentBulletPrefab = bulletPrefab_1;
        currentSkillPrefab = skillPrefab_1;

        UpdateLifeUI();
    }

    void Update()
    {
        SlopeCheck();

        SurfaceEffectorCheck();

        if (InputManager.isNeedInit)
        {
            //InputManager.isNeedInit = false;
            if (playerMaru.isPlayerDead)
                InputManager.isNeedInit = false;
            OnPlayerInit();
        }

        if (isDashing)
            rigidBody.velocity = new Vector2(dashDirection * 20, 0.0f);

        if (moveHorizontal == 0 && !isDashing && !playerAnimator.GetBool("isDead") && !isSurfaceEffector) //isDead는 Slope에서 죽으면 밀려야 하기 때문
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
    #endregion

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
        isAttacksNow = false;
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

        //ToDo : getkey and curtime switch
        if (curTime <= 0) //canPlayerState[4]
        {
            if (Input.GetKey(normalAtkKey) && !isAttacksNow)
            {
                playerAnimator.SetBool("isAtk", true);
                if (currentBulletPrefab.name == "NABI_Bullet_3")
                {
                    isAttacksNow = true;
                }
                GameObject bulletObject = Managers.Pool.Pop(currentBulletPrefab, playerBullets.transform).gameObject;
                bulletObject.transform.position = atkPosition.position;
                bulletObject.transform.rotation = transform.rotation;
            }
            else if (!(Input.GetKey(normalAtkKey)))
            {
                isAttacksNow = false;
                playerAnimator.SetBool("isAtk", false);
            }
            curTime = coolTime;
        }
        curTime -= Time.deltaTime;

        if (Input.GetKeyDown(specialAtkKey)) //canPlayerState[4]
        {
            if (ultimateGauge == maxUltimateGauge)
            {
                //StartCoroutine(PlayerSit(true));
                GameObject skillObject = Managers.Pool.Pop(currentSkillPrefab, playerSkills.transform).gameObject;
                skillObject.transform.position = atkPosition.position;
                skillObject.transform.rotation = transform.rotation;

                ultimateGauge = 0.0f;
            }
            if (ultimateGauge >= 700.0f && !isNabiTraitActivated)
            {
                StartCoroutine("NabiTraitActive");
                ultimateGauge -= 700.0f;
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
            //canPlayerState[4] = false;
            skillChangeUI.GetComponent<PlayerSkillChange>().SkillChangeImage();
            currentBulletPrefab = bulletPrefab_2;
            currentSkillPrefab = skillPrefab_2;
            switch (PlayerSkillDataManager.nabiSkillSet[3])
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

    private IEnumerator NabiTraitActive()
    {
        NabiTraitParticlePlay();
        Managers.Sound.PlaySFX("Player_DamageBuff");
        playerMaru.NabiTraitParticlePlay();
        isNabiTraitActivated = true;
        NabiTraitScore += 1;
        yield return new WaitForSeconds(1f);
        isNabiTraitActivated = false;
    }

    private void PlayerKeySetting()
    {
        if (KeyData.isNabiPad)
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

        if (KeyData.isBothPad)
        {
            isPad = true;
            selectedPadName = "Horizontal_J2";
            jumpKey = KeyCode.Joystick2Button0;
            lockKey = KeyCode.Joystick2Button4;
            sitKey = KeyCode.Joystick2Button8;
            normalAtkKey = KeyCode.Joystick2Button5;
            specialAtkKey = KeyCode.Joystick2Button3;
            skillChangeKey = KeyCode.Joystick2Button2;
            dashKey = KeyCode.Joystick2Button1;
        }
    }
}

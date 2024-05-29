using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNabi : Player
{
    public static float ultimateGauge;
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
            bulletPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/NABI_Bullet_" + 2);
            skillPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/NABI_Skill_" + 2);
        }

        UpdateLifeUI();

        Managers.Pool.CreatePool(bulletPrefab_1, 20);
        Managers.Pool.CreatePool(skillPrefab_1, 5);

        Managers.Pool.CreatePool(bulletPrefab_2, 20);
        Managers.Pool.CreatePool(skillPrefab_2, 5);

        Managers.Input.keyAction -= OnKeyboard;
        Managers.Input.keyAction += OnKeyboard;

        Managers.Input.keyAction -= OnPlayerAttack;
        Managers.Input.keyAction += OnPlayerAttack;

        Managers.Input.keyAction -= OnPlayerDash;
        Managers.Input.keyAction += OnPlayerDash;

        Managers.Input.keyAction -= OnPlayerJump;
        Managers.Input.keyAction += OnPlayerJump;

        currentBulletPrefab = bulletPrefab_1;
        currentSkillPrefab = skillPrefab_1;
    }

    void Update()
    {
        //if (isDashing)
            //return;

        if (PauseUI.isGamePaused)
            return;

        SlopeCheck();

        SurfaceEffectorCheck();

        if (InputManager.isNeedInit)
        {
            InputManager.isNeedInit = false;
            Debug.Log("Init!");
            OnPlayerInit();
        }

        if (moveHorizontal == 0 && !isDashing && !playerAnimator.GetBool("isDead") && !isSurfaceEffector) //isDead는 Slope에서 죽으면 밀려야 하기 때문 NeedConfirm
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

    private void OnPlayerInit()
    {
        isJumping = false;
        isLock = false;
        isAttacksNow = false;
        isSitting = false;
        canPlayerState[0] = true;
        canPlayerState[1] = true;
        playerAnimator.SetBool("isSit", false);
        isAttacksNow = false;
        playerAnimator.SetBool("isAtk", false);
    }

    private void OnPlayerKeyUp()
    {
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            isJumping = false;
        }

        /*if (Input.GetKeyUp(KeyCode.L))
        {
            isLock = false;
        }*/

        if (Input.GetKeyUp(KeyCode.RightBracket))
        {
            isAttacksNow = false;
            playerAnimator.SetBool("isAtk", false);
        }
    }

    private void OnPlayerJump()
    {
        //Jump
        if (Input.GetKeyDown(KeyCode.RightShift) && !isJumping && !isSitting && canPlayerState[3] && cJumpCount < cMaxJumpCount)
        {
            rigidBody.velocity = Vector2.zero;
            PlayerJump(cMiniJumpPower);
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
        }
        else if (!(Input.GetKey(KeyCode.RightShift)))
        {
            isJumping = false;
        }
    }

    private void OnPlayerAttack()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLock = true;
        }
        else if (!(Input.GetKey(KeyCode.L)))
        {
            isLock = false;
        }

        //ToDo : getkey and curtime switch
        if (curTime <= 0 && canPlayerState[4])
        {
            if (Input.GetKey(KeyCode.RightBracket) && !isAttacksNow)
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
            else if (!(Input.GetKey(KeyCode.RightBracket)))
            {
                isAttacksNow = false;
                playerAnimator.SetBool("isAtk", false);
            }
            curTime = coolTime;
        }
        curTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftBracket) && canPlayerState[4])
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
                GameObject skillObject = Managers.Pool.Pop(currentSkillPrefab, playerSkills.transform).gameObject;
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
    }

    private void OnPlayerDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && canPlayerState[1] && !isSitting)
        {
            DoubleClickDash(true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && canPlayerState[1] && !isSitting)
        {
            DoubleClickDash(false);
        }
    }

    private void OnPlayerSit()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && canPlayerState[2] && !isJumping)
        {
            StartCoroutine(PlayerSit());
        }
        else if (!(Input.GetKey(KeyCode.DownArrow)) && isSitting) //Input.GetKeyUp(KeyCode.DownArrow)
        {
            isSitting = false;
            canPlayerState[0] = true;
            canPlayerState[1] = true;
            playerAnimator.SetBool("isSit", false);
        }
    }

    private void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.RightShift) && isSitting)
        {
            if (canFallDown)
            {
                playerStandCollider.isTrigger = true;
                playerSideFrictionCollider.isTrigger = true;
                playerAnimator.SetBool("isDown", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Equals) && canChangeSkillSet)
        {
            canChangeSkillSet = false;
            canPlayerState[4] = false;
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
                    cLife += 1;
                    break;
                case 3:
                    cMaxJumpCount = 1;
                    cSpeed = 8.0f;
                    break;
                default:
                    break;
            }
            canPlayerState[4] = true;
        }
    }
}

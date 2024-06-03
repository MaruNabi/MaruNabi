using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

        moveLeftKey = KeyCode.A;
        moveRightKey = KeyCode.D;
        jumpKey = KeyCode.Space;
        lockKey = KeyCode.LeftControl;
        sitKey = KeyCode.S;

        defaultPlayerColliderSize = playerCollider.size;
        sitPlayerColliderSize = defaultPlayerColliderSize;
        sitPlayerColliderSize.y -= 0.5f;

        ultimateGauge = 0.0f;
        currentHp = cLife;

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
            swordPrefab_1 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Bullet_" + 2);
            skillPrefab_1 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Skill_" + 2);
            swordPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Bullet_" + 3);
            skillPrefab_2 = Resources.Load<GameObject>("Prefabs/Player/Bullets/MARU_Skill_" + 3);
        }

        UpdateLifeUI();

        Managers.Pool.CreatePool(swordPrefab_1, 2);
        Managers.Pool.CreatePool(skillPrefab_1, 2);

        Managers.Pool.CreatePool(swordPrefab_2, 2);
        Managers.Pool.CreatePool(skillPrefab_2, 2);

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
    }

    void Update()
    {
        if (PauseUI.isGamePaused)
            return;

        SlopeCheck();

        SurfaceEffectorCheck();

        if (InputManager.isNeedInit)
        {
            InputManager.isNeedInit = false;
            OnPlayerInit();
        }

        if (moveHorizontal == 0 && !isDashing && !playerAnimator.GetBool("isDead") && !isSurfaceEffector)
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

        PlayerStateTransition(false);
        //Shield on
        yield return new WaitForSeconds(0.25f);
        //Shield Idle
        yield return new WaitForSeconds(0.5f);
        //Shield Off
        playerAnimator.SetBool("isDefence", false);
        yield return new WaitForSeconds(0.25f);
        PlayerStateTransition(true);
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

    protected override void OnPlayerAttack()
    {
        base.OnPlayerAttack();

        if (Input.GetKey(KeyCode.V)) //canPlayerState[4]
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
        else if (!(Input.GetKey(KeyCode.V)))
        {
            playerAnimator.SetBool("isAtk", false);
        }

        if (Input.GetKeyDown(KeyCode.B)) //canPlayerState[4]
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

                if (currentSkillPrefab.name == "MARU_Skill_1")
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
    }

    protected override void OnPlayerSkillChange()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && canChangeSkillSet)
        {
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

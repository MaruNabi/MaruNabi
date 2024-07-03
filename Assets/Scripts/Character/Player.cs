using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    protected bool characterID; //True : Maru, False : Nabi
    protected string characterName;
    public const int MAX_LIFE = 5;
    public int cLife = 3;
    [SerializeField] [Range(0, 10)] protected float cSpeed = 6.0f; //Character Speed
    protected bool[] canPlayerState = new bool[6]; //move, dash, sit, jump, atk, hit = 사용 끝나면 삭제
    protected const float maxUltimateGauge = 2500.0f;
    public static bool isReviveSuccess = false;
    private bool canHit = true;
    private bool isTimerEnd = false;
    private bool isCalledOnce = true;

    protected KeyCode moveLeftKey;
    protected KeyCode moveRightKey;
    protected KeyCode jumpKey;
    protected KeyCode lockKey;
    protected KeyCode sitKey;
    protected KeyCode normalAtkKey;
    protected KeyCode specialAtkKey;
    protected KeyCode skillChangeKey;
    protected KeyCode dashKey;

    private const float MINIMUM_JUMP = 12.0f;
    [SerializeField] [Range(0, 10)] protected float cJumpPower = 0.03f; //Incremental Jump Force
    [SerializeField] protected float cMaxJumpPower = 17.0f; //Maximum Jump Force
    protected float cMiniJumpPower = MINIMUM_JUMP; //Minimum Jump Force
    protected float cMaxJumpCount;
    protected float cJumpCount;
    protected bool isJumping = false; //Jumping State (Double Jump X)
    public bool IsJumping => isJumping;
    
    protected bool isJumpingEnd = true;
    protected bool isGround = true;
    protected bool isLock = false;

    private const float DOUBLE_CLICK_TIME = 0.2f;
    protected float lastClickTime = -1.0f;
    protected bool isDoubleClicked;

    private float cDashPower = 20.0f;
    private float cDashTime = 0.2f;
    private float cDashCooldown = 1.0f;
    protected bool isDashCoolEnd = true;
    protected bool isDashing = false;

    protected bool isSitting = false;
    protected Vector2 defaultPlayerColliderSize;
    protected Vector2 sitPlayerColliderSize;

    protected bool isInvincibleTime = false;

    public bool IsInvincibleTime => isInvincibleTime;

    protected bool isHit = false;

    [SerializeField] protected GameObject reviveZone;
    public GameObject ReviveZone => reviveZone;

    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    protected Animator playerAnimator;
    protected BoxCollider2D playerCollider;
    [SerializeField] protected BoxCollider2D playerStandCollider;
    [SerializeField] protected BoxCollider2D playerSideFrictionCollider;

    [SerializeField] protected Transform atkPosition;
    protected Vector2 defaultAtkPosition;
    protected Vector2 sitAtkPosition;

    protected GameObject reviveEffect;
    protected bool canFallDown;

    [SerializeField] private GameObject landingEffect;
    private bool isLandingEffectOnce = true;
    [SerializeField] private GameObject dashEffect;
    protected int dashDirection;

    protected float moveHorizontal = 0.0f;

    private bool pastKey;

    [SerializeField] protected Transform slopeCheckPosition;
    public LayerMask groundMask;
    [SerializeField] protected float slopeRayDistance;
    protected float slopeAngle;
    protected Vector2 slopePerp;
    protected bool isSlope;
    protected RaycastHit2D groundRay;
    protected string currentGroundName;
    protected bool isTargetGround;
    protected bool isSurfaceEffector;
    protected bool canChangeSkillSet;

    private bool isForcedInputChanged = false;
    private bool isInputChanged = false;

    public bool isPlayerDead { get; private set; }

    public bool IsTargetGround
    {
        set => isTargetGround = value;
    }

    public void PlayerStateTransition(bool _set, int _index = 4) //사용 끝나면 삭제
    {
        for (int i = _index; i < canPlayerState.Length; i++)
        {
            canPlayerState[i] = _set;
        }
    }

    public void PlayerHit(Vector2 _enemyPos, bool _isKnockBack = true)
    {
        if (isInvincibleTime)
            return;

        if (!canHit) //!canPlayerState[5]
            return;

        Managers.Sound.PlaySFX("Hit");
        isHit = true;
        //canPlayerState[0] = false;
        PlayerInputControl(false, OnPlayerMove);

        if (cLife > 1)
        {
            cLife -= 1;
            StartCoroutine(Ondamaged(_enemyPos, _isKnockBack));
        }
        else
        {
            cLife -= 1;
            StartCoroutine(Death());
        }
    }

    public void PlayerHitSpecial(Vector2 _enemyPos)
    {
        if (!canHit)
            return;

        Managers.Sound.PlaySFX("Hit");
        isHit = true;
        //canPlayerState[0] = false;
        PlayerInputControl(false, OnPlayerMove);

        if (cLife > 1)
        {
            cLife -= 1;
            StartCoroutine(OnSpecialDamaged(_enemyPos));
        }
        else
        {
            cLife -= 1;
            StartCoroutine(Death());
        }
    }

    public void PlayerInputControl(bool _isSet, params System.Action[] _actions)
    {
        if (!isForcedInputChanged)
        {
            if (_isSet)
            {
                for (int i = 0; i < _actions.Length; i++)
                {
                    Managers.Input.keyAction += _actions[i];
                }
            }
            else
            {
                for (int i = 0; i < _actions.Length; i++)
                {
                    Managers.Input.keyAction -= _actions[i];
                }
            }
        }
    }

    public void PlayerInputEnable()
    {
        if (isInputChanged)
        {
            PlayerInputControl(true, OnPlayerMove, OnPlayerAttack, OnPlayerDash, OnPlayerJump, OnPlayerSit, OnPlayerSkillChange);
            isInputChanged = false;
        }
    }

    public void PlayerInputDisable()
    {
        PlayerInputControl(false, OnPlayerMove, OnPlayerAttack, OnPlayerDash, OnPlayerJump, OnPlayerSit, OnPlayerSkillChange);
        isInputChanged = true;
    }

    public void PlayerForcedInputEnable()
    {
        if (isForcedInputChanged)
        {
            Managers.Input.keyAction += OnPlayerMove;
            Managers.Input.keyAction += OnPlayerAttack;
            Managers.Input.keyAction += OnPlayerDash;
            Managers.Input.keyAction += OnPlayerJump;
            Managers.Input.keyAction += OnPlayerSit;
            Managers.Input.keyAction += OnPlayerSkillChange;
            canHit = true;
            isForcedInputChanged = false;
        }
    }

    public void PlayerForcedInputDisable()
    {
        Managers.Input.keyAction -= OnPlayerMove;
        Managers.Input.keyAction -= OnPlayerAttack;
        Managers.Input.keyAction -= OnPlayerDash;
        Managers.Input.keyAction -= OnPlayerJump;
        Managers.Input.keyAction -= OnPlayerSit;
        Managers.Input.keyAction -= OnPlayerSkillChange;
        canHit = false;
        isForcedInputChanged = true;
    }

    public IEnumerator PlayerUpAnimation(float _remainingTime)
    {
        playerAnimator.SetBool("isUp", true);
        playerAnimator.SetBool("isDown", false);
        yield return new WaitForSeconds(_remainingTime);
        playerAnimator.SetBool("isUp", false);
        playerAnimator.SetBool("isDown", false);
    }

    public IEnumerator PlayerDownAnimation(float _remainingTime)
    {
        playerAnimator.SetBool("isUp", false);
        playerAnimator.SetBool("isDown", true);
        yield return new WaitForSeconds(_remainingTime);
        playerAnimator.SetBool("isUp", false);
        playerAnimator.SetBool("isDown", false);
    }

    protected void SurfaceEffectorCheck()
    {
        if (isGround && groundRay.collider.GetComponent<SurfaceEffector2D>())
        {
            isSurfaceEffector = groundRay.collider.GetComponent<SurfaceEffector2D>().enabled;
        }
        else
            isSurfaceEffector = false;
    }

    protected void SlopeCheck()
    {
        groundRay = Physics2D.Raycast(slopeCheckPosition.position, Vector2.down, slopeRayDistance, groundMask);

        if (groundRay)
        {
            if (groundRay.collider.tag == "Ground" && currentGroundName == groundRay.collider.gameObject.name)
            {
                isGround = true;
                if (isLandingEffectOnce)
                {
                    //canPlayerState[3] = true;
                    isLandingEffectOnce = false;
                    isJumpingEnd = true;
                    isJumping = false;
                    cJumpCount = 0;
                    cMiniJumpPower = MINIMUM_JUMP;
                    Managers.Sound.PlaySFX("Landing");
                    Instantiate(landingEffect, transform.position, transform.rotation);
                }
            }
            else if (groundRay.collider.tag == "Ground" && currentGroundName != groundRay.collider.gameObject.name)
            {
                isGround = true; //false
                //cJumpCount = 0;
                playerStandCollider.isTrigger = false;
                playerSideFrictionCollider.isTrigger = false;
                canFallDown = groundRay.collider.gameObject.GetComponent<GroundObject>().canFallDown;
            }

            slopePerp = Vector2.Perpendicular(groundRay.normal).normalized;
            slopeAngle = Vector2.Angle(groundRay.normal, Vector2.up);

            if (slopeAngle != 0)
                isSlope = true;
            else
                isSlope = false;

            Debug.DrawLine(groundRay.point, groundRay.point + groundRay.normal, Color.blue);
        }
        else
        {
            isGround = false;
            isLandingEffectOnce = true;
            isSurfaceEffector = false;
            if (1 < cMaxJumpCount)
                isJumping = false;
            else
                isJumping = true;
            //canPlayerState[3] = false;
        }
    }

    protected void PlayerJump(float jumpPower)
    {
        rigidBody.AddForce(new Vector3(0, jumpPower, 0), ForceMode2D.Impulse);
        isJumping = true;
    }

    protected void PlayerJumping(float jumpPower)
    {
        rigidBody.AddForce(new Vector3(0, jumpPower, 0), ForceMode2D.Impulse);
    }

    protected virtual void OnPlayerMove()
    {
        moveHorizontal = 0.0f;

        /*if (Input.GetKey(moveLeftKey))
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
        }*/
    }

    protected void PlayerMovement()
    {
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        if (movement.magnitude > 1)
            movement.Normalize();

        movement *= cSpeed;

        Vector3 velocityYOnly = new Vector3(0.0f, rigidBody.velocity.y, 0.0f);

        if (isSurfaceEffector)
        {
            if (moveHorizontal == 0.0f)
                return;
            else if (moveHorizontal > 0)
            {
                //rigidBody.AddForce((movement).normalized / 4, ForceMode2D.Impulse);
                rigidBody.velocity = movement * 0.05f + velocityYOnly;
            }
            else
            {
                rigidBody.velocity = movement * 3.0f + velocityYOnly;
            }
        }
        else
            rigidBody.velocity = movement + velocityYOnly;
    }

    public void ForcedPlayerMoveToRight()
    {
        if (isTargetGround)
            return;

        moveHorizontal = 1.0f;

        transform.rotation = Quaternion.Euler(0, 180, 0);
        atkPosition.rotation = Quaternion.Euler(0, 180, 0);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        if (movement.magnitude > 1)
            movement.Normalize();

        movement *= cSpeed;

        Vector3 velocityYOnly = new Vector3(0.0f, rigidBody.velocity.y, 0.0f);

        if (isSlope)
            movement *= slopePerp * -1f;

        rigidBody.velocity = movement + velocityYOnly;
    }

    protected void OnPlayerJump()
    {
        //Debug.Log("isjumping " + isJumping);
        //Debug.Log("issitting " + isSitting);
        //Debug.Log("jumpcount " + cJumpCount);
        //Debug.Log("isLock " + isLock);

        if (Input.GetKeyDown(jumpKey) && !isJumping && !isSitting && cJumpCount < cMaxJumpCount && !isLock) //canPlayerState[3]
        {
            Debug.Log("Jump");
            rigidBody.velocity = Vector2.zero;
            Managers.Sound.PlaySFX("Jump");
            PlayerJump(cMiniJumpPower);
            isJumpingEnd = false;
            cJumpCount++;
        }

        //JumpAddForce
        if (Input.GetKey(jumpKey) && !isJumpingEnd && !isSitting && !isLock)
        {
            if (cMiniJumpPower < cMaxJumpPower)
            {
                PlayerJumping(cJumpPower);
                cMiniJumpPower += cJumpPower;
            }
        }
        else if (!(Input.GetKey(jumpKey)))
        {
            isJumping = false;
        }
    }

    protected virtual void OnPlayerAttack()
    {
        if (Input.GetKeyDown(lockKey))
        {
            isLock = true;
        }
        else if (!(Input.GetKey(lockKey)))
        {
            isLock = false;
        }
    }

    protected virtual void OnPlayerDash()
    {
        /*if (Input.GetKeyDown(moveLeftKey) && !isSitting && !isLock && isDashCoolEnd) //canPlayerState[1]
        {
            DoubleClickDash(true);
        }

        if (Input.GetKeyDown(moveRightKey) && !isSitting && !isLock && isDashCoolEnd) //canPlayerState[1]
        {
            DoubleClickDash(false);
        }*/
    }

    protected void OnPlayerSit()
    {
        if (Input.GetKeyDown(sitKey) && !isJumping) //canPlayerState[2]
        {
            StartCoroutine(PlayerSit());
        }
        else if (!(Input.GetKey(sitKey)) && isSitting) //Input.GetKeyUp(KeyCode.DownArrow)
        {
            isSitting = false;
            //canPlayerState[0] = true;
            //canPlayerState[1] = true;
            playerAnimator.SetBool("isSit", false);
        }

        if (Input.GetKey(sitKey) && Input.GetKeyDown(jumpKey) && isSitting && !isLock)
        {
            if (canFallDown)
            {
                playerStandCollider.isTrigger = true;
                playerSideFrictionCollider.isTrigger = true;
                playerAnimator.SetBool("isDown", true);
            }
        }
    }

    protected virtual void OnPlayerSkillChange()
    {

    }

    protected void DoubleClickDash(bool key) 
    {
        if ((Time.time - lastClickTime) < DOUBLE_CLICK_TIME && pastKey == key)
        {
            isDoubleClicked = true;
            lastClickTime = -1.0f;
            StartCoroutine(PlayerDash());
        }
        else
        {
            isDoubleClicked = false;
            lastClickTime = Time.time;
        }

        pastKey = key;
    }

    protected IEnumerator Death()
    {
        isTimerEnd = false;
        isReviveSuccess = false;
        isCalledOnce = true;
        if (playerAnimator.GetBool("isHit"))
            playerAnimator.SetBool("isHit", false);
        playerAnimator.SetBool("isDead", true);
        reviveZone.SetActive(true);
        PlayerForcedInputDisable(); //PlayerStateTransition(false, 0);
        Invoke("InvokeTimer", 10.0f);

        while (!isTimerEnd) //wait 10 seconds
        {
            if (isReviveSuccess)
            {
                if (isCalledOnce)
                {
                    isCalledOnce = false;

                    StartCoroutine(Revive());
                    CancelInvoke("InvokeTimer");

                    yield break;
                }
            }

            yield return null;
        }

        if (!isReviveSuccess)
        {
            //Real Dead
            //Destroy(gameObject);
            Managers.Sound.PlaySFX("Dead");
            isPlayerDead = true;
            this.gameObject.SetActive(false);
            reviveZone.SetActive(false);
        }
    }

    public bool GetIsDeadBool()
    {
        return playerAnimator.GetBool("isDead");
    }

    private void InvokeTimer()
    {
        isTimerEnd = true;
    }

    protected virtual IEnumerator Revive()
    {
        reviveZone.SetActive(false);
        Managers.Sound.PlaySFX("Revive");
        //canPlayerState[0] = true;
        cLife = 1;
        PlayerForcedInputEnable(); //PlayerStateTransition(true, 0);
        playerAnimator.SetBool("isDead", false);
        Instantiate(reviveEffect, transform);
        StartCoroutine(Invincible(3.0f));
        yield return null;
    }

    public void ReviveCheat()
    {
        isReviveSuccess = true;
        cLife = 5;
    }

    protected IEnumerator Ondamaged(Vector2 enemyPos, bool _isKnockBack = true)
    {
        if (isHit)
        {
            playerAnimator.SetBool("isHit", true);
            StartCoroutine(Invincible(3.0f));
            if (_isKnockBack)
            {
                int dir = transform.position.x - enemyPos.x > 0 ? 1 : -1;
                rigidBody.AddForce(new Vector2(dir, 1) * 4f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            playerAnimator.SetBool("isHit", false);
            isHit = false;
            if (!playerAnimator.GetBool("isDead"))
                PlayerInputControl(true, OnPlayerMove);//canPlayerState[0] = true;
            yield return new WaitForSeconds(2.5f);
        }
    }

    protected IEnumerator OnSpecialDamaged(Vector2 enemyPos)
    {
        if (isHit)
        {
            playerAnimator.SetBool("isHit", true);
            StartCoroutine(Invincible(3.0f));
            int dir = transform.position.x - enemyPos.x > 0 ? 1 : -1;
            //rigidBody.AddForce(new Vector2(dir, 1) * 4f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.5f);
            playerAnimator.SetBool("isHit", false);
            isHit = false;
            if (!playerAnimator.GetBool("isDead"))
                PlayerInputControl(true, OnPlayerMove); //canPlayerState[0] = true;
            yield return new WaitForSeconds(2.5f);
        }
    }

    private IEnumerator Invincible(float invincibleTime)
    {
        isInvincibleTime = true;
        StartCoroutine(BlinkEffect(invincibleTime, spriteRenderer));
        yield return new WaitForSeconds(invincibleTime);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        isInvincibleTime = false;
    }

    private IEnumerator BlinkEffect(float blinkTime, SpriteRenderer spriteRenderer)
    {
        float remainingTime = 0.0f;
        float startTime = Time.time;

        while (remainingTime < blinkTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.15f);
            remainingTime = Time.time - startTime;
        }
    }

    protected IEnumerator PlayerDash()
    {
        playerAnimator.SetBool("isDash", true);
        Managers.Sound.PlaySFX("Dash");
        PlayerInputControl(false, OnPlayerMove, OnPlayerAttack, OnPlayerDash, OnPlayerJump, OnPlayerSit);
        //canPlayerState[1] = false;
        isDashing = true;
        moveHorizontal = 0.0f;
        isDashCoolEnd = false;
        Instantiate(dashEffect, transform.position, dashEffect.transform.rotation);
        dashEffect.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 90);
        float originalGravity = rigidBody.gravityScale;
        dashDirection = 0;
        rigidBody.gravityScale = 0f;
        //rigidBody.velocity = Vector2.zero;
        if (transform.rotation.y == 0)
        {
            dashDirection = -1;
        }
        else
        {
            dashDirection = 1;
        }
        if (isSurfaceEffector && dashDirection == 1)
            dashDirection *= 2;
        //rigidBody.velocity = new Vector2(dashDirection * 20, 0.0f);
        yield return new WaitForSeconds(cDashTime);
        rigidBody.velocity = Vector2.zero;
        rigidBody.gravityScale = originalGravity;
        isDashing = false;
        playerAnimator.SetBool("isDash", false);
        PlayerInputControl(true, OnPlayerMove, OnPlayerAttack, OnPlayerJump, OnPlayerSit);
        yield return new WaitForSeconds(cDashCooldown);
        isDashCoolEnd = true;
        if (!isSitting)
            PlayerInputControl(true, OnPlayerDash);
        //canPlayerState[1] = true;
    }

    protected IEnumerator PlayerSit(bool isTimeLimit = false, float time = 0.5f)
    {
        isSitting = true;
        playerAnimator.SetBool("isSit", true);
        PlayerInputControl(false, OnPlayerJump, OnPlayerDash);
        //canPlayerState[1] = false;
        //canPlayerState[0] = false;
        defaultAtkPosition = atkPosition.transform.localPosition;
        sitAtkPosition = defaultAtkPosition;
        sitAtkPosition.y = defaultAtkPosition.y - 0.7f;
        playerCollider.size = sitPlayerColliderSize;
        atkPosition.transform.localPosition = sitAtkPosition;
        //if (canPlayerState[1])
        //{
            //canPlayerState[1] = false;
        //}
        if (isDashCoolEnd)
        {
            Managers.Input.keyAction -= OnPlayerDash;
        }

        if (isTimeLimit)
        {
            yield return new WaitForSeconds(time);
            isSitting = false;
            //canPlayerState[1] = true;
            //canPlayerState[0] = true;
            playerAnimator.SetBool("isSit", false);
        }

        yield return new WaitUntil(() => isSitting == false);
        PlayerInputControl(true, OnPlayerJump, OnPlayerDash);
        playerCollider.size = defaultPlayerColliderSize;
        atkPosition.transform.localPosition = defaultAtkPosition;
        playerAnimator.SetBool("isSit", false);
    }
}
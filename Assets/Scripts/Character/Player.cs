using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected bool characterID;                       //True : Maru, False : Nabi
    protected string characterName;
    public const int MAX_LIFE = 3;
    protected int cLife = 3;
    [SerializeField]
    [Range(0, 10)]
    protected float cSpeed = 6.0f;                     //Character Speed
    protected bool canMove = true;
    protected const float maxUltimateGauge = 1500.0f;
    public static bool isReviveSuccess = false;
    private bool isTimerEnd = false;

    private const float MINIMUM_JUMP = 12.0f;
    [SerializeField]
    [Range(0, 10)]
    protected float cJumpPower = 0.03f;               //Incremental Jump Force
    [SerializeField]
    protected float cMaxJumpPower = 17.0f;            //Maximum Jump Force
    protected float cMiniJumpPower = MINIMUM_JUMP;    //Minimum Jump Force
    protected bool isJumping = false;                 //Jumping State (Double Jump X)
    protected bool isJumpingEnd = true;
    protected bool isGround = true;

    private const float DOUBLE_CLICK_TIME = 0.2f;
    protected float lastClickTime = -1.0f;
    protected bool isDoubleClicked;

    private float cDashPower = 20.0f;
    private float cDashTime = 0.2f;
    private float cDashCooldown = 2.0f;
    protected bool canDash = true;
    protected bool isDashing = false;

    protected bool canSit = true;
    protected bool isSitting = false;
    protected Vector2 defaultPlayerColliderSize;
    protected Vector2 sitPlayerColliderSize;

    protected bool canAtk = true;
    protected bool canJump = true;

    protected bool isInvincibleTime = false;
    protected bool isHit = false;

    [SerializeField]
    protected GameObject reviveZone;

    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    protected Animator playerAnimator;
    protected BoxCollider2D playerCollider;

    protected float moveHorizontal = 0.0f;

    private bool pastKey;

    protected float Charging(float minimumCharging, float maximumCharging, float addCharging) //minimum, maximum, incremental
    {
        if (minimumCharging < maximumCharging)
        {
            minimumCharging += addCharging;
        }

        return minimumCharging;
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

    protected virtual void PlayerMove()
    {
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        if (movement.magnitude > 1)
            movement.Normalize();

        movement *= cSpeed;

        Vector3 velocityYOnly = new Vector3(0.0f, rigidBody.velocity.y, 0.0f);

        rigidBody.velocity = movement + velocityYOnly;
    }

    protected void DoubleClickDash(bool key)
    {
        if ((Time.time - lastClickTime) < DOUBLE_CLICK_TIME && pastKey == key)
        {
            isDoubleClicked = true;
            lastClickTime = -1.0f;
            playerAnimator.SetBool("isDash", true);
            StartCoroutine(PlayerDash());
        }
        else
        {
            isDoubleClicked = false;
            lastClickTime = Time.time;
        }
        pastKey = key;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            isJumpingEnd = true;
            isGround = true;
            cMiniJumpPower = MINIMUM_JUMP;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

    protected IEnumerator Death()
    {
        canMove = false;
        isTimerEnd = false;
        isReviveSuccess = false;
        playerAnimator.SetBool("isDead", true);
        reviveZone.SetActive(true);
        Invoke("InvokeTimer", 10.0f);
        
        while (!isTimerEnd) //wait 10 seconds
        {
            if (isReviveSuccess)
            {
                StartCoroutine(Revive());
                CancelInvoke("InvokeTimer");
                
                yield break;
            }
            yield return null;
        }

        if (!isReviveSuccess)
        {
            this.gameObject.SetActive(false);
            reviveZone.SetActive(false);
        }
    }

    private void InvokeTimer()
    {
        isTimerEnd = true;
    }

    protected virtual IEnumerator Revive()
    {
        reviveZone.SetActive(false);
        canMove = true;
        cLife = 1;
        playerAnimator.SetBool("isDead", false);
        StartCoroutine(Invincible(3.0f));
        yield return null;
    }

    protected IEnumerator Ondamaged(Vector2 enemyPos)
    {
        if (isHit)
        {
            playerAnimator.SetBool("isHit", true);
            StartCoroutine(Invincible(3.0f));
            int dir = transform.position.x - enemyPos.x > 0 ? 1 : -1;
            rigidBody.AddForce(new Vector2(dir, 1) * 4f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.5f);
            playerAnimator.SetBool("isHit", false);
            isHit = false;
            canMove = true;
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

    private IEnumerator PlayerDash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0f;
        if (transform.rotation.y == 0)
        {
            rigidBody.velocity = new Vector2(-cDashPower, 0.0f);
        }
        else
        {
            rigidBody.velocity = new Vector2(cDashPower, 0.0f);
        }
        //무적 넣으려면 여기~
        yield return new WaitForSeconds(cDashTime);
        rigidBody.gravityScale = originalGravity;
        isDashing = false;
        playerAnimator.SetBool("isDash", false);
        yield return new WaitForSeconds(cDashCooldown);
        canDash = true;
    }

    protected IEnumerator PlayerSit(bool isTimeLimit = false, float time = 0.5f)
    {
        isSitting = true;
        playerAnimator.SetBool("isSit", true);
        canDash = false;
        canMove = false;
        playerCollider.size = sitPlayerColliderSize;
        if (canDash)
        {
            canDash = false;
        }
        if (isTimeLimit)
        {
            yield return new WaitForSeconds(time);
            isSitting = false;
            canDash = true;
            canMove = true;
            playerAnimator.SetBool("isSit", false);
        }
        yield return new WaitUntil(() => isSitting == false);
        playerCollider.size = defaultPlayerColliderSize;
    }
}

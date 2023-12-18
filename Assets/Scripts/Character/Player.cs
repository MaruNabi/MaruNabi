using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected const float MINIMUM_JUMP = 12.0f;
    protected bool characterID;                       //True : Maru, False : Nabi
    protected string characterName;
    protected int cLife;                              //Character Health
    [SerializeField]
    [Range(0, 10)]
    protected float cSpeed = 6.0f;                     //Character Speed
    public static float ultimateGauge;
    protected float maxUltimateGauge;
    [SerializeField]
    [Range(0, 10)]
    protected float cJumpPower = 0.03f;                //Incremental Jump Force
    [SerializeField]
    protected float cMaxJumpPower = 17.0f;            //Maximum Jump Force
    protected float cMiniJumpPower = MINIMUM_JUMP;    //Minimum Jump Force
    protected float reviveTime = 0.0f;
    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    protected bool isJumping = false;                 //Jumping State (Double Jump X)
    protected float moveHorizontal = 0.0f;
    protected Animator playerAnimation;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            cMiniJumpPower = MINIMUM_JUMP;
        }
    }
}

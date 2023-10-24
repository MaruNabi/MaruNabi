using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float MINIMUM_JUMP = 12.0f;
    [SerializeField]
    private bool characterID;                       //True : Maru, False : Nabi
    private int cLife;                              //Character Health
    [Range(0, 10)]
    public float cSpeed = 6.0f;                     //Character Speed
    [Range(0, 10)]
    public float cJumpPower = 0.03f;                //Jump Power
    [SerializeField]
    private float cMaxJumpPower = 17.0f;            //Maximum Jump Force
    private float cMiniJumpPower = MINIMUM_JUMP;    //Minimum Jump Force
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private bool isJumping = false;                 //Jumping State (Double Jump X)
    /*public GameObject bullet;
    public Transform pos;*/

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() //Jump Key Code가 달라서 함수로 분할하지 않음
    {
        if (characterID) //Maru Jump
        {
            if (Input.GetKey(KeyCode.R) && !isJumping)
            {
                if (cMiniJumpPower <= cMaxJumpPower)
                {
                    cMiniJumpPower += cJumpPower;
                }
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                rigidBody.AddForce(new Vector3(0, cMiniJumpPower, 0), ForceMode2D.Impulse);
                isJumping = true;
                cMiniJumpPower = MINIMUM_JUMP;
            }
        }

        if (!characterID) //Nabi Jump
        {
            if (Input.GetKey(KeyCode.Comma) && !isJumping)
            {
                if (cMiniJumpPower <= cMaxJumpPower)
                {
                    cMiniJumpPower += cJumpPower;
                }
            }

            if (Input.GetKeyUp(KeyCode.Comma))
            {
                rigidBody.AddForce(new Vector3(0, cMiniJumpPower, 0), ForceMode2D.Impulse);
                isJumping = true;
                cMiniJumpPower = MINIMUM_JUMP;
            }
        }
    }
    void FixedUpdate() //Movement Key Code가 달라서 함수로 분할하지 않음, 후에 추가할 가능성 있음
    {
        float moveHorizontal = 0.0f;

        if (characterID) //Maru Movement
        {
            if (Input.GetKey(KeyCode.A))
            {
                moveHorizontal = -1.0f;
                spriteRenderer.flipX = false;
            }

            if (Input.GetKey(KeyCode.D))
            {
                moveHorizontal = 1.0f;
                spriteRenderer.flipX = true;
            }
        }

        if (!characterID) //Nabi Movement
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveHorizontal = -1.0f;
                spriteRenderer.flipX = false;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveHorizontal = 1.0f;
                spriteRenderer.flipX = true;
            }
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        if (movement.magnitude > 1)
            movement.Normalize();
        //1 기준 move

        movement *= cSpeed;

        Vector3 velocityYOnly = new Vector3(0.0f, rigidBody.velocity.y, 0.0f);
        //중력 및 점프를 위한 velocityYOnly Vector

        rigidBody.velocity = movement + velocityYOnly;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isJumping = false;
    }
}

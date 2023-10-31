using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNabi : MonoBehaviour
{
    private const float MINIMUM_JUMP = 12.0f;
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
    public GameObject bulletPrefab;                 //Bullet Prefab
    public Transform bulletPosition;                //Bullet Instantiate Position
    public float coolTime;
    private float curTime;
    private bool isLock = false;                    //Lock

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isLock)
        {
            if (Input.GetKey(KeyCode.Comma) && !isJumping)
            {
                if (cMiniJumpPower <= cMaxJumpPower)
                {
                    cMiniJumpPower += cJumpPower;
                }
            }

            if (Input.GetKeyUp(KeyCode.Comma) && !isJumping)
            {
                rigidBody.AddForce(new Vector3(0, cMiniJumpPower, 0), ForceMode2D.Impulse);
                isJumping = true;
                cMiniJumpPower = MINIMUM_JUMP;
            }
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            isLock = true;
            Debug.Log("Locked");
        }
        
        else if (Input.GetKeyUp(KeyCode.Slash))
        {
            isLock = false;
            Debug.Log("Unlocked");
        }

        if (curTime <= 0)
        {
            if (Input.GetKey(KeyCode.Period))
            {
                Instantiate(bulletPrefab, bulletPosition.position, transform.rotation);
            }
            curTime = coolTime;
        }
        curTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        float moveHorizontal = 0.0f;

        if (!isLock)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveHorizontal = -1.0f;
                //spriteRenderer.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveHorizontal = 1.0f;
                //spriteRenderer.flipX = true;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        
        else if(isLock)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveHorizontal = 0.0f;
                //spriteRenderer.flipX = false;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveHorizontal = 0.0f;
                //spriteRenderer.flipX = true;
                transform.rotation = Quaternion.Euler(0, 180, 0);
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

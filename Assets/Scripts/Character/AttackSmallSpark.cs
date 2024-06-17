using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSmallSpark : Bullet
{
    private const float SPARK_ATTACK_POWER = 80.0f;

    private bool isRelease = false;
    private bool isSetOnce = true;
    private float bulletAngle;
    private int direction;

    private SpriteRenderer sparkSpriteRenderer;

    private Color originColor = new Color(1, 1, 1, 1);
    private Color disableColor = new Color(1, 1, 1, 0);

    private GameObject sparkStartPosition;
    private GameObject playerPosition;

    private void Start()
    {
        sparkSpriteRenderer = GetComponent<SpriteRenderer>();
        sparkStartPosition = GameObject.Find("NabiBulletPosition");
        playerPosition = GameObject.Find("Nabi_Test");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightBracket))
        {
            //Charging Effect
            if (isSetOnce)
            {
                sparkSpriteRenderer.color = disableColor;
            }
        }

        if (Input.GetKeyUp(KeyCode.RightBracket))
        {
            if (isSetOnce)
            {
                if (PlayerNabi.isNabiTraitActivated)
                    attackPower = SPARK_ATTACK_POWER * 2.3f;
                else
                    attackPower = SPARK_ATTACK_POWER;
                isSetOnce = false;
                SetBullet();
                shootEffect = Resources.Load<GameObject>("Prefabs/VFX/Player/15Sprites/Shotgun");
                sparkSpriteRenderer.color = originColor;

                transform.position = sparkStartPosition.transform.position;

                if (playerPosition.transform.rotation.y == 0)
                {
                    bulletAngle = 180;
                    direction = -1;
                }
                else
                {
                    bulletAngle = 0;
                    direction = 1;
                }
                transform.rotation = Quaternion.Euler(0, bulletAngle, 0);

                isRelease = true;
            }
        }

        if (isRelease)
        {
            PlayShootEffect();

            AttackInstantiate();
        }
    }

    private void OnDisable()
    {
        isRelease = false;
        isSetOnce = true;
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        SmallSparkMovement();

        ColliderCheck(true);
    }

    private void SmallSparkMovement()
    {
        if (lockedBulletVector.magnitude == 0)
        {
            bulletRigidbody.velocity = new Vector2(speed * direction, 0);
        }

        else
        {
            bulletRigidbody.velocity = lockedBulletVector * speed;
            bulletAngle = Mathf.Atan2(lockedBulletVector.y, lockedBulletVector.x) * Mathf.Rad2Deg;
            bulletAngle %= 360f;
            transform.rotation = Quaternion.Euler(0, 0, bulletAngle);
        }
    }
}
